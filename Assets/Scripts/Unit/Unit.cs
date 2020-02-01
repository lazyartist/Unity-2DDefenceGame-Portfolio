using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool AutoAttack = false;
    public bool AutoMoveToTarget = false;
    public bool AutoMoveToWaypoint = false;
    public bool AutoMoveToWaitingPosition = false;

    public bool ShowHpBar = true;

    public UnitBody UnitBody;
    //public UnitAttackArea UnitAttackArea;

    public Types.TeamType TeamType;
    public Color ATeamColor;
    public Color BTeamColor;

    public SpriteRenderer HpBarBgSR;
    public SpriteRenderer HpBarGaugeSR;

    public Vector2 UnitSize;
    public Vector3 UnitCenterOffset;

    //public float MaxHealth = 20;
    public float Health = 20;
    public float Speed = 2f;

    //public float AttackPower = 2f;
    //public float AttackCoolTime = 2f;
    //public float TargetRange = 5f;
    //public float AttackRange = 5f;

    //[SerializeField]
    public UnitData UnitData;
    public AttackData AttackData;

    [SerializeField]
    public CCData TakenCCData;
    //private float _elasedCCTime = 0f;

    //private AttackData _takenAttackData;
    //private float _damageElasedTime = 0f;

    //public AttackData AttackData;

    public Waypoint TargetWaypoint;
    public Waypoint WaitWaypoint;
    public Vector3 WaitingPosition;

    public Unit AttackTargetUnit { get; set; }
    //public Unit AttackTargetUnit { get; private set; }

    protected bool _Attackable = false;
    protected float _elapsedAttackTime;
    protected float _velocity = 0f;
    protected float _velocityAddition = 0f;

    public bool IsDied { get; private set; }
    //public bool IsDamaged;

    // FSM
    public Waypoint TargetWaypoint2;
    public UnitFSM UnitFSM;
    
    // FSM ==========

    protected void Awake()
    {
        TakenCCData = new CCData();

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        UnitCenterOffset = boxCollider.offset;
        UnitSize = boxCollider.size;

        UnitBody.UnitEventListener += OnUnitEventListener;
    }

    void OnUnitEventListener(Types.UnitEventType unitEventType)
    {
        Debug.Log("UnitEventListener " + unitEventType);

        switch (unitEventType)
        {
            case Types.UnitEventType.None:
                break;
            case Types.UnitEventType.Attack:
                Attack();
                break;
            case Types.UnitEventType.DiedComplete:
                DiedComplete();
                break;
            default:
                break;
        }
    }

    protected void Start()
    {
        Health = UnitData.Health;

        if (TargetWaypoint2 == null)
        {
            TargetWaypoint2 = WaypointManager.Inst.WaypointPool.Get();
            TargetWaypoint2.transform.position = transform.position;// 현재 위치로 설정
        }

        // 팀 컬러 설정
        switch (TeamType)
        {
            case Types.TeamType.A:
                HpBarGaugeSR.color = ATeamColor;
                break;
            case Types.TeamType.B:
                HpBarGaugeSR.color = BTeamColor;
                break;
            default:
                break;
        }

        HpBarBgSR.enabled = ShowHpBar;
        HpBarGaugeSR.enabled = ShowHpBar;
    }

    private void OnApplicationQuit()
    {
        ReleaseWaypoint();
    }

    private void OnDestroy()
    {
        ReleaseWaypoint();
    }

    void ReleaseWaypoint()
    {
        if (TargetWaypoint2 != null)
        {
            WaypointManager.Inst.WaypointPool.Release(TargetWaypoint2);
            TargetWaypoint2 = null;
        }
    }

    protected void Update()
    {
        // update cc
        if(TakenCCData.CCType != Types.CCType.None)
        {
            TakenCCData.CCTime -= Time.deltaTime;
            if(TakenCCData.CCTime <= 0)
            {
                TakenCCData.CCType = Types.CCType.None;
            }
        }
    }

    //protected void Update222()
    //{
    //    _velocityAddition = 0f;

    //    if (IsDied == true)
    //    {
    //        _velocity = 0f;
    //    }
    //    else
    //    {
    //        //if (_takenAttackData != null)
    //        //{
    //            //if (_damageElasedTime >= _takenAttackData.CCTime)
    //            //{
    //            //    _takenAttackData = null;
    //            //    _damageElasedTime = 0f;
    //            //}
    //            //else
    //            //{
    //            //    _damageElasedTime += Time.deltaTime;
    //            //    switch (_takenAttackData.CCType)
    //            //    {
    //            //        case Consts.CCType.None:
    //            //            break;
    //            //        case Consts.CCType.Stun:
    //            //            // 스턴 상태이므로 이동, 공격 불가
    //            //            _velocity = 0;
    //            //            return;
    //            //        case Consts.CCType.Slow:
    //            //            // 슬로우이므로 이동 속도 감소, 공격 가능
    //            //            _velocityAddition = _takenAttackData.CCValue;
    //            //            break;
    //            //        //break;
    //            //        default:
    //            //            break;
    //            //    }
    //            //}
    //        //}

    //        if (_Attackable && AutoAttack && AttackTargetUnit != null)
    //        {
    //            Toward(AttackTargetUnit.transform.position);
    //            _elapsedAttackTime += Time.deltaTime;
    //            if (_elapsedAttackTime >= UnitData.AttackCoolTime)
    //            {
    //                PlayAttack();
    //                _elapsedAttackTime = 0;
    //            }

    //            _velocity = 0f;
    //        }
    //        else if (AutoMoveToTarget && AttackTargetUnit != null)
    //        {
    //            Toward(AttackTargetUnit.transform.position);
    //            MoveToAttackTarget();
    //        }
    //        else if (AutoMoveToWaypoint && TargetWaypoint != null)
    //        {
    //            Toward(TargetWaypoint.transform.position);
    //            MoveToWaypoint();

    //            // 웨이포인트에 다다르면 다음 웨이포인트를 타겟 웨이포인트로 지정
    //            float distance = Vector3.Distance(TargetWaypoint.transform.position, transform.position);
    //            if (distance < 0.01f)
    //            {
    //                TargetWaypoint = TargetWaypoint.NextWaypoint;
    //            }
    //        }
    //        else if (AutoMoveToWaitingPosition)
    //        {
    //            Toward(WaitingPosition);
    //            MoveToWaitingPosition();
    //        }
    //        else
    //        {
    //            _velocity = 0f;
    //        }
    //    }

    //    UnitBody.Animator.SetFloat("Velocity", _velocity);
    //}

    //protected void FixedUpdate()
    //{
    //    FindAttackTarget();

    //    UpdateAttackable();
    //}

    //virtual protected bool UpdateAttackable()
    //{
    //    _Attackable = AttackTargetUnit != null && UnitAttackArea.TargetUnit != null && AttackTargetUnit.gameObject == UnitAttackArea.TargetUnit.gameObject;
    //    return _Attackable;
    //}

    virtual public void Toward(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        UnitBody.Toward(direction);
        //UnitBody.Toward(direction.x < 0);
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

        if (Health < 0f)
        {
            Health = 0f;
        }

        //UnitBody.Animator.SetFloat("Health", Health);

        if (Health <= 0f)
        {
            IsDied = true;
            //UnitBody.Animator.SetTrigger("Die");
        }
        //else if (Health <= 0f)
        //{
            //switch (_ccData.CCType)
            //{
            //    case Consts.CCType.None:
            //        break;
            //    case Consts.CCType.Stun:
            //        UnitBody.Animator.SetTrigger("Hurt");
            //        break;
            //    case Consts.CCType.Slow:
            //        break;
            //    default:
            //        break;
            //}
        //}
        // todo hp ui 분리
        HpBarGaugeSR.size = new Vector2(HpBarBgSR.size.x * Health / UnitData.Health, HpBarGaugeSR.size.y);
    }

    //virtual public void FindAttackTarget()
    //{
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, UnitData.TargetRange, Consts.lmUnit);
    //    if (colliders.Length == 0) return;

    //    GameObject draftTarget = null;
    //    //AttackTargetUnit = null;
    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        Collider2D collider = colliders[i];

    //        if (this.gameObject == collider.gameObject) continue;

    //        if (collider.tag == Consts.tUnit && collider.gameObject.GetComponent<Unit>().IsDied == false)
    //        {
    //            Unit unit = collider.gameObject.GetComponent<Unit>();

    //            // 기존 타겟과 동일한 객체가 있으면 타겟을 변경하지 않는다.
    //            if(AttackTargetUnit == unit.gameObject)
    //            {
    //                return;
    //            }

    //            if (draftTarget == null && unit.TeamType != TeamType && unit.IsDied == false)
    //            {
    //                // 임시로 타겟을 하나 정한다.
    //                draftTarget = collider.gameObject;
    //                //AttackTargetUnit = collider.gameObject;
    //                //break;
    //            }
    //        }
    //    }

    //    // 기존 타겟과 같은 객체가 발견되지 않았으니 새로운 타겟을 지정한다.
    //    AttackTargetUnit = draftTarget;
    //}

    virtual public Unit FindAttackTarget2()
    {
        Unit draftTargetUnit = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, UnitData.TargetRange, Consts.lmUnit);
        if (colliders.Length == 0) return draftTargetUnit;

        //AttackTargetUnit = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];

            if (this.gameObject == collider.gameObject) continue;

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
                    //draftTargetUnit = collider.gameObject;
                    //AttackTargetUnit = collider.gameObject;
                    //break;
                }
            }
        }

        // 기존 타겟과 같은 객체가 발견되지 않았으니 새로운 타겟을 지정한다.
        AttackTargetUnit = draftTargetUnit;

        return draftTargetUnit;
    }

    virtual public bool IsAttackTargetInAttackArea()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + AttackData.AttackArea.offset, AttackData.AttackArea.size, 0.0f, Consts.lmUnit);
        if (colliders.Length == 0) return false;

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];
            if (this.gameObject == collider.gameObject) continue;

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

    virtual protected void MoveToAttackTarget()
    {
        if (_Attackable == false && AttackTargetUnit != null)
        {
            MoveTo(AttackTargetUnit.transform.position);
        }
    }

    virtual protected void MoveToWaypoint()
    {
        if (TargetWaypoint != null)
        {
            MoveTo(TargetWaypoint.transform.position);
        }
    }

    //virtual protected void MoveToWaitingPosition()
    //{
    //    MoveTo(WaitingPosition);
    //}

    virtual public void MoveTo(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        float distance = direction.magnitude;

        _velocity = Mathf.Min(distance, UnitData.Speed * Time.deltaTime);
        float velocity = _velocity + _velocityAddition;

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

    protected void PlayAttack()
    {
        if (AttackTargetUnit == null) return;

        UnitBody.Animator.SetTrigger("Attack");
    }

    virtual public void Attack()
    {
        if (AttackTargetUnit != null && AttackTargetUnit.IsDied == false)
        {
            AttackTargetUnit.TakeDamage(AttackData);
        }
        //if (UnitAttackArea.TargetUnit != null)
        //{
        //    UnitAttackArea.TargetUnit.Damage(AttackData);
        //}
    }

    public void DiedComplete()
    {
        Destroy(this.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Consts.tagUnit)
        {
            Unit unit = collision.gameObject.GetComponent<Unit>();

            if (unit.TeamType != TeamType)
            {
                _elapsedAttackTime = UnitData.AttackCoolTime;
                _Attackable = true;
            }

            if (transform.position.y < unit.gameObject.transform.position.y && UnitBody.SpriteRenderer.sortingOrder <= unit.UnitBody.SpriteRenderer.sortingOrder)
            {
                UnitBody.SpriteRenderer.sortingOrder = unit.UnitBody.SpriteRenderer.sortingOrder + 1;
                HpBarBgSR.sortingOrder = unit.UnitBody.SpriteRenderer.sortingOrder + 2;
                HpBarGaugeSR.sortingOrder = unit.UnitBody.SpriteRenderer.sortingOrder + 3;
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Consts.tagUnit)
        {
            Unit unit = collision.gameObject.GetComponent<Unit>();

            if (unit.TeamType != TeamType)
            {
                _Attackable = true;
            }

            if (transform.position.y < unit.gameObject.transform.position.y && UnitBody.SpriteRenderer.sortingOrder <= unit.UnitBody.SpriteRenderer.sortingOrder)
            {
                UnitBody.SpriteRenderer.sortingOrder = unit.UnitBody.SpriteRenderer.sortingOrder + 1;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Consts.tagUnit && collision.gameObject.GetComponent<Unit>().TeamType != TeamType)
        {
            _Attackable = false;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("OnTriggerEnter2D Unit");
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log("OnTriggerStay2D Unit");
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    Debug.Log("OnTriggerExit2D Unit");
    //}

    private void OnDrawGizmos()
    {
        if (AttackTargetUnit != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, AttackTargetUnit.transform.position);
        }
    }
}
