using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Idle : AUnitState
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
        // 이동
        if (_unit.TargetWaypoint != null)
        {
            //Debug.Log("Move Waypoint " + unit.TargetWaypoint);
            return unitStates[(int)Types.UnitFSMType.Move];
        }

        // 공격대상이 없으면 찾는다
        if (_unit.HasEnemyUnit() == false && _unit.TryFindEnemy() != null)
        {
            // 공격대상을 찾았다
            //Debug.Log("Found Enemy " + unit.EnemyUnit);
            if (_unit.GetAttackData().ProjectilePrefab == null)
            {
                // 공격대상에게 이동할 동안 대기하도록 통보
                _unit.EnemyUnit.Notify(Types.UnitNotifyType.Wait, _unit);
                // 근거리 공격 : 적의 앞까지 이동할 waypoint 설정(진행 방향에 대해 앞)
                float enemyDirection = Mathf.Sign(_unit.EnemyUnit.MoveDirection.x);
                _unit.TargetWaypoint = WaypointManager.Inst.WaypointPool.Get();
                _unit.TargetWaypoint.transform.position = _unit.EnemyUnit.transform.position
                    + ((new Vector3(_unit.ColliderSize.x * 0.5f, 0.0f, 0.0f) + new Vector3(_unit.EnemyUnit.ColliderSize.x * 0.5f, 0.0f, 0.0f)) * enemyDirection);
                return unitStates[(int)Types.UnitFSMType.Move];
            }
            else
            {
                // 원거리 공격 : 즉시 공격
                return unitStates[(int)Types.UnitFSMType.Attack];
            }
        }

        //공격대상이 있다
        if (_unit.HasEnemyUnit())
        {
            //Debug.LogAssertion("Idle : unit.EnemyUnit != null " + unit);
            return unitStates[(int)Types.UnitFSMType.Attack];
        }

        // 대기장소로 이동
        if (_unit.TargetWaypoint == null && _unit.RallyWaypoint != null && _unit.IsArrivedRallyPoint() == false)
        {
            _unit.TargetWaypoint = WaypointManager.Inst.WaypointPool.Get();
            _unit.TargetWaypoint.transform.position = _unit.RallyWaypoint.transform.position;
            //Debug.Log("Move RallyPoint " + unit.TargetWaypoint);
            return unitStates[(int)Types.UnitFSMType.Move];
        }

        return null;
    }
}
