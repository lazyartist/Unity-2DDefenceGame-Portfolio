﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Move : AUnitState
{
    // implements AUnitState
    public override void EnterState(Unit unit)
    {
        unit.UnitBody.Animator.SetFloat("Velocity", 1.0f);
    }
    public override void ExitState(Unit unit)
    {
        unit.UnitBody.Animator.SetFloat("Velocity", 0.0f);
        if (unit.HasAttackTargetUnit())
        {
            // 공격대상을 향하기
            unit.Toward(unit.AttackTargetUnit.transform.position);
        }
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        // 이동
        unit.MoveTo(unit.TargetWaypoint.GetPosition(unit.TargetWaypointSubIndex));
        if (unit.CanChangeDirection)
        {
            unit.Toward(unit.TargetWaypoint.GetPosition(unit.TargetWaypointSubIndex));
        }

        float distance = Vector3.Distance(unit.transform.position, unit.TargetWaypoint.GetPosition(unit.TargetWaypointSubIndex));
        if (distance < 0.01f)
        {
            // 목표지점 도착
            unit.transform.position = unit.TargetWaypoint.GetPosition(unit.TargetWaypointSubIndex);

            // 다음 waypoint가 있으면 이동
            if (unit.TargetWaypoint.NextWaypoint != null)
            {
                unit.TargetWaypoint = unit.TargetWaypoint.NextWaypoint;
                return null;
            }
            else
            {
                // waypoint 해제
                WaypointManager.Inst.WaypointPool.Release(unit.TargetWaypoint);
                unit.TargetWaypoint = null;
                return unitStates[(int)Types.UnitFSMType.Idle];
            }
        }

        return null;
    }
}
