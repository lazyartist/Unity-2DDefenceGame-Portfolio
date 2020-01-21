using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFSM : MonoBehaviour {
    public AUnitState[] UnitStates;
    public AUnitState CurUnitState;
    public Unit Unit;

	void Start () {
        foreach (var item in UnitStates)
        {
            item.enabled = CurUnitState == item;
        }
        CurUnitState.EnterState(Unit);
    }
	
	void Update () {
        // update FSM
        AUnitState unitState = CurUnitState.UpdateState(Unit, UnitStates);
        if(unitState != null && unitState != CurUnitState)
        {
            CurUnitState.ExitState(Unit);
            CurUnitState.enabled = false;
            CurUnitState = unitState;
            CurUnitState.enabled = true;
            CurUnitState.EnterState(Unit);
            //CurUnitState.UpdateState(Unit, UnitStates);
        }
    }
}
