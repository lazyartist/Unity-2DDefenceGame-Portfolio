using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Move : AUnitState
{
    IUnitRenderOrder _unitRenderOrder;
    public override void Init(Unit unit, AUnitState[] unitStates)
    {
        base.Init(unit, unitStates);
        _unitRenderOrder = unit.GetComponent<IUnitRenderOrder>();
    }

    // implements AUnitState
    public override void EnterState()
    {
        _unit.UnitBody.Animator.SetFloat("Velocity", 1.0f);
    }

    public override void ExitState()
    {
        _unit.UnitBody.Animator.SetFloat("Velocity", 0.0f);
        if (_unit.HasEnemyUnit())
        {
            // 공격대상을 향하기
            _unit.Toward(_unit.EnemyUnit.transform.position);
        }
    }

    public override AUnitState UpdateState()
    {
        // 이동
        Vector3 targetPosition = _unit.UnitMovePoint.GetPosition();
        _unit.MoveTo(targetPosition);
        _unitRenderOrder.CalcRenderOrder();
        if (_unit.CanChangeDirection)
        {
            _unit.Toward(targetPosition);
        }

        if (_unit.UnitMovePoint.IsArrivedPosition(_unit.transform.position))
        {
            // 목표지점 도착
            _unit.transform.position = targetPosition;
            // 다음 목표지점으로 변경
            if (_unit.UnitMovePoint.TryNextPosition())
            {
                // 새로운 목표지점이 있다. Move 상태 계속 유지
                return null;
            }
            else
            {
                // 새로운 목표지점이 없다. Idle 상태로 전환
                return unitStates[(int)Types.UnitFSMType.Idle];
            }
        }

        return null;
    }
}
