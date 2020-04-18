using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Die : AUnitState
{
    // implements AUnitState
    public override void EnterState()
    {
        _unit.UnitBody.Animator.SetTrigger("Die");
        _unit.Notify(Types.UnitNotifyType.ClearEnemyUnit, null);
        _unit.UnitEvent += OnUnitBodyEventHandler;
    }

    public override void ExitState()
    {
        _unit.UnitEvent -= OnUnitBodyEventHandler;
    }

    public override AUnitState UpdateState()
    {
        return null;
    }

    void OnUnitBodyEventHandler(Types.UnitEventType unitEventType, Unit unit)
    {
        switch (unitEventType)
        {
            case Types.UnitEventType.None:
                break;
            case Types.UnitEventType.AttackStart:
                break;
            case Types.UnitEventType.AttackEnd:
                break;
            case Types.UnitEventType.AttackFire:
                break;
            case Types.UnitEventType.Die:
                break;
            case Types.UnitEventType.DiedComplete:
                DiedComplete();
                break;
            case Types.UnitEventType.AttackStopped:
                break;
            default:
                break;
        }
    }

    void DiedComplete()
    {
        _unit.DiedComplete();
        //Destroy(_unit.gameObject);
    }
}
