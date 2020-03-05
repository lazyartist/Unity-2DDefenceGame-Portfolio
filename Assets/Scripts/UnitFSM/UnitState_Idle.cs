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
        // 목표지점에 도착하지 않았으면 이동
        if (_unit.UnitMovePoint.IsArrived == false)
        {
            return unitStates[(int)Types.UnitFSMType.Move];
        }

        // 공격대상이 없으면 찾는다
        if (_unit.HasEnemyUnit() == false && _unit.TryFindEnemyOrNull() != null)
        {
            // 공격대상을 찾았다
            if (_unit.GetAttackData().ProjectilePrefab == null)
            {
                // 공격대상에게 이동할 동안 대기하도록 통보
                _unit.EnemyUnit.Notify(Types.UnitNotifyType.Wait, _unit);
                // 근거리 공격 : 적의 앞까지 이동할 waypoint 설정(진행 방향에 대해 앞)
                float enemyDirection = Mathf.Sign(_unit.EnemyUnit.MoveDirection.x);
                Vector3 targetPosition = _unit.EnemyUnit.transform.position
                    + ((new Vector3(_unit.ColliderSize.x * 0.5f, 0.0f, 0.0f) + new Vector3(_unit.EnemyUnit.ColliderSize.x * 0.5f, 0.0f, 0.0f)) * enemyDirection);
                _unit.UnitMovePoint.SetMovePoint(null, _unit.transform.position, targetPosition);
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
            return unitStates[(int)Types.UnitFSMType.Attack];
        }

        // 적이 없으면 랠리포인트로 이동
        if (_unit.UnitMovePoint.IsArrived && _unit.UnitMovePoint.IsArrivedRallyPoint(_unit.transform.position) == false)
        {
            _unit.UnitMovePoint.SetMovePoint(null, _unit.transform.position, _unit.UnitMovePoint.RallyPoint);
            return unitStates[(int)Types.UnitFSMType.Move];
        }

        return null;
    }
}
