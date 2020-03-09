﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildUnitCreator : MonoBehaviour
{
    public Unit ParentUnit;
    public Unit ChildUnitPrefab;
    public Unit[] Units;
    public Vector3 CenterRallyPointInLocal = Vector3.right; // 이 랠리포인트 주변으로 유닛 배치
    public Vector3[] IndividualRallyPointInLocals; // 유닛별 랠리포인트
    public WaypointData WaypointDataUsedRallyPosition; // 가까운 WayPoint를 찾기 위한 데이터
    public bool IsUseParentUnitCenter; // 부모 유닛의 센터를 자식 유닛이 공유(자식유닛이 부모유닛의 공격범위를 사용할 경우)
    public bool IsSetRallyPointToNearlyWayPoint; // 가까운 WayPoint를 RallyPoint로 사요할지 여부
    public int MaxUnitCount;
    public float RallyPointRadius = 0.3f;

    LayerMask _wayPointLayerMask;

    void Awake()
    {
        ParentUnit = GetComponent<Unit>();
        Units = new Unit[MaxUnitCount];
    }

    private void Start()
    {
        if (IsSetRallyPointToNearlyWayPoint)
        {
            _wayPointLayerMask = LayerMask.GetMask(WaypointDataUsedRallyPosition.LayerName);

            float radiusScale = 1.0f;
            // RallyPosition의 첫 검색은 부모 유닛의 공격범위에 있는 Waypoint 중 가장 뒤에 있는 Waypoint로 지정
            if (TryFindRallyPosition(radiusScale, true) == false)
            {
                // 랠리 포지션을 유닛의 공격범위에서 찾고 못찾았으면 범위를 늘려서 다시 찾고 가장 가까운 Waypoint를 지정
                while (TryFindRallyPosition(radiusScale, false) == false)
                {
                    radiusScale += 0.2f;
                    if (radiusScale > 5f) break;
                }
            }
        }
    }

    bool TryFindRallyPosition(float radiusScale, bool isOrderNumberPriority/*true:OrderNumber 우선, false:거리 우선*/)
    {
        Unit parentUnit = GetComponent<Unit>();
        Waypoint rallyPoint = null;
        float minDistance = 999f;
        if (parentUnit != null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(parentUnit.transform.position, parentUnit.UnitData.TargetRange * radiusScale, _wayPointLayerMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider2D collider = colliders[i];
                Waypoint waypoint = collider.gameObject.GetComponent<Waypoint>();
                if (waypoint != null)
                {
                    if (rallyPoint == null)
                    {
                        rallyPoint = waypoint;
                    }
                    else
                    {
                        if (isOrderNumberPriority)
                        {
                            if (waypoint.OrderNumber > rallyPoint.OrderNumber)
                            {
                                rallyPoint = waypoint;
                            }
                        }
                        else
                        {
                            if (minDistance > Vector3.Distance(parentUnit.transform.position, waypoint.transform.position))
                            {
                                rallyPoint = waypoint;
                            }
                        }
                    }
                }
            }

            if (rallyPoint != null)
            {
                //CenterRallyPointInLocal = transform.position - rallyPoint.transform.position;
                CenterRallyPointInLocal = rallyPoint.transform.position - transform.position;
            }
        }

        //Debug.Log("radiusScale " + radiusScale);
        return rallyPoint != null;
    }

    public bool ExistNullUnit()
    {
        for (int i = 0; i < MaxUnitCount; i++)
        {
            if (Units[i] == null)
            {
                return true;
            }
        }
        return false;
    }

    public void CreateUnits()
    {
        for (int i = 0; i < MaxUnitCount; i++)
        {
            if (Units[i] == null)
            {
                Unit unit = UnitPoolManager.Inst.Get(ChildUnitPrefab.UnitData.UnitTypeName, ChildUnitPrefab);
                unit.transform.position = ParentUnit.SpawnPosition.transform.position;
                unit.transform.SetParent(ParentUnit.transform);
                unit.Init();
                unit.UnitEvent += OnUnitEvent;
                unit.gameObject.SetActive(true);
                if (IsUseParentUnitCenter)
                {
                    unit.UnitCenter = ParentUnit.UnitCenter;
                }
                Units[i] = unit;
            }
        }

        if (IndividualRallyPointInLocals.Length == 0)
        {
            SetCenterRallyPointToAllUnits();
        }
        else
        {
            SetIndividualRallyPointToAllUnits();
        }
    }

    public void SetCenterRallyPointToAllUnits()
    {
        float startAngle = Random.Range(0f, (360f / (float)MaxUnitCount));
        for (int i = 0; i < MaxUnitCount; i++)
        {
            Unit unit = Units[i];
            if (unit != null && unit.IsDied == false)
            {
                SetCenterRallyPoint(unit, i, startAngle);
            }
        }
    }

    void SetCenterRallyPoint(Unit unit, int index, float startAngle = 0f)
    {
        Vector3 localPosition = Quaternion.Euler(0f, 0f, startAngle + (360f / (float)MaxUnitCount) * (float)index) * (Vector3.up * RallyPointRadius);
        Vector3 startPosition = unit.transform.position;
        Vector3 endPosition = transform.position + CenterRallyPointInLocal + localPosition;
        unit.UnitMovePoint.SetMovePoint(null, startPosition, endPosition);
        unit.UnitMovePoint.RallyPoint = endPosition; // todo 이동 후 랠리포인트 지정 방법 필요, 메서드 활용
    }

    public void SetIndividualRallyPointToAllUnits()
    {
        for (int i = 0; i < MaxUnitCount; i++)
        {
            Unit unit = Units[i];
            if (unit != null && unit.IsDied == false)
            {
                SetIndividualRallyPoint(unit, IndividualRallyPointInLocals[i]);
            }
        }
    }

    void SetIndividualRallyPoint(Unit unit, Vector3 rallyPoint)
    {
        Vector3 startPosition = unit.transform.position;
        Vector3 endPosition = transform.position + rallyPoint;
        unit.UnitMovePoint.SetMovePoint(null, startPosition, endPosition);
        unit.UnitMovePoint.RallyPoint = endPosition;
    }

    public void ClearAllEnemyUnits()
    {
        for (int i = 0; i < MaxUnitCount; i++)
        {
            Unit unit = Units[i];
            if (unit != null && unit.IsDied == false)
            {
                unit.ClearEnemyUnit();
            }
        }
    }

    void OnUnitEvent(Types.UnitEventType unitEventType, Unit unit)
    {
        switch (unitEventType)
        {
            case Types.UnitEventType.None:
                break;
            case Types.UnitEventType.Live:
                break;
            case Types.UnitEventType.AttackStart:
                break;
            case Types.UnitEventType.AttackEnd:
                break;
            case Types.UnitEventType.AttackFire:
                break;
            case Types.UnitEventType.Die:
                {
                    unit.UnitEvent -= OnUnitEvent;
                    for (int i = 0; i < Units.Length; i++)
                    {
                        if (Units[i] == unit)
                        {
                            Units[i] = null;
                        }
                    }
                }

                break;
            case Types.UnitEventType.DiedComplete:
                break;
            case Types.UnitEventType.AttackStopped:
                break;
            default:
                break;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + CenterRallyPointInLocal, 0.1f);
        Gizmos.DrawIcon(transform.position + CenterRallyPointInLocal, "RallyPoint_Flag.png");
        for (int i = 0; i < IndividualRallyPointInLocals.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + IndividualRallyPointInLocals[i], 0.1f);
            Gizmos.DrawIcon(transform.position + IndividualRallyPointInLocals[i], "RallyPoint_Flag");
        }
    }
}
