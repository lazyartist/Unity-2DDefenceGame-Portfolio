using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildUnits : MonoBehaviour
{
    public Unit ParentUnit;
    public Unit ChildUnitPrefab;
    public Unit[] Units;
    public int MaxUnitCount;
    public GameObject RallyPoint;
    public float RallyPointRadius = 0.3f;

    void Awake()
    {
        ParentUnit = GetComponent<Unit>();
        Units = new Unit[MaxUnitCount];
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
                Units[i] = unit;
                SetRallyPoint(unit, i);
            }
        }
    }

    void SetRallyPoint(Unit unit, int index)
    {
        Vector3 position = Quaternion.Euler(0f, 0f, (360f / (float)MaxUnitCount) * (float)index) * (Vector3.up * RallyPointRadius);
        unit.SetRallyPoint(RallyPoint.transform.position + position);
    }

    public void SetRallyPointOfAllUnits()
    {
        for (int i = 0; i < MaxUnitCount; i++)
        {
            Unit unit = Units[i];
            if (unit != null && unit.IsDied == false)
            {
                SetRallyPoint(unit, i);
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
