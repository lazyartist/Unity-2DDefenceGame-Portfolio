using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Idle_ArcherTower : AUnitState
{
    // implements AUnitState
    public override void EnterState()
    {
        _unit.UnitBody.Animator.SetTrigger("Idle");

        ChildUnitCreator childUnitCreator = GetComponent<ChildUnitCreator>();
        childUnitCreator.CreateUnits();

        UnitRenderOrder unitRenderOrder = GetComponent<UnitRenderOrder>();
        for (int i = 0; i < childUnitCreator.Units.Length; i++)
        {
            unitRenderOrder.SpriteRenderers[i + 1] = childUnitCreator.Units[i].UnitBody.UnitSR;
        }
        unitRenderOrder.CalcRenderOrder();
    }

    public override void ExitState()
    {
    }

    public override AUnitState UpdateState()
    {
        return null;
    }
}
