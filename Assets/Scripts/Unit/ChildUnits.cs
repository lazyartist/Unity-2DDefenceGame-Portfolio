﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildUnits : MonoBehaviour
{
    public Unit ParentUnit;
    public Unit ChildUnitPrefab;
    public Unit[] Units;
    public int MaxUnitCount;
    public Vector3 RallyPosition = Vector3.right;
    public float RallyPointRadius = 0.3f;
    public WaypointData WaypointDataUsedRallyPosition;

    LayerMask rallyPointLayerMask;

    void Awake()
    {
        ParentUnit = GetComponent<Unit>();
        Units = new Unit[MaxUnitCount];
    }

    private void Start()
    {
        RallyPosition = this.transform.position + Vector3.right;
        rallyPointLayerMask = LayerMask.GetMask(WaypointDataUsedRallyPosition.LayerName);

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

    bool TryFindRallyPosition(float radiusScale, bool isOrderNumberPriority/*true:OrderNumber 우선, false:거리 우선*/)
    {
        Unit parentUnit = GetComponent<Unit>();
        Waypoint rallyPoint = null;
        float minDistance = 999f;
        if (parentUnit != null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(parentUnit.transform.position, parentUnit.UnitData.TargetRange * radiusScale, rallyPointLayerMask);
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
                RallyPosition = rallyPoint.transform.position;
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
                Unit unit = Instantiate(ChildUnitPrefab, ParentUnit.SpawnPosition.transform.position, Quaternion.identity, ParentUnit.transform);
                unit.gameObject.SetActive(true);
                Units[i] = unit;
            }
        }
        SetRallyPointOfAllUnits();
    }

    void SetRallyPoint(Unit unit, int index, float startAngle = 0f)
    {
        Vector3 position = Quaternion.Euler(0f, 0f, startAngle + (360f / (float)MaxUnitCount) * (float)index) * (Vector3.up * RallyPointRadius);
        unit.UnitMovePoint.SetMovePoint(null, unit.transform.position, RallyPosition + position);
        unit.UnitMovePoint.RallyPoint = RallyPosition + position; // todo 이동 후 랠리포인트 지정 방법 필요, 메서드 활용
    }

    public void SetRallyPointOfAllUnits()
    {
        float startAngle = Random.Range(0f, (360f / (float)MaxUnitCount));
        for (int i = 0; i < MaxUnitCount; i++)
        {
            Unit unit = Units[i];
            if (unit != null && unit.IsDied == false)
            {
                SetRallyPoint(unit, i, startAngle);
            }
        }
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
}
