using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Die : AUnitState
{
    private float _coolTime = 0.0f;

    // implements AUnitState
    public override void EnterState(Unit unit)
    {
        _unit = unit;

        unit.UnitBody.Animator.SetTrigger("Die");
        unit.AttackTargetUnit.Notify(Types.UnitNotifyType.AttackTargetUnitDied, unit);
        unit.AttackTargetUnit = null;
        unit.UnitBody.UnitBodyEventHandler += OnUnitBodyEventHandler;
    }
    public override void ExitState(Unit unit)
    {
        unit.UnitBody.UnitBodyEventHandler -= OnUnitBodyEventHandler;
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        return null;
    }
    void OnUnitBodyEventHandler(Types.UnitBodyEventType unitBodyEventType)
    {
        Debug.Log("UnitEventListener " + unitBodyEventType);

        switch (unitBodyEventType)
        {
            //case Types.UnitBodyEventType.None:
            //    break;
            //case Types.UnitBodyEventType.Attack:
            //    Attack();
            //    break;
            case Types.UnitBodyEventType.DiedComplete:
                DiedComplete();
                break;
            default:
                break;
        }
    }
    void DiedComplete()
    {
        Destroy(_unit.gameObject);
    }
}
