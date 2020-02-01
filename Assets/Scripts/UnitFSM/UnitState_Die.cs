using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Die : AUnitState
{
    private float _coolTime = 0.0f;

    // implements AUnitState
    public override void EnterState(Unit unit)
    {
        unit.UnitBody.Animator.SetTrigger("Die");
    }
    public override void ExitState(Unit unit)
    {
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        return null;
    }
}
