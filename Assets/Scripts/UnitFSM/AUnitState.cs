using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract UnitState
public abstract class AUnitState : MonoBehaviour
{
    public AUnitState PrevUnitState;

    protected Unit _unit;
    protected AUnitState[] unitStates;

    public virtual void Init(Unit unit, AUnitState[] unitStates)
    {
        _unit = unit;
        this.unitStates = unitStates;
    }
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract AUnitState UpdateState();
}
