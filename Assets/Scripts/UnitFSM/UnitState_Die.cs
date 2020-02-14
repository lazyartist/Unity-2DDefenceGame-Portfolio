using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Die : AUnitState
{
    // implements AUnitState
    public override void EnterState(Unit unit)
    {
        _unit = unit;

        unit.UnitBody.Animator.SetTrigger("Die");
        unit.Notify(Types.UnitNotifyType.ClearAttackTarget, null);
        unit.UnitEvent += OnUnitBodyEventHandler;
    }
    public override void ExitState(Unit unit)
    {
        unit.UnitEvent -= OnUnitBodyEventHandler;
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        return null;
    }
    void OnUnitBodyEventHandler(Types.UnitEventType unitEventType, Unit unit)
    {
        Debug.Log("UnitEventListener " + unitEventType);

        switch (unitEventType)
        {
            case Types.UnitEventType.None:
                break;
            case Types.UnitEventType.AttackStart:
                break;
            case Types.UnitEventType.AttackEnd:
                break;
            case Types.UnitEventType.Attack:
                break;
            case Types.UnitEventType.Die:
                break;
            case Types.UnitEventType.DiedComplete:
                DiedComplete();
                break;
            case Types.UnitEventType.AttackStop:
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
