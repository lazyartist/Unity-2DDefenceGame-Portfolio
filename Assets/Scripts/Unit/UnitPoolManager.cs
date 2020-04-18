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

        //Debug.Log("UnitPool Get " + unitTypeName + " " + activeUnits.Count + "/" + deactiveUnits.Count);
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

        //Debug.Log("UnitPool Release " + unit.UnitData.UnitTypeName + " " + activeUnits.Count + "/" + deactiveUnits.Count);
    }
}
