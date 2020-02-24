using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_None: AUnitState
{
    // implements AUnitState
    public override void EnterState(Unit unit)
    {
    }
    public override void ExitState(Unit unit)
    {
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        return null;
    }
}
