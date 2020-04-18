using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Wait : AUnitState
{
    // implements AUnitState
    public override void EnterState()
    {
        _unit.UnitBody.Animator.SetTrigger("Idle");
    }

    public override void ExitState()
    {
    }

    public override AUnitState UpdateState()
    {
        return null;
    }
}
