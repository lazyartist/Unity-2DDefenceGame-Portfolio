using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AUnitState : MonoBehaviour
{
    public AUnitState PrevUnitState;

    public abstract void EnterState(Unit unit);
    public abstract void ExitState(Unit unit);
    public abstract AUnitState UpdateState(Unit unit, AUnitState[] unitStates);
}
