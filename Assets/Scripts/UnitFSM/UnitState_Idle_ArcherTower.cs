using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Idle_ArcherTower : UnitState_Idle
{
    public override void Init(Unit unit, AUnitState[] unitStates)
    {
        base.Init(unit, unitStates);

        ChildUnitCreator childUnitCreator = GetComponent<ChildUnitCreator>();
        childUnitCreator.CreateAllUnits();
        int childUnitCount = childUnitCreator.Units.Length;
        for (int i = 0; i < childUnitCount; i++)
        {
            Unit childUnit = childUnitCreator.GetUnit(i);
            UnitCommand unitCommand = new UnitCommand();
            unitCommand.UnitCommandType = Types.UnitCommandType.ToMoveState;
            unitCommand.Position = _unit.transform.position + childUnitCreator.IndividualRallyPointsInLocal[i];
            childUnit.AddUnitCommand(unitCommand);
        }

        UnitRenderOrder unitRenderOrder = GetComponent<UnitRenderOrder>();
        for (int i = 0; i < childUnitCreator.Units.Length; i++)
        {
            unitRenderOrder.SpriteRenderers[i + 1] = childUnitCreator.Units[i].UnitBody.UnitSR;
        }
        unitRenderOrder.CalcRenderOrder();
    }
}
