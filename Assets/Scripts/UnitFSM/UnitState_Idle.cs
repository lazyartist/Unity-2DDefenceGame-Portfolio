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
        if (_unit.HasEnemyUnit() == false)
        {
            if (_unit.HasEnemyUnit() == false && _unit.TryFindEnemy())
            {
                switch (_unit.UnitTargetRangeType)
                {
                    case Types.UnitTargetRangeType.Short: // 근거리 공격
                        {
                            _unit.GoToEnemy();
                            return unitStates[(int)Types.UnitFSMType.Move];
                        }
                    case Types.UnitTargetRangeType.Long: // 원거리 공격
                        {
                            // 즉시 공격
                            return unitStates[(int)Types.UnitFSMType.Attack];
                        }
                    //case Types.UnitTargetRangeType.Count:
                        //break;
                    default:
                        break;
                }
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
