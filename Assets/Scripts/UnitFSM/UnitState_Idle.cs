using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Idle : AUnitState
{
    // implements AUnitState
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
            //Debug.Log("Move Waypoint " + unit.TargetWaypoint);
            return unitStates[(int)Types.UnitFSMType.Move];
        }

        // 공격대상이 없으면 찾는다
        if (unit.HasAttackTargetUnit() == false && unit.TryFindEnemy() != null)
        {
            // 공격대상을 찾았다
            //Debug.Log("Found AttackTarget " + unit.AttackTargetUnit);
            if (unit.AttackData.ProjectilePrefab == null)
            {
                // 공격대상에게 이동할 동안 대기하도록 통보
                unit.AttackTargetUnit.Notify(Types.UnitNotifyType.Wait, unit);
                // 근거리 공격 : 적의 앞까지 이동할 waypoint 설정(진행 방향에 대해 앞)
                float enemyDirection = Mathf.Sign(unit.AttackTargetUnit.MoveDirection.x);
                unit.TargetWaypoint = WaypointManager.Inst.WaypointPool.Get();
                unit.TargetWaypoint.transform.position = unit.AttackTargetUnit.transform.position
                    + ((new Vector3(unit.UnitSize.x * 0.5f, 0.0f, 0.0f) + new Vector3(unit.AttackTargetUnit.UnitSize.x * 0.5f, 0.0f, 0.0f)) * enemyDirection);
                return unitStates[(int)Types.UnitFSMType.Move];
            }
            else
            {
                // 원거리 공격 : 즉시 공격
                return unitStates[(int)Types.UnitFSMType.Attack];
            }
        }

        //공격대상이 있다
        if (unit.HasAttackTargetUnit())
        {
            //Debug.LogAssertion("Idle : unit.AttackTargetUnit != null " + unit);
            return unitStates[(int)Types.UnitFSMType.Attack];
        }

        // 대기장소로 이동
        if (unit.TargetWaypoint == null && unit.RallyWaypoint != null && unit.IsArrivedRallyPoint() == false)
        {
            unit.TargetWaypoint = WaypointManager.Inst.WaypointPool.Get();
            unit.TargetWaypoint.transform.position = unit.RallyWaypoint.transform.position;
            //Debug.Log("Move RallyPoint " + unit.TargetWaypoint);
            return unitStates[(int)Types.UnitFSMType.Move];
        }

        return null;
    }
}
