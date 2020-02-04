using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Types.UnitEvent UnitEvent;

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
        UnitCenterOffset = boxCollider.offset;
        UnitSize = boxCollider.size;
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
        LastAttackTargetPosition = AttackTargetUnit.GetCenterPosition();

        AttackTargetUnit.UnitEvent -= _OnUnitEventHandler_AttackTargetUnit;
        AttackTargetUnit = null;
    }

    private void _OnUnitEventHandler_AttackTargetUnit(Types.UnitEventType unitEventType)
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
        if (UnitEvent != null)
        {
            UnitEvent(Types.UnitEventType.Die);
        }
        else
        {
            Debug.LogAssertion("UnitEvent != null " + this);
            Debug.Break();
        }
    }

    virtual public Unit FindAttackTarget()
    {
        Unit draftTargetUnit = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, UnitData.TargetRange, AttackTargetData.AttackTargetLayerMask);
        if (colliders.Length == 0) return draftTargetUnit;

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];

            if (collider.tag == Consts.tagUnit && collider.gameObject.GetComponent<Unit>().IsDied == false)
            {
                Unit unit = collider.gameObject.GetComponent<Unit>();

                // 기존 타겟과 동일한 객체가 있으면 타겟을 변경하지 않는다.
                if (AttackTargetUnit == unit.gameObject)
                {
                    return draftTargetUnit;
                }

                if (draftTargetUnit == null && unit.IsDied == false)
                {
                    // 임시로 타겟을 하나 정한다.
                    draftTargetUnit = unit;
                }
            }
        }

        // 기존 타겟과 같은 객체가 발견되지 않았으니 새로운 타겟을 지정한다.
        _AddAttackTargetUnit(draftTargetUnit);

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
