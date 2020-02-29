using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Move : AUnitState
{
    UnitRenderOrder _unitRenderOrder;
    public override void Init(Unit unit, AUnitState[] unitStates)
    {
        base.Init(unit, unitStates);
        _unitRenderOrder = unit.GetComponent<UnitRenderOrder>();
        _unitRenderOrder.CalcRenderOrder();
        _unitRenderOrder.enabled = false;
    }

    // implements AUnitState
    public override void EnterState()
    {
        _unit.UnitBody.Animator.SetFloat("Velocity", 1.0f);
        _unitRenderOrder.enabled = true;
    }

    public override void ExitState()
    {
        _unit.UnitBody.Animator.SetFloat("Velocity", 0.0f);
        if (_unit.HasEnemyUnit())
        {
            // 공격대상을 향하기
            _unit.Toward(_unit.EnemyUnit.transform.position);
        }
        _unitRenderOrder.enabled = false;
    }

    public override AUnitState UpdateState()
    {
        // 이동
        _unit.MoveTo(_unit.TargetWaypoint.GetPosition(_unit.WaypointSubIndex));
        if (_unit.CanChangeDirection)
        {
            _unit.Toward(_unit.TargetWaypoint.GetPosition(_unit.WaypointSubIndex));
        }

        float distance = Vector3.Distance(_unit.transform.position, _unit.TargetWaypoint.GetPosition(_unit.WaypointSubIndex));
        if (distance < 0.01f)
        {
            // 목표지점 도착
            _unit.transform.position = _unit.TargetWaypoint.GetPosition(_unit.WaypointSubIndex);

            // 다음 waypoint가 있으면 이동
            if (_unit.TargetWaypoint.NextWaypoint != null)
            {
                _unit.TargetWaypoint = _unit.TargetWaypoint.NextWaypoint;
                return null;
            }
            else
            {
                // waypoint 해제
                WaypointManager.Inst.WaypointPool.Release(_unit.TargetWaypoint);
                _unit.TargetWaypoint = null;
                return unitStates[(int)Types.UnitFSMType.Idle];
            }
        }

        return null;
    }
}
