using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Idle : AUnitState
{
    // implements IUnitState
    public override void EnterState(Unit unit)
    {
        unit.UnitBody.Animator.SetTrigger("Idle");
    }
    public override void ExitState(Unit unit)
    {
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        // 이동
        if (unit.TargetWaypoint != null)
        {
            Debug.Log("Move Waypoint " + unit.TargetWaypoint);
            return unitStates[(int)Types.UnitFSMType.Move];
        }

        // 공격대상이 없으면 찾는다
        if (unit.AttackTargetUnit == null && unit.FindAttackTarget() != null)
        {
            // 공격대상을 찾았다
            // todo 근거리 공격이면 이동, 원거리이면 공격
            Debug.Log("Found AttackTarget " + unit.AttackTargetUnit);
            // 공격대상의 앞까지 이동할 waypoint 설정
            unit.TargetWaypoint = WaypointManager.Inst.WaypointPool.Get();
            unit.TargetWaypoint.transform.position = unit.AttackTargetUnit.transform.position
                + new Vector3(unit.UnitSize.x * 0.5f, 0.0f, 0.0f)
                + new Vector3(unit.AttackTargetUnit.UnitSize.x * 0.5f, 0.0f, 0.0f);
            unit.AttackTargetUnit.Notify(Types.UnitNotifyType.Wait, unit);
            return unitStates[(int)Types.UnitFSMType.Move];
        }

        //공격대상이 있다
        if (unit.AttackTargetUnit != null)
        {
            // 공격범위에 있으면 공격
            if (unit.IsAttackTargetInAttackArea())
            {
                Debug.Log("Attack in idle" + unit.AttackTargetUnit);
                return unitStates[(int)Types.UnitFSMType.Attack];
            }
            // 공격범위에 없으면 이동
            //else
            //{
            //    Debug.Log("Move AttackTargetUnit " + unit.AttackTargetUnit);
            //    if (unit.TargetWaypoint2 == null)
            //    {
            //        //unit.TargetWaypoint2 = WaypointManager.Inst.WaypointPool.Get();
            //        //// 공격대상의 앞까지 이동할 waypoint 설정
            //        //unit.TargetWaypoint2.transform.position = unit.AttackTargetUnit.transform.position
            //        //    + new Vector3(unit.UnitSize.x * 0.5f, 0.0f, 0.0f)
            //        //    + new Vector3(unit.AttackTargetUnit.UnitSize.x * 0.5f, 0.0f, 0.0f);
            //        //// 공격대상에게 대기하라고 통보
            //        //unit.AttackTargetUnit.Notify(Types.UnitNotifyType.Wait, unit);
            //    }
            //    else
            //    {
            //        Debug.LogAssertion("unit.TargetWaypoint2 != null !!!");
            //    }

            //    return unitStates[(int)Types.UnitFSMType.Move];
            //}
        }

        // 대기장소로 이동
        if (unit.TargetWaypoint == null && unit.WaitWaypoint != null && unit.IsArrivedWaitWaypoint() == false)
        {
            unit.TargetWaypoint = WaypointManager.Inst.WaypointPool.Get();
            unit.TargetWaypoint.transform.position = unit.WaitWaypoint.transform.position;
            Debug.Log("Move WaitWaypoint " + unit.TargetWaypoint);
            return unitStates[(int)Types.UnitFSMType.Move];
        }

        return null;
    }
}
