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
            Debug.Log("Found AttackTarget " + unit.AttackTargetUnit);
            if(unit.AttackData.ProjectilePrefab == null)
            {
                // 공격대상에게 이동할 동안 대기하도록 통보
                unit.AttackTargetUnit.Notify(Types.UnitNotifyType.Wait, unit);
                // 근거리 공격 : 공격대상의 앞까지 이동할 waypoint 설정
                unit.TargetWaypoint = WaypointManager.Inst.WaypointPool.Get();
                unit.TargetWaypoint.transform.position = unit.AttackTargetUnit.transform.position
                    + new Vector3(unit.UnitSize.x * 0.5f, 0.0f, 0.0f)
                    + new Vector3(unit.AttackTargetUnit.UnitSize.x * 0.5f, 0.0f, 0.0f);
                return unitStates[(int)Types.UnitFSMType.Move];
            }
            else
            {
                // 원거리 공격 : 즉시 공격
                return unitStates[(int)Types.UnitFSMType.Attack];
            }
        }

        //공격대상이 있다
        if (unit.AttackTargetUnit != null)
        {
            //Debug.LogAssertion("Idle : unit.AttackTargetUnit != null " + unit);
            return unitStates[(int)Types.UnitFSMType.Attack];
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
