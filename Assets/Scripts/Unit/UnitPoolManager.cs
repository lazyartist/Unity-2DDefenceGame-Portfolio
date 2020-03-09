using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPoolManager : SingletonBase<UnitPoolManager>
{
    public GameObject UnitPoolContainer;

    public Dictionary<string, HashSet<Unit>> ActiveUnitsPool;
    Dictionary<string, HashSet<Unit>> _deactiveUnitsPool;

    protected override void Awake()
    {
        base.Awake();

        ActiveUnitsPool = new Dictionary<string, HashSet<Unit>>();
        _deactiveUnitsPool = new Dictionary<string, HashSet<Unit>>();
    }


    public Unit Get(string unitTypeName, Unit unitPrefab)
    {
        HashSet<Unit> deactiveUnits = null;
        if (_deactiveUnitsPool.ContainsKey(unitTypeName))
        {
            deactiveUnits = _deactiveUnitsPool[unitTypeName];
        }
        else
        {
            deactiveUnits = new HashSet<Unit>();
            _deactiveUnitsPool.Add(unitTypeName, deactiveUnits);
        }

        Unit unit = null;
        IEnumerator<Unit> iter = deactiveUnits.GetEnumerator();
        if (iter.MoveNext())
        {
            // remove deactiveUnitsPool
            unit = iter.Current;
            deactiveUnits.Remove(unit);
        }
        else
        {
            unit = Instantiate<Unit>(unitPrefab);
        }

        // add activeUnitsPool
        HashSet<Unit> activeUnits = null;
        if (ActiveUnitsPool.ContainsKey(unitTypeName))
        {
            activeUnits = ActiveUnitsPool[unitTypeName];
        }
        else
        {
            activeUnits = new HashSet<Unit>();
            ActiveUnitsPool.Add(unitTypeName, activeUnits);
        }
        activeUnits.Add(unit);

        Debug.Log("UnitPool Get " + unitTypeName + " " + activeUnits.Count + "/" + deactiveUnits.Count);
        return unit;
    }

    public void Release(Unit unit)
    {
        HashSet<Unit> activeUnits = null;
        if (ActiveUnitsPool.ContainsKey(unit.UnitData.UnitTypeName))
        {
            activeUnits = ActiveUnitsPool[unit.UnitData.UnitTypeName];
        }
        else
        {
            Debug.LogAssertion("no units in activeUnitsPool!!");
        }
        activeUnits.Remove(unit);

        HashSet<Unit> deactiveUnits = null;
        if (_deactiveUnitsPool.ContainsKey(unit.UnitData.UnitTypeName))
        {
            deactiveUnits = _deactiveUnitsPool[unit.UnitData.UnitTypeName];
        }
        else
        {
            Debug.LogAssertion("no units in deactiveUnitsPool!!");
        }
        unit.transform.SetParent(UnitPoolContainer.transform);
        unit.gameObject.SetActive(false);
        deactiveUnits.Add(unit);

        Debug.Log("UnitPool Release " + unit.UnitData.UnitTypeName + " " + activeUnits.Count + "/" + deactiveUnits.Count);
    }

    //public Unit Get(string unitTypeName, Unit unitPrefab)
    //{
    //    int endIndex = 0;
    //    if (_unitsEndIndex.ContainsKey(unitTypeName))
    //    {
    //        endIndex = _unitsEndIndex[unitTypeName];
    //    }
    //    else
    //    {
    //        _unitsEndIndex.Add(unitTypeName, endIndex);
    //    }

    //    List<Unit >units = null;
    //    if (_unitsPool.ContainsKey(unitTypeName))
    //    {
    //        units = _unitsPool[unitTypeName];
    //    }
    //    else
    //    {
    //        units = new List<Unit>();
    //        _unitsPool.Add(unitTypeName, units);
    //    }

    //    Unit unit = null;
    //    if(units.Count <= endIndex)
    //    {
    //        unit = Instantiate<Unit>(unitPrefab);
    //        units.Add(unit);
    //    }
    //    else
    //    {
    //        unit = units[endIndex];
    //    }
    //    ++endIndex;

    //    return unit;
    //}

    //public void Release(Unit unit)
    //{
    //    if (waypoint == null) return;

    //    // waypoint 인덱스 검사
    //    int waypointIndex = -1;
    //    for (int i = 0; i < Waypoints.Length; i++)
    //    {
    //        if (Waypoints[i] == waypoint)
    //        {
    //            waypointIndex = i;
    //            break;
    //        }
    //    }

    //    if (waypointIndex == -1)
    //    {
    //        // 객체풀에 있는 waypoint가 아니라 이동 경로에 있는 waypoint이다.
    //        return;
    //    }

    //    // 해제하는 Waypoint를 활성화된 Waypoint 가장 끝으로 보내고 비활성화 시킴
    //    Waypoint tempWaypoint = Waypoints[waypointIndex];
    //    tempWaypoint.enabled = false;
    //    Waypoints[waypointIndex] = Waypoints[_endIndex];
    //    Waypoints[_endIndex] = tempWaypoint;
    //    // 활성화된 Waypoint의 끝을 한칸 앞으로 이동
    //    --_endIndex;
    //    //Debug.Log("waypoint rel _endIndex " + _endIndex);
    //}
}
