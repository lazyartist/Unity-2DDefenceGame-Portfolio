using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPoolManager : SingletonBase<UnitPoolManager>
{
    public GameObject UnitPoolContainer;

    Dictionary<string, HashSet<Unit>> _unitsPool;

    protected override void Awake()
    {
        base.Awake();

        _unitsPool = new Dictionary<string, HashSet<Unit>>();
    }


    public Unit Get(string unitTypeName, Unit unitPrefab)
    {
        HashSet<Unit> units = null;
        if (_unitsPool.ContainsKey(unitTypeName))
        {
            units = _unitsPool[unitTypeName];
        }
        else
        {
            units = new HashSet<Unit>();
            _unitsPool.Add(unitTypeName, units);
        }

        Unit unit = null;
        IEnumerator<Unit> iter = units.GetEnumerator();
        if (iter.MoveNext())
        {
            unit = iter.Current;
            units.Remove(unit);
        }
        else
        {
            unit = Instantiate<Unit>(unitPrefab);
        }

        Debug.Log("UnitPool Get " + units.Count);
        return unit;
    }

    public void Release(Unit unit)
    {
        HashSet<Unit> units = null;
        if (_unitsPool.ContainsKey(unit.UnitData.UnitTypeName))
        {
            units = _unitsPool[unit.UnitData.UnitTypeName];
        }
        else
        {
            Debug.LogAssertion("no units in unitsPool!!");
        }

        unit.transform.SetParent(UnitPoolContainer.transform);
        unit.gameObject.SetActive(false);
        units.Add(unit);

        Debug.Log("UnitPool Release " + units.Count);
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
