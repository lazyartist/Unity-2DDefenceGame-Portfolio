using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Types.TeamType TeamType;
    public UnitBody UnitBody;
    public LayerMask EnemyTeamLayerMask;
    public TeamData TeamData;

    public Vector2 UnitSize;
    public Vector3 UnitCenterOffset;

    public UnitData UnitData;
    public AttackData AttackData;

    public Unit AttackTargetUnit { get; set; }
    public Waypoint WaitWaypoint;

    public float Health = 20;
    public float Speed = 2f;
    protected float _velocity = 0f;

    public CCData TakenCCData;
    
    public bool IsDied { get; private set; }

    public Waypoint TargetWaypoint;
    public UnitFSM UnitFSM;

    protected void Awake()
    {
        gameObject.layer = Mathf.RoundToInt(Mathf.Log(TeamData.TeamLayerMask[(int)TeamType].value, 2f));//LayerMask를 LayerIndex로 변환
        EnemyTeamLayerMask = TeamData.EnemyTeamLayerMask[(int)TeamType];

        TakenCCData = new CCData();

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        UnitCenterOffset = boxCollider.offset;
        UnitSize = boxCollider.size;

        UnitBody.UnitBodyEventListener += OnUnitBodyEventListener;
    }

    void OnUnitBodyEventListener(Types.UnitBodyEventType unitBodyEventType)
    {
        Debug.Log("UnitEventListener " + unitBodyEventType);

        switch (unitBodyEventType)
        {
            case Types.UnitBodyEventType.None:
                break;
            case Types.UnitBodyEventType.Attack:
                Attack();
                break;
            case Types.UnitBodyEventType.DiedComplete:
                DiedComplete();
                break;
            default:
                break;
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
        if (UnitBody != null && UnitBody.UnitBodyEventListener != null)
        {
            UnitBody.UnitBodyEventListener -= OnUnitBodyEventListener;
        }

        if (TargetWaypoint != null)
        {
            WaypointManager.Inst.WaypointPool.Release(TargetWaypoint);
            TargetWaypoint = null;
        }
    }

    public void Notify(Types.UnitNotifyType unitNotifyType, Unit notifyUnit)
    {
        switch (unitNotifyType)
        {
            case Types.UnitNotifyType.None:
                break;
            case Types.UnitNotifyType.Wait:
                AttackTargetUnit = notifyUnit;
                UnitFSM.Transit(Types.UnitFSMType.Wait);
                break;
            case Types.UnitNotifyType.Attack:
                AttackTargetUnit = notifyUnit;
                UnitFSM.Transit(Types.UnitFSMType.Attack);
                break;
            case Types.UnitNotifyType.AttackTargetUnitDied:
                AttackTargetUnit = null;
                //UnitFSM.Transit(Types.UnitFSMType.Idle);
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

        //if (Health < 0f)
        //{
        //    Health = 0f;
        //}

        if (Health <= 0f)
        {
            IsDied = true;
        }
    }

    virtual public Unit FindAttackTarget()
    {
        Unit draftTargetUnit = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, UnitData.TargetRange, EnemyTeamLayerMask);
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

                if (draftTargetUnit == null && unit.TeamType != TeamType && unit.IsDied == false)
                {
                    // 임시로 타겟을 하나 정한다.
                    draftTargetUnit = unit;
                }
            }
        }

        // 기존 타겟과 같은 객체가 발견되지 않았으니 새로운 타겟을 지정한다.
        AttackTargetUnit = draftTargetUnit;

        return draftTargetUnit;
    }

    virtual public bool IsAttackTargetInAttackArea()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + AttackData.AttackArea.offset, AttackData.AttackArea.size, 0.0f, EnemyTeamLayerMask);
        if (colliders.Length == 0) return false;

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];

            Unit unit = collider.gameObject.GetComponent<Unit>();
            if (collider.tag == Consts.tagUnit && unit.IsDied == false)
            {
                if (AttackTargetUnit == unit)
                {
                    return true;
                }
            }
        }
        return false;
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

    virtual public void Attack()
    {
        if (AttackTargetUnit != null && AttackTargetUnit.IsDied == false)
        {
            AttackTargetUnit.TakeDamage(AttackData);
        }
    }

    public void DiedComplete()
    {
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        if (AttackTargetUnit != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, AttackTargetUnit.transform.position);
        }
    }
}
