using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Idle : AUnitState
{
    // implements IUnitState
    public override void EnterState(Unit unit)
    {
    }
    public override void ExitState(Unit unit)
    {
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        // 이동
        if (unit.TargetWaypoint2 != null)
        {
            Debug.Log("Move " + unit.TargetWaypoint2);
            //unit.TargetWaypoint2.transform.position = unit.AttackTargetUnit.transform.position;
            //Debug.Log("new AttackTargetUnit Found");
            //Debug.Log(Unit.TargetWaypoint2.transform.position);
            //Debug.Log(Unit.AttackTargetUnit.transform.position);
            //unit.TargetWaypoint2.enabled = true;

            return unitStates[(int)Consts.UnitFSMType.Move];
        }

        //공격대상이 없으면 찾는다.
        if (unit.AttackTargetUnit == null && unit.FindAttackTarget2() != null)
        {
            Debug.Log("Found AttackTarget " + unit.AttackTargetUnit);
        }

        //공격대상이 있다
        if (unit.AttackTargetUnit != null)
        {
            // 공격범위에 있으면 공격
            if (unit.IsAttackTargetInAttackArea())
            {
                Debug.Log("Attack " + unit.AttackTargetUnit);
                return unitStates[(int)Consts.UnitFSMType.Attack];
            }
            // 공격범위에 없으면 이동
            else
            {
                Debug.Log("Move " + unit.AttackTargetUnit);
                if (unit.TargetWaypoint2 == null)
                {
                    // 공격대상까지 이동할 waypoint 설정
                    unit.TargetWaypoint2 = WaypointManager.Inst.WaypointPool.Get();
                    unit.TargetWaypoint2.transform.position = unit.AttackTargetUnit.transform.position;
                }
                else
                {
                    // todo assert 여기 들어오면 안된다.
                    Debug.Log("unit.TargetWaypoint2 != null !!!");
                }

                //Debug.Log("new AttackTargetUnit Found");
                //Debug.Log(Unit.TargetWaypoint2.transform.position);
                //Debug.Log(Unit.AttackTargetUnit.transform.position);
                //unit.TargetWaypoint2.enabled = true;

                return unitStates[(int)Consts.UnitFSMType.Move];
            }
        }

        return null;
    }
}
