using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Attack_ArcherTower : UnitState_Attack
{
    ChildUnitCreator _childUnitCreator;
    int _childUnitCount;
    int _attackChildUnitIndex;

    public override void Init(Unit unit, AUnitState[] unitStates)
    {
        base.Init(unit, unitStates);
        _childUnitCreator = GetComponent<ChildUnitCreator>();
    }

    // implements AUnitState
    public override void EnterState()
    {
        base.EnterState();
        _attackChildUnitIndex = 0;
        _childUnitCount = _childUnitCreator.Units.Length;
    }

    protected override void AttackFire()
    {
        //Debug.Log("AttackFire Archer");

        // 자식 유닛이 순서대로 공격하도록 한다.
        Unit unit = _childUnitCreator.GetUnit(_attackChildUnitIndex);
        UnitCommand unitCommand = new UnitCommand();
        unitCommand.UnitCommandType = Types.UnitCommandType.ToAttackState;
        unitCommand.Unit = _unit.EnemyUnit;
        unitCommand.UnitTargetRangeType = _unit.UnitTargetRangeType;
        unit.AddUnitCommand(unitCommand);
        if (++_attackChildUnitIndex >= _childUnitCount)
        {
            _attackChildUnitIndex = 0;
        }

        _lastAttackFireTime = Time.time;
    }
}
