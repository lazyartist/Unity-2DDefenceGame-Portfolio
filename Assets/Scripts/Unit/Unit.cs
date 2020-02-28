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
    public Waypoint RallyWaypoint;
    public int WaypointSubIndex = 0;

    public Unit EnemyUnit { get; private set; }
    public Vector3 LastEnemyPosition;
    public GameObject SpawnPosition;

    public float Velocity = 0f;
    public Vector3 MoveDirection;

    // todo create UnitStatus
    public float Health = 20;
    public float Speed = 2f;
    public bool IsDied = false;
    public CCData TakenCCData;
    public bool CanChangeDirection = true;
    public bool GoalComplete = false;

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
        if (RallyWaypoint != null)
        {
            WaypointManager.Inst.WaypointPool.Release(RallyWaypoint);
            RallyWaypoint = null;
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
                AddEnemyUnit(notifyUnit);
                UnitFSM.Transit(Types.UnitFSMType.Wait);
                break;
            case Types.UnitNotifyType.BeAttackState:
                AddEnemyUnit(notifyUnit);
                UnitFSM.Transit(Types.UnitFSMType.Attack);
                break;
            case Types.UnitNotifyType.ClearEnemyUnit:
                if (HasEnemyUnit())
                {
                    DispatchUnitEvent(Types.UnitEventType.AttackStopped, null);
                    RemoveEnemyUnit();
                }
                break;
            default:
                break;
        }
    }

    void AddEnemyUnit(Unit unit)
    {
        if (HasEnemyUnit())
        {
            RemoveEnemyUnit();
        }
        EnemyUnit = unit;
        EnemyUnit.UnitEvent += OnUnitEventHandler_EnemyUnit;
    }

    void RemoveEnemyUnit()
    {
        if (HasEnemyUnit() == false)
        {
            Debug.LogAssertion("HasEnemyUnit() == false " + this);
            Debug.Break();
        }
        LastEnemyPosition = EnemyUnit.GetCenterPosition();

        EnemyUnit.UnitEvent -= OnUnitEventHandler_EnemyUnit;
        EnemyUnit = null;
    }

    void OnUnitEventHandler_EnemyUnit(Types.UnitEventType unitEventType, Unit unit)
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
                RemoveEnemyUnit();
                break;
            case Types.UnitEventType.DiedComplete:
                break;
            case Types.UnitEventType.AttackStopped:// 공격대상 유닛이 공격을 멈추고 다른 행동을 한다.
                RemoveEnemyUnit();
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

        if (HasEnemyUnit())
        {
            EnemyUnit.UnitEvent -= OnUnitEventHandler_EnemyUnit;
        }
    }

    virtual public Unit TryFindEnemy()
    {
        // 이미 공격목표가 있다
        if (HasEnemyUnit())
        {
            return EnemyUnit;
        }

        Vector3 findPosition;
        if(RallyWaypoint != null)
        {
            // 랠리포인트 중심으로 적을 찾는다
            findPosition = RallyWaypoint.GetPosition(WaypointSubIndex);
        } else
        {
            findPosition = transform.position + UnitCenterOffset;
        }
        Unit draftTargetUnit = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(findPosition, UnitData.TargetRange, EnemyLayerMask);
        if (colliders.Length == 0) return draftTargetUnit;

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];
            Unit unit = collider.gameObject.GetComponent<Unit>();
            // 사망하거나 전투중이 아닌 유닛만 공격대상으로 한다
            if (unit.IsDied == false && unit.HasEnemyUnit() == false)
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
            AddEnemyUnit(draftTargetUnit);
        }

        return draftTargetUnit;
    }

    virtual public void MoveTo(Vector3 position)
    {
        MoveDirection = position - transform.position;
        float distance = MoveDirection.magnitude;

        Velocity = Mathf.Min(distance, UnitData.MoveSpeed * Time.deltaTime);
        float velocity = Velocity;

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
        transform.position = transform.position + (MoveDirection.normalized * velocity);
    }

    public bool IsArrivedRallyPoint()
    {
        if (RallyWaypoint != null)
        {
            float distance = Vector3.Distance(transform.position, RallyWaypoint.GetPosition(WaypointSubIndex));
            bool arrived = distance < Consts.ArriveDistance;
            return arrived;
        }
        return false;
    }

    public bool HasEnemyUnit()
    {
        return EnemyUnit != null;
    }

    public bool IsValidEnemyUnit()
    {
        return HasEnemyUnit() && EnemyUnit.IsDied == false;
    }

    public bool IsValidEnemyUnitInRange()
    {
        if (HasEnemyUnit() == false) return false;

        float distance = Vector3.Distance(transform.position + UnitCenterOffset, EnemyUnit.transform.position);
        return distance < UnitData.TargetRange;
    }

    public Vector3 GetCenterPosition()
    {
        return transform.position + UnitCenterOffset;
    }

    public void SetRallyPoint(Vector3 position)
    {
        if (RallyWaypoint == null)
        {
            RallyWaypoint = WaypointManager.Inst.WaypointPool.Get();
        }
        if (TargetWaypoint == null)
        {
            TargetWaypoint = WaypointManager.Inst.WaypointPool.Get();
        }
        RallyWaypoint.transform.position = position;
        TargetWaypoint.transform.position = position;
        ClearEnemyUnit();
    }

    public void ClearEnemyUnit()
    {
        Notify(Types.UnitNotifyType.ClearEnemyUnit, null);
    }
}
