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
            // todo UnitState.Init(Unit, UnitStates);
        }
        CurUnitState.EnterState(Unit);
    }
	
	void Update () {
        AUnitState unitState = null;
        if (Unit.IsDied == true)
        {
            // 유닛 사망
            unitState = UnitStates[(int)Types.UnitFSMType.Died];
        }else if (Unit.TakenCCData.CCType == Types.CCType.Stun && CurUnitState != UnitStates[(int)Types.UnitFSMType.Hurt])
        {
            // 데미지 받음
            unitState = UnitStates[(int)Types.UnitFSMType.Hurt];
        }

        if (unitState == null || unitState == CurUnitState)
        {
            unitState = CurUnitState.UpdateState(Unit, UnitStates);
        }

        if(unitState != null && unitState != CurUnitState)
        {
            Transit(unitState);
        }
    }

    // 상태 전환
    public void Transit(Types.UnitFSMType unitFSMType)
    {
        AUnitState unitState = UnitStates[(int)unitFSMType];
        if(CurUnitState != unitState)
        {
            Transit(unitState);
        }
    }

    void Transit(AUnitState unitState)
    {
        CurUnitState.ExitState(Unit);
        CurUnitState.enabled = false;
        unitState.PrevUnitState = CurUnitState;
        CurUnitState = unitState;
        CurUnitState.enabled = true;
        CurUnitState.EnterState(Unit);
        //CurUnitState.UpdateState(Unit, UnitStates);
    }

    public void ClearnUp()
    {
        CurUnitState.ExitState(Unit);
    }
}
