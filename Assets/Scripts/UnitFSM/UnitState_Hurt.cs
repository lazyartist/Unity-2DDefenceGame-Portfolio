using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Hurt : AUnitState
{
    // implements AUnitState
    public override void EnterState()
    {
        _unit.UnitBody.Animator.SetTrigger("Hurt");
    }

    public override void ExitState()
    {
    }

    public override AUnitState UpdateState()
    {
        if (_unit.TakenCCData.CCType == Types.CCType.Stun)
        {
            return null;
        }
        else
        {
            return PrevUnitState;
        }
    }
}
