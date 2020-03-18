using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unit에 명령이 있을 때만 행동한다.
public class UnitState_IdleCommand : AUnitState
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
        if (_unit.ExistUnitCommand())
        {
            UnitCommand unitCommand = _unit.PopUnitCommand();
            switch (unitCommand.UnitCommandType)
            {
                case Types.UnitCommandType.ToAttackState:
                    _unit.SetUnitTargetRangeType(unitCommand.UnitTargetRangeType);
                    _unit.AddEnemyUnit(unitCommand.Unit);
                    return unitStates[(int)Types.UnitFSMType.Attack];
                case Types.UnitCommandType.ToMoveState:
                    _unit.UnitMovePoint.SetRallyPoint(unitCommand.Position);
                    return unitStates[(int)Types.UnitFSMType.Move];
                default:
                    break;
            }
        }
        return null;
    }
}
