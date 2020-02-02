using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Hurt : AUnitState
{
    // implements AUnitState
    public override void EnterState(Unit unit)
    {
        unit.UnitBody.Animator.SetTrigger("Hurt");
    }
    public override void ExitState(Unit unit)
    {
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        if(unit.TakenCCData.CCType == Types.CCType.Stun)
        {
            return null;
        }
        else
        {
            return PrevUnitState;
        }
    }
}
