using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Hurt : AUnitState
{
    private float _coolTime = 0.0f;

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
            return unitStates[(int)Types.UnitFSMType.Idle];
        }
    }
}
