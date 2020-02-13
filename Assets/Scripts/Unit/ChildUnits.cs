using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildUnits : MonoBehaviour {
    public Unit ChildUnitPrefab;
    public Unit[] Units;
    public int MaxUnitCount;
    public GameObject RallyPoint;
    //public List<Unit> Units;
    private void Awake()
    {
        Units = new Unit[MaxUnitCount];
    }

    public bool ExistNullUnit()
    {
        for (int i = 0; i < MaxUnitCount; i++)
        {
            if(Units[i] == null)
            {
                return true;
            }
        }
        return false;
    }
}
