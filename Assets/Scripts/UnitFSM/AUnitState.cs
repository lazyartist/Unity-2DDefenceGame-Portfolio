using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract UnitState
public abstract class AUnitState : MonoBehaviour
{
    [HideInInspector]
    public AUnitState PrevUnitState;
    protected Unit _unit;
    // todo Init(Unit unit, AUnitState[] unitStates);
    // todo EnterState();
    public abstract void EnterState(Unit unit);
    public abstract void ExitState(Unit unit);
    public abstract AUnitState UpdateState(Unit unit, AUnitState[] unitStates);
}
