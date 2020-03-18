using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 한 번만 공격하고 Idle 상태로 돌아간다.
public class UnitState_AttackOnce : UnitState_Attack
{
    AUnitState _unitStateToReturn = null;

    // implements AUnitState
    public override void EnterState()
    {
        base.EnterState();
        _unitStateToReturn = null;
    }

    public override void ExitState()
    {
        base.ExitState();
        _unitStateToReturn = null;
    }

    public override AUnitState UpdateState()
    {
        AUnitState unitStateResult = base.UpdateState();
        if (unitStateResult == null)
        {
            return _unitStateToReturn;
        }
        else
        {
            return unitStateResult;
        }
    }

    override protected void OnUnitEventHandler(Types.UnitEventType unitBodyEventType, Unit unit)
    {
        //Debug.Log("UnitEventListener " + unitBodyEventType);
        base.OnUnitEventHandler(unitBodyEventType, unit);

        switch (unitBodyEventType)
        {
            case Types.UnitEventType.None:
                break;
            case Types.UnitEventType.AttackFire:
                break;
            case Types.UnitEventType.AttackStart:
                break;
            case Types.UnitEventType.AttackEnd:
                {
                    _unitStateToReturn = unitStates[(int)Types.UnitFSMType.Idle];
                }
                break;
            case Types.UnitEventType.Die:
                break;
            case Types.UnitEventType.DiedComplete:
                break;
            case Types.UnitEventType.AttackStopped:
                break;
            default:
                break;
        }
    }
}
