using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Types.UnitEvent UnitEvent;

    public Dictionary<int, int> aa;

    public TeamData TeamData;
    public UnitData UnitData;
    public AttackTargetData AttackTargetData;
    public AttackData AttackData;

    public UnitBody UnitBody;
    public Vector2 UnitSize;
    public Vector3 UnitCenterOffset;
    public UnitFSM UnitFSM;

    public Waypoint TargetWaypoint;
    public Waypoint WaitWaypoint;

    public Unit AttackTargetUnit;
    public Vector3 LastAttackTargetPosition;
    public GameObject ProjectileSpawnPosition;

    // todo create UnitStatus
    public float Health = 20;
    public float Speed = 2f;
    protected float _velocity = 0f;
    public bool IsDied { get; private set; }
    public CCData TakenCCData;

    protected void Awake()
    {
        gameObject.layer = Mathf.RoundToInt(Mathf.Log(AttackTargetData.LayerMask.value, 2f));//LayerMask를 LayerIndex로 변환

        TakenCCData = new CCData();

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if(boxCollider != null)
        {
            //UnitCenterOffset = boxCollider.offset;
            UnitSize = boxCollider.size;
        }
    }

    protected void Start()
    {
        Health = UnitData.Health;

        if (TargetWaypoint == null)
        {
            TargetWaypoint = WaypointManager.Inst.WaypointPool.Get();
            TargetWaypoint.transform.position = transform.position;// 현재 위치로 설정
        }
    }

    private void OnApplicationQuit()
    {
        CleanUpUnit();
    }

    private void OnDestroy()
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

    private void CleanUpUnit()
    {
        if (TargetWaypoint != null)
        {
            WaypointManager.Inst.WaypointPool.Release(TargetWaypoint);
            TargetWaypoint = null;
        }

        UnitFSM.ClearnUp();
    }

    public void DispatchUnitEvent(Types.UnitEventType unitEventType, Unit unit)
    {
        if(UnitEvent == null)
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
                _AddAttackTargetUnit(notifyUnit);
                UnitFSM.Transit(Types.UnitFSMType.Wait);
                break;
            case Types.UnitNotifyType.Attack:
                _AddAttackTargetUnit(notifyUnit);
                UnitFSM.Transit(Types.UnitFSMType.Attack);
                break;
            default:
                break;
        }
    }

    private void _AddAttackTargetUnit(Unit unit)
    {
        if (AttackTargetUnit != null)
        {
            _RemoveAttackTargetUnit();
        }
        AttackTargetUnit = unit;
        AttackTargetUnit.UnitEvent += _OnUnitEventHandler_AttackTargetUnit;
    }

    private void _RemoveAttackTargetUnit()
    {
        if (AttackTargetUnit == null)
        {
            Debug.LogAssertion("AttackTargetUnit == null " + this);
        }
        LastAttackTargetPosition = AttackTargetUnit.GetCenterPosition();

        AttackTargetUnit.UnitEvent -= _OnUnitEventHandler_AttackTargetUnit;
        AttackTargetUnit = null;
    }

    private void _OnUnitEventHandler_AttackTargetUnit(Types.UnitEventType unitEventType, Unit unit)
    {
        switch (unitEventType)
        {
            case Types.UnitEventType.None:
                break;
            case Types.UnitEventType.AttackStart:
                break;
            case Types.UnitEventType.AttackEnd:
                break;
            case Types.UnitEventType.Attack:
                break;
            case Types.UnitEventType.Die:
                _RemoveAttackTargetUnit();
                break;
            case Types.UnitEventType.DiedComplete:
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

        if (Health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.layer = 0; // 타겟으로 검색되지 않도록 LayerMask 초기화
        IsDied = true;
        DispatchUnitEvent(Types.UnitEventType.Die, this);

        if (AttackTargetUnit != null)
        {
            AttackTargetUnit.UnitEvent -= _OnUnitEventHandler_AttackTargetUnit;
        }
    }

    virtual public Unit FindAttackTarget()
    {
        // 이미 공격목표가 있다
        if (AttackTargetUnit != null)
        {
            return AttackTargetUnit;
        }

        Unit draftTargetUnit = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + UnitCenterOffset, UnitData.TargetRange, AttackTargetData.AttackTargetLayerMask);
        if (colliders.Length == 0) return draftTargetUnit;

        // todo 가장 목표지점에 가까운 적을 찾는다.
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];
            Unit unit = collider.gameObject.GetComponent<Unit>();
            // 사망하거나 전투중이 아닌 유닛만 공격대상으로 한다
            if (unit.IsDied == false && unit.AttackTargetUnit == null)
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
                    else if (draftTargetUnit.TargetWaypoint.OrderNumber < unit.TargetWaypoint.OrderNumber)
                    {
                        draftTargetUnit = unit;
                    }
                }
            }
        }

        if (draftTargetUnit != null)
        {
            _AddAttackTargetUnit(draftTargetUnit);
        }

        return draftTargetUnit;
    }

    virtual public void MoveTo(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        float distance = direction.magnitude;

        _velocity = Mathf.Min(distance, UnitData.Speed * Time.deltaTime);
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
            float distance = Vector3.Distance(transform.position, WaitWaypoint.transform.position);
            bool arrived = distance < Consts.ArriveDistance;
            return arrived;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (AttackTargetUnit != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, AttackTargetUnit.transform.position);
        }
    }

    public Vector3 GetCenterPosition()
    {
        return transform.position + UnitCenterOffset;
    }
}
