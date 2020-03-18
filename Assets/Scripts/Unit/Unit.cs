using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Types.UnitEvent UnitEvent;

    // Data
    public TeamData TeamData;
    public UnitData UnitData;
    public AttackData[] ShortAttackDatas;
    // Body
    public UnitBody UnitBody;
    public Vector2 ColliderOffset;
    public Vector2 ColliderSize;
    public UnitCenter UnitCenter; // 유닛의 중심 || 공격 범위의 중심, 외부에서 넣어줄 수도 있다.
    public Animator HitEffectAnimator;
    public UnitFSM UnitFSM;
    public bool CanChangeDirection = true;
    // Move
    public UnitMovePoint UnitMovePoint;
    // todo move end
    public float Velocity { get; private set; }
    public Vector3 MoveDirection;
    // Enemy
    public Unit EnemyUnit { get; private set; }
    public Vector3 LastEnemyPosition;
    public GameObject SpawnPosition;
    // Status
    // todo create UnitStatus
    public float Health = 20;
    public CCData TakenCCData;
    public bool IsDied = false;
    public bool GoalComplete = false;
    public Types.UnitTargetRangeType UnitTargetRangeType;

    LayerMask[] _enemyLayerMasks = new LayerMask[(int)Types.UnitPlaceType.Count];
    IUnitRenderOrder unitRenderOrder;

    protected void Awake()
    {
        gameObject.name += Consts.GetUnitNumber();
        TakenCCData = new CCData();

        unitRenderOrder = GetComponent<IUnitRenderOrder>();

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            ColliderOffset = boxCollider.offset;
            ColliderSize = boxCollider.size;
        }

        // 초기는 RallyPoint로 설정
        UnitMovePoint.SetRallyPoint(transform.position);
    }

    protected void Start()
    {
        Init();
    }

    void InitLayer()
    {
        if (TeamData.TeamType == Types.TeamType.None)
        {
            gameObject.layer = 0;
        }
        else
        {
            // 
            gameObject.layer = LayerMask.NameToLayer(TeamData.TeamType.ToString() + UnitData.UnitPlaceType.ToString());
        }
    }

    LayerMask GetLayerMask(Types.TeamType teamType, Types.UnitPlaceType unitPlaceType)
    {
        int mask = LayerMask.GetMask(teamType.ToString() + unitPlaceType.ToString());
        return mask;
    }

    // todo 여러 번 불리지 않게 수정
    public void Init()
    {
        IsDied = false;
        GoalComplete = false;
        Health = UnitData.Health;
        InitLayer();
        unitRenderOrder.Init(UnitData.UnitSortingLayerType.ToString());
        UnitFSM.Reset();
        UnitBody.Reset();
        DispatchUnitEvent(Types.UnitEventType.Live);
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
        UnitFSM.ClearnUp();
    }

    public void DispatchUnitEvent(Types.UnitEventType unitEventType)
    {
        if (UnitEvent == null)
        {
            // 이 유닛이 죽거나 이 유닛의 공격대상이 죽어서 등록된 이벤트핸들러가 모두 사라진 경우
            // 스플래시 데미지로 죽을 경우 나를 감시하는 유닛이 없을 수 있기 때문에 UnitEvent == null일 수 있다
        }
        else
        {
            UnitEvent(unitEventType, this);
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
                    DispatchUnitEvent(Types.UnitEventType.AttackStopped);
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
        LastEnemyPosition = EnemyUnit.transform.position;
        EnemyUnit.UnitEvent += OnUnitEventHandler_EnemyUnit;
    }

    void RemoveEnemyUnit()
    {
        if (HasEnemyUnit() == false)
        {
            Debug.LogAssertion("HasEnemyUnit() == false " + this);
            Debug.Break();
        }
        LastEnemyPosition = EnemyUnit.transform.position;

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
        if (direction.x == 0f) return;

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
        DispatchUnitEvent(Types.UnitEventType.Die);
        // 사망 시 sortingLayer를 Ground로 바꿔준다.
        UnitBody.UnitSR.sortingLayerName = Types.UnitSortingLayerType.Unit_Ground.ToString();

        if (HasEnemyUnit())
        {
            EnemyUnit.UnitEvent -= OnUnitEventHandler_EnemyUnit;
        }
    }

    public void DiedComplete()
    {
        UnitPoolManager.Inst.Release(this);
        UnitBody.Reset();
    }

    public Vector3 GetFindPosition(Types.UnitTargetRangeType unitTargetRangeType)
    {
        Vector3 findPosition = Vector3.zero;

        switch (unitTargetRangeType)
        {
            case Types.UnitTargetRangeType.Short:
                {
                    // 근거리는 이동 방법에 따라 랠리포인트가 기준일지 유닛중심이 기준일지 결정한다.
                    if (UnitMovePoint.UnitMovePointType == Types.UnitMovePointType.RallyPoint)
                    {
                        findPosition = UnitMovePoint.RallyPoint;
                    }
                    else
                    {
                        findPosition = UnitCenter.transform.position;
                    }
                }
                break;
            case Types.UnitTargetRangeType.Long:
                // 원거리는 유닛중심을 기준으로 적을 찾는다.
                findPosition = UnitCenter.transform.position;
                break;
            //case Types.UnitTargetRangeType.Count:
            //break;
            default:
                break;
        }
        return findPosition;
    }

    virtual public bool TryFindEnemy()
    {
        // 이미 공격목표가 있다면 반환
        if (HasEnemyUnit() && EnemyUnit.IsDied == false)
        {
            return true;
        }

        // 근거리 타겟을 먼저 찾는다.
        if (TryFindShortRangeEnemy())
        {
            return true;
        }
        // 원거리 타겟을 찾는다.
        if (TryFindLongRangeEnemy())
        {
            return true;
        }

        // 타겟을 못 찾았다.
        SetUnitTargetRangeType(Types.UnitTargetRangeType.Short);
        return false;
    }

    public bool TryFindShortRangeEnemy()
    {
        Vector3 findPosition = GetFindPosition(Types.UnitTargetRangeType.Short);
        float targetRange = GetTargetRange(Types.UnitTargetRangeType.Short);
        Unit enemyUnit = null;
        if (targetRange > 0f)
        {
            enemyUnit = GetEnemyOrNull(findPosition, Consts.GetUnitLayerMask(TeamData.EnemyTeamType, Types.UnitPlaceType.Ground), targetRange);
            if (enemyUnit != null)
            {
                AddEnemyUnit(enemyUnit);
                SetUnitTargetRangeType(Types.UnitTargetRangeType.Short);
                return true;
            }
        }
        return false;
    }

    public bool TryFindLongRangeEnemy()
    {
        Vector3 findPosition = GetFindPosition(Types.UnitTargetRangeType.Long);
        Unit enemyUnit = null;
        float targetRange = GetTargetRange(Types.UnitTargetRangeType.Long);
        if (targetRange > 0f)
        {
            // 원거리 타겟중 공중유닛을 먼저 찾는다.
            enemyUnit = GetEnemyOrNull(findPosition, Consts.GetUnitLayerMask(TeamData.EnemyTeamType, Types.UnitPlaceType.Air), targetRange);
            if (enemyUnit == null)
            {
                // 원거리 타겟중 지상유닛을 찾는다.
                enemyUnit = GetEnemyOrNull(findPosition, Consts.GetUnitLayerMask(TeamData.EnemyTeamType, Types.UnitPlaceType.Ground), targetRange);
            }

            if (enemyUnit != null)
            {
                SetUnitTargetRangeType(Types.UnitTargetRangeType.Long);
                AddEnemyUnit(enemyUnit);
                return true;
            }
        }
        return false;
    }

    Unit GetEnemyOrNull(Vector3 findPosition, LayerMask layerMask, float targetRange)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(findPosition, targetRange, layerMask);
        if (colliders.Length == 0) return null;

        Unit targetUnit = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];
            Unit unit = collider.gameObject.GetComponent<Unit>();
            // 사망하거나 전투중이 아닌 유닛만 공격대상으로 한다
            if (unit.IsDied == false && unit.HasEnemyUnit() == false)
            {
                if (targetUnit == null)
                {
                    targetUnit = unit;
                }
                // 가장 목표지점에 가까운 적을 찾는다.
                // todo 적 찾기 정책
                else if (targetUnit.UnitMovePoint.UnitMovePointType == Types.UnitMovePointType.WayPoint
                    && targetUnit.UnitMovePoint.WayPoint.OrderNumber < unit.UnitMovePoint.WayPoint.OrderNumber)
                {
                    targetUnit = unit;
                }
            }
        }

        if (targetUnit != null)
        {
            AddEnemyUnit(targetUnit);
        }

        return targetUnit;
    }

    public void GoToEnemy()
    {
        // 공격대상에게 이동할 동안 대기하도록 통보
        EnemyUnit.Notify(Types.UnitNotifyType.Wait, this);
        // 적의 앞까지 이동
        float enemyDirection = Mathf.Sign(EnemyUnit.MoveDirection.x);
        Vector3 targetPosition = EnemyUnit.transform.position
            + ((new Vector3(ColliderSize.x * 0.5f, 0.0f, 0.0f) + new Vector3(EnemyUnit.ColliderSize.x * 0.5f, 0.0f, 0.0f)) * enemyDirection);
        UnitMovePoint.SetMovePoint(null, transform.position, targetPosition);
    }

    virtual public void MoveTo(Vector3 position)
    {
        MoveDirection = position - transform.position;
        float distance = MoveDirection.magnitude;
        Velocity = Mathf.Min(distance, UnitData.MoveSpeed * Time.deltaTime);

        // cc
        switch (TakenCCData.CCType)
        {
            case Types.CCType.None:
                break;
            case Types.CCType.Stun:
                Velocity = 0f;
                break;
            case Types.CCType.Slow:
                Velocity *= TakenCCData.CCValue;
                break;
            default:
                break;
        }

        if (Velocity < 0)
        {
            Velocity = 0f;
        }
        transform.position = transform.position + (MoveDirection.normalized * Velocity);
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

        float distance = Vector3.Distance(GetFindPosition(UnitTargetRangeType), EnemyUnit.transform.position);
        float targetRange = GetCurTargetRange();
        return distance < targetRange;
    }

    public Vector3 GetCenterPosition()
    {
        return UnitCenter.transform.position;
    }

    public void ClearEnemyUnit()
    {
        Notify(Types.UnitNotifyType.ClearEnemyUnit, null);
    }

    public AttackData GetAttackData()
    {
        // todo 공격을 정하는 정책
        AttackDataList attackDataList = UnitData.AttackDatasLists[(int)UnitTargetRangeType];
        int index = UnityEngine.Random.Range(0, attackDataList.AttackDataListElements.Length);
        AttackDataListElement attackDataListElement = attackDataList.AttackDataListElements[index];
        return attackDataListElement.AttackData;
    }

    void SetUnitTargetRangeType(Types.UnitTargetRangeType unitTargetRangeType)
    {
        UnitTargetRangeType = unitTargetRangeType;
    }

    public float GetCurTargetRange()
    {
        return GetTargetRange(UnitTargetRangeType);
    }

    public float GetTargetRange(Types.UnitTargetRangeType unitTargetRangeType)
    {
        int unitTargetRangeTypeIndex = (int)unitTargetRangeType;
        if (unitTargetRangeTypeIndex >= UnitData.AttackDatasLists.Length)
        {
            return 0;
        }
        else
        {
            return UnitData.AttackDatasLists[unitTargetRangeTypeIndex].TargetRange;
        }
    }
}
