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
    public UnitAttackArea UnitAttackArea;

    public Consts.TeamType TeamType;
    public Color ATeamColor;
    public Color BTeamColor;

    public SpriteRenderer HpBarBgSR;
    public SpriteRenderer HpBarGaugeSR;

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

    private AttackData _damageAttackData;
    private float _damageElasedTime = 0f;

    //public AttackData AttackData;

    public Waypoint TargetWaypoint;
    public Vector3 WaitingPosition;

    public GameObject AttackTargetUnit { get; private set; }

    protected bool _Attackable = false;
    protected float _elapsedAttackTime;
    protected float _velocity = 0f;
    protected float _velocityAddition = 0f;

    public bool IsDied { get; private set; }

    protected void Awake()
    {
        //_animationSR = GetComponent<SpriteRenderer>();
        //_animator = GetComponent<Animator>();
        //_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
        Health = UnitData.Health;

        switch (TeamType)
        {
            case Consts.TeamType.A:
                HpBarGaugeSR.color = ATeamColor;
                break;
            case Consts.TeamType.B:
                HpBarGaugeSR.color = BTeamColor;
                break;
            default:
                break;
        }

        HpBarBgSR.enabled = ShowHpBar;
        HpBarGaugeSR.enabled = ShowHpBar;
    }

    protected void Update()
    {
        _velocityAddition = 0f;

        if (IsDied == true)
        {
            _velocity = 0f;
        }
        else
        {
            if(_damageAttackData != null)
            {
                if(_damageElasedTime >= _damageAttackData.SkillTime)
                {
                    _damageAttackData = null;
                    _damageElasedTime = 0f;
                }
                else
                {
                    _damageElasedTime += Time.deltaTime;
                    switch (_damageAttackData.SkillType)
                    {
                        case Consts.SkillType.None:
                            break;
                        case Consts.SkillType.Stun:
                            // 스턴 상태이므로 이동, 공격 불가
                            _velocity = 0;
                            return;
                        case Consts.SkillType.Slow:
                            // 슬로우이므로 이동 속도 감소, 공격 가능
                            _velocityAddition = _damageAttackData.SkillValue;
                            break;
                        //break;
                        default:
                            break;
                    }
                }
            }


            if (_Attackable && AutoAttack && AttackTargetUnit != null)
            {
                Toward(AttackTargetUnit.transform.position);
                _elapsedAttackTime += Time.deltaTime;
                if (_elapsedAttackTime >= UnitData.AttackCoolTime)
                {
                    PlayAttack();
                    _elapsedAttackTime = 0;
                }

                _velocity = 0f;
            }
            else if (AutoMoveToTarget && AttackTargetUnit != null)
            {
                Toward(AttackTargetUnit.transform.position);
                MoveToAttackTarget();
            }
            else if (AutoMoveToWaypoint && TargetWaypoint != null)
            {
                Toward(TargetWaypoint.transform.position);
                MoveToWaypoint();

                // 웨이포인트에 다다르면 다음 웨이포인트를 타겟 웨이포인트로 지정
                float distance = Vector3.Distance(TargetWaypoint.transform.position, transform.position);
                if (distance < 0.01f)
                {
                    TargetWaypoint = TargetWaypoint.NextWaypoint;
                }
            }
            else if (AutoMoveToWaitingPosition)
            {
                Toward(WaitingPosition);
                MoveToWaitingPosition();
            }
            else
            {
                _velocity = 0f;
            }
        }

        UnitBody.Animator.SetFloat("Velocity", _velocity);
    }

    protected void FixedUpdate()
    {
        FindAttackTarget();

        UpdateAttackable();
    }

    virtual protected bool UpdateAttackable()
    {
        _Attackable = AttackTargetUnit != null && UnitAttackArea.TargetUnit != null && AttackTargetUnit.gameObject == UnitAttackArea.TargetUnit.gameObject;
        return _Attackable;
    }

    virtual protected void Toward(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        UnitBody.Toward(direction.x < 0);
    }

    public void Damage(AttackData damageAttackData)
    {
        // 현재 스킬 데미지 없음
        if(_damageAttackData == null)
        {
            //if (damageAttackData.SkillType == Consts.SkillType.Stun)
            if (damageAttackData.SkillType != Consts.SkillType.None)
            {
                _damageAttackData = damageAttackData;
                _damageElasedTime = 0f;
            }
        }
        // 현재 스킬 데미지를 받고 있음
        else
        {
            // 이미 스턴 상태이면 추가 스턴 없음, 데미지 있음
            if (_damageAttackData.SkillType == Consts.SkillType.Stun)
            {
                //_damageAttackData = damageAttackData;
                //_damageElasedTime = 0f;
            }
            // 이미 스턴 상태가 아니므로 스턴, 데미지 있음
            else if (damageAttackData.SkillType == Consts.SkillType.Stun)
            {
                _damageAttackData = damageAttackData;
                _damageElasedTime = 0f;
            }
            //else
            //{
            //    _damageAttackData = null;
            //    _damageElasedTime = 0f;
            //}
        }
        

        if (IsDied) return;

        Health -= damageAttackData.Power;

        if (Health < 0f)
        {
            Health = 0f;
        }

        UnitBody.Animator.SetFloat("Health", Health);

        if (Health <= 0f)
        {
            IsDied = true;
            UnitBody.Animator.SetTrigger("Die");
        }
        else
        {
            UnitBody.Animator.SetTrigger("Hit");
        }

        HpBarGaugeSR.size = new Vector2(HpBarBgSR.size.x * Health / UnitData.Health, HpBarGaugeSR.size.y);
    }

    virtual protected void FindAttackTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, UnitData.TargetRange, Consts.lmUnit);
        if (colliders.Length == 0) return;

        GameObject draftTarget = null;
        //AttackTargetUnit = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];

            if (this.gameObject == collider.gameObject) continue;

            if (collider.tag == Consts.tUnit && collider.gameObject.GetComponent<Unit>().IsDied == false)
            {
                Unit unit = collider.gameObject.GetComponent<Unit>();

                // 기존 타겟과 동일한 객체가 있으면 타겟을 변경하지 않는다.
                if(AttackTargetUnit == unit.gameObject)
                {
                    return;
                }
                
                if (draftTarget == null && unit.TeamType != TeamType && unit.IsDied == false)
                {
                    // 임시로 타겟을 하나 정한다.
                    draftTarget = collider.gameObject;
                    //AttackTargetUnit = collider.gameObject;
                    //break;
                }
            }
        }

        // 기존 타겟과 같은 객체가 발견되지 않았으니 새로운 타겟을 지정한다.
        AttackTargetUnit = draftTarget;
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

    virtual protected void MoveToWaitingPosition()
    {
        MoveTo(WaitingPosition);
    }

    virtual protected void MoveTo(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        float distance = direction.magnitude;

        _velocity = Mathf.Min(distance, UnitData.Speed * Time.deltaTime);
        float velocity = _velocity + _velocityAddition;
        if (velocity < 0)
        {
            velocity = 0f;
        }
        transform.position = transform.position + (direction.normalized * velocity);
    }

    protected void PlayAttack()
    {
        if (AttackTargetUnit == null) return;

        UnitBody.Animator.SetTrigger("Attack");
    }

    virtual public void Attack()
    {
        if (UnitAttackArea.TargetUnit != null)
        {
            UnitAttackArea.TargetUnit.Damage(AttackData);
        }
    }

    public void DiedComplete()
    {
        Destroy(this.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Consts.tUnit)
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
        if (collision.gameObject.tag == Consts.tUnit)
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
        if (collision.gameObject.tag == Consts.tUnit && collision.gameObject.GetComponent<Unit>().TeamType != TeamType)
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
        if(AttackTargetUnit != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, AttackTargetUnit.transform.position);
        }
    }
}
