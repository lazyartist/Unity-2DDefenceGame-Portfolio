using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Wait : AUnitState
{
    // implements AUnitState
    public override void EnterState(Unit unit)
    {
        //unit.UnitBody.Animator.SetFloat("Velocity", 1.0f);
    }
    public override void ExitState(Unit unit)
    {
        //unit.UnitBody.Animator.SetFloat("Velocity", 0.0f);
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        //unit.Toward(unit.TargetWaypoint2.transform.position);
        //unit.MoveTo(unit.TargetWaypoint2.transform.position);

        //float distance = Vector3.Distance(unit.transform.position, unit.TargetWaypoint2.transform.position);
        //if (distance < 0.01f)
        //{
        //    // 목표지점 도착
        //    unit.transform.position = unit.TargetWaypoint2.transform.position;

        //    // 다음 waypoint가 있으면 이동
        //    if (unit.TargetWaypoint2.NextWaypoint != null)
        //    {
        //        unit.TargetWaypoint2 = unit.TargetWaypoint2.NextWaypoint;
        //        return null;
        //    }
        //    else
        //    {
        //        // waypoint 해제
        //        WaypointManager.Inst.WaypointPool.Release(unit.TargetWaypoint2);
        //        unit.TargetWaypoint2 = null;

        //        //공격대상이 있고 공격범위에 있으면 공격
        //        if (unit.AttackTargetUnit != null && unit.IsAttackTargetInAttackArea())
        //        {
        //            Debug.Log("Attack " + unit.AttackTargetUnit);
        //            return unitStates[(int)Consts.UnitFSMType.Attack];
        //        }
        //        return unitStates[(int)Consts.UnitFSMType.Idle];
        //    }
        //}

        return null;
    }
}
