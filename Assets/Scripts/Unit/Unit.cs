using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Types.UnitEvent UnitEvent;

    public TeamData TeamData;
    public UnitData UnitData;
    public AttackData AttackData;

    public UnitBody UnitBody;
    public Vector2 UnitSize;
    public Vector3 UnitCenterOffset;
    public UnitFSM UnitFSM;
    public Animator HitEffectAnimator;

    public Waypoint TargetWaypoint;
    public int TargetWaypointSubIndex = 0;
    public Waypoint WaitWaypoint; // todo rename -> RallyPoint

    public Unit AttackTargetUnit { get; private set; }
    public Vector3 LastAttackTargetPosition;
    public GameObject SpawnPosition;

    // todo create UnitStatus
    public float Health = 20;
    public float Speed = 2f;
    public bool IsDied = false;
    public CCData TakenCCData;
    public bool CanChangeDirection = true;
    public bool GoalComplete = false;

    float _velocity = 0f;

    protected void Awake()
    {
        gameObject.name += Consts.GetUnitId();
        TakenCCData = new CCData();

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            UnitSize = boxCollider.size;
        }
    }

    public LayerMask EnemyLayerMask;
    protected void Start()
    {
        InitLayer();
        SetEnemyLayerMask(AttackData);
        Health = UnitData.Health;
    }

    void InitLayer()
    {
        if (TeamData.TeamType == Types.TeamType.None || UnitData.UnitType == Types.UnitType.None)
        {
            gameObject.layer = 0;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer(TeamData.TeamType.ToString() + UnitData.UnitType.ToString());
        }
    }

    void SetEnemyLayerMask(AttackData attackData)
    {
        EnemyLayerMask = 0;
        for (int i = 0; i < AttackData.TargetUnitTypes.Length; i++)
        {
            int mask = LayerMask.GetMask(TeamData.EnemyTeamType.ToString() + AttackData.TargetUnitTypes[i].ToString());
            EnemyLayerMask |= mask;
        }
    }

    void OnApplicationQuit()
    {
        CleanUpUnit();
    }

    void OnDestroy()
    {
        CleanUpUnit();
    }

    protected void Update()
    {
        // update cc
        if (TakenCCData.CCType != Types.CCType.None)
        {
            TakenCCData.CCTime -= Time.deltaTime;
            if (TakenCCData.CCTime <= 0)
            {
                TakenCCData.CCType = Types.CCType.None;
            }
        }
    }

    void CleanUpUnit()
    {
        if (TargetWaypoint != null)
        {
            WaypointManager.Inst.WaypointPool.Release(TargetWaypoint);
            TargetWaypoint = null;
        }
        if (WaitWaypoint != null)
        {
            WaypointManager.Inst.WaypointPool.Release(WaitWaypoint);
            WaitWaypoint = null;
        }

        UnitFSM.ClearnUp();
    }

    public void DispatchUnitEvent(Types.UnitEventType unitEventType, Unit unit)
    {
        if (UnitEvent == null)
        {
            // 이 유닛이 죽거나 이 유닛의 공격대상이 죽어서 등록된 이벤트핸들러가 모두 사라진 경우
            // 스플래시 데미지로 죽을 경우 나를 감시하는 유닛이 없을 수 있기 때문에 UnitEvent == null일 수 있다
        }
        else
        {
            UnitEvent(unitEventType, unit);
        }
    }

    public void Notify(Types.UnitNotifyType unitNotifyType, Unit notifyUnit)
    {
        switch (unitNotifyType)
        {
            case Types.UnitNotifyType.None:
                break;
            case Types.UnitNotifyType.Wait:
                AddAttackTargetUnit(notifyUnit);
                UnitFSM.Transit(Types.UnitFSMType.Wait);
                break;
            case Types.UnitNotifyType.BeAttackState:
                AddAttackTargetUnit(notifyUnit);
                UnitFSM.Transit(Types.UnitFSMType.Attack);
                break;
            case Types.UnitNotifyType.ClearAttackTarget:
                if (HasAttackTargetUnit())
                {
                    DispatchUnitEvent(Types.UnitEventType.AttackStopped, null);
                    RemoveAttackTargetUnit();
                }
                break;
            default:
                break;
        }
    }

    void AddAttackTargetUnit(Unit unit)
    {
        if (HasAttackTargetUnit())
        {
            RemoveAttackTargetUnit();
        }
        AttackTargetUnit = unit;
        AttackTargetUnit.UnitEvent += OnUnitEventHandler_AttackTargetUnit;
    }

    void RemoveAttackTargetUnit()
    {
        if (HasAttackTargetUnit() == false)
        {
            Debug.LogAssertion("HasAttackTargetUnit() == false " + this);
            Debug.Break();
        }
        LastAttackTargetPosition = AttackTargetUnit.GetCenterPosition();

        AttackTargetUnit.UnitEvent -= OnUnitEventHandler_AttackTargetUnit;
        AttackTargetUnit = null;
    }

    void OnUnitEventHandler_AttackTargetUnit(Types.UnitEventType unitEventType, Unit unit)
    {
        switch (unitEventType)
        {
            case Types.UnitEventType.None:
                break;
            case Types.UnitEventType.AttackStart:
                break;
            case Types.UnitEventType.AttackEnd:
                break;
            case Types.UnitEventType.AttackFire:
                break;
            case Types.UnitEventType.Die:
                RemoveAttackTargetUnit();
                break;
            case Types.UnitEventType.DiedComplete:
                break;
            case Types.UnitEventType.AttackStopped:// 공격대상 유닛이 공격을 멈추고 다른 행동을 한다.
                RemoveAttackTargetUnit();
                UnitFSM.Transit(Types.UnitFSMType.Idle);
                break;
            default:
                break;
        }
    }

    virtual public void Toward(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        UnitBody.Toward(direction);
    }

    public void TakeDamage(AttackData attackData)
    {
        if (IsDied) return;

        float damage = attackData.Power;

        // CC기 추가
        if (attackData.CCData != null && attackData.CCData.CCType != Types.CCType.None && TakenCCData.CCType == Types.CCType.None)
        {
            attackData.CCData.Copy(TakenCCData);
            switch (TakenCCData.CCType)
            {
                case Types.CCType.None:
                    break;
                case Types.CCType.Stun:
                    break;
                case Types.CCType.Slow:
                    break;
                default:
                    break;
            }
        }

        Health -= damage;
        if (HitEffectAnimator != null)
        {
            HitEffectAnimator.SetTrigger("Hit");
        }

        if (Health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        gameObject.layer = 0; // 타겟으로 검색되지 않도록 LayerMask 초기화
        IsDied = true;
        DispatchUnitEvent(Types.UnitEventType.Die, this);

        if (HasAttackTargetUnit())
        {
            AttackTargetUnit.UnitEvent -= OnUnitEventHandler_AttackTargetUnit;
        }
    }

    virtual public Unit FindAttackTarget()
    {
        // 이미 공격목표가 있다
        if (HasAttackTargetUnit())
        {
            return AttackTargetUnit;
        }

        Unit draftTargetUnit = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + UnitCenterOffset, UnitData.TargetRange, EnemyLayerMask);
        if (colliders.Length == 0) return draftTargetUnit;

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];
            Unit unit = collider.gameObject.GetComponent<Unit>();
            // 사망하거나 전투중이 아닌 유닛만 공격대상으로 한다
            if (unit.IsDied == false && unit.HasAttackTargetUnit() == false)
            {
                if (draftTargetUnit == null)
                {
                    draftTargetUnit = unit;
                }
                else
                {
                    if (unit.TargetWaypoint == null)
                    {
                        //
                    }
                    else if (draftTargetUnit.TargetWaypoint == null)
                    {
                        draftTargetUnit = unit;
                    }
                    // 가장 목표지점에 가까운 적을 찾는다.
                    else if (draftTargetUnit.TargetWaypoint.OrderNumber < unit.TargetWaypoint.OrderNumber)
                    {
                        draftTargetUnit = unit;
                    }
                }
            }
        }

        if (draftTargetUnit != null)
        {
            AddAttackTargetUnit(draftTargetUnit);
        }

        return draftTargetUnit;
    }

    virtual public void MoveTo(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        float distance = direction.magnitude;

        _velocity = Mathf.Min(distance, UnitData.MoveSpeed * Time.deltaTime);
        float velocity = _velocity;

        // cc
        switch (TakenCCData.CCType)
        {
            case Types.CCType.None:
                break;
            case Types.CCType.Stun:
                velocity = 0f;
                break;
            case Types.CCType.Slow:
                velocity *= TakenCCData.CCValue;
                break;
            default:
                break;
        }

        if (velocity < 0)
        {
            velocity = 0f;
        }
        transform.position = transform.position + (direction.normalized * velocity);
    }

    public bool IsArrivedWaitWaypoint()
    {
        if (WaitWaypoint != null)
        {
            float distance = Vector3.Distance(transform.position, WaitWaypoint.GetPosition(TargetWaypointSubIndex));
            bool arrived = distance < Consts.ArriveDistance;
            return arrived;
        }
        return false;
    }

    public bool HasAttackTargetUnit()
    {
        return AttackTargetUnit != null;
    }

    public bool IsValidAttackTargetUnit()
    {
        return HasAttackTargetUnit() && AttackTargetUnit.IsDied == false;
    }

    public bool IsValidAttackTargetUnitInRange()
    {
        if (HasAttackTargetUnit() == false) return false;

        float distance = Vector3.Distance(transform.position + UnitCenterOffset, AttackTargetUnit.transform.position);
        return distance < UnitData.TargetRange;
    }

    public Vector3 GetCenterPosition()
    {
        return transform.position + UnitCenterOffset;
    }

    public void SetRallyPoint(Vector3 position)
    {
        if (WaitWaypoint == null)
        {
            WaitWaypoint = WaypointManager.Inst.WaypointPool.Get();
        }
        if (TargetWaypoint == null)
        {
            TargetWaypoint = WaypointManager.Inst.WaypointPool.Get();
        }
        WaitWaypoint.transform.position = position;
        TargetWaypoint.transform.position = position;
        ClearAttackTargetUnit();
    }

    public void ClearAttackTargetUnit()
    {
        Notify(Types.UnitNotifyType.ClearAttackTarget, null);
    }
}
