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

            return unitStates[(int)Types.UnitFSMType.Move];
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
                return unitStates[(int)Types.UnitFSMType.Attack];
            }
            // 공격범위에 없으면 이동
            else
            {
                Debug.Log("Move " + unit.AttackTargetUnit);
                if (unit.TargetWaypoint2 == null)
                {
                    // 공격대상까지 이동할 waypoint 설정
                    unit.TargetWaypoint2 = WaypointManager.Inst.WaypointPool.Get();
                    // 공격 대상의 크기
                    unit.TargetWaypoint2.transform.position = unit.AttackTargetUnit.transform.position
                        + new Vector3(unit.UnitSize.x * 0.5f, 0.0f, 0.0f)
                        + new Vector3(unit.AttackTargetUnit.UnitSize.x * 0.5f, 0.0f, 0.0f);
                    // 공객대상의 공격대상에 현재 유닛을 등록하고 대기상태로 만듦
                    unit.AttackTargetUnit.AttackTargetUnit = unit;
                    unit.AttackTargetUnit.UnitFSM.Transit(Types.UnitFSMType.Wait);
                    //unit.AttackTargetUnit.GetComponent<UnitFSM>().Transit(Consts.UnitFSMType.Wait);
                }
                else
                {
                    Debug.LogAssertion("unit.TargetWaypoint2 != null !!!");
                }

                //Debug.Log("new AttackTargetUnit Found");
                //Debug.Log(Unit.TargetWaypoint2.transform.position);
                //Debug.Log(Unit.AttackTargetUnit.transform.position);
                //unit.TargetWaypoint2.enabled = true;

                return unitStates[(int)Types.UnitFSMType.Move];
            }
        }

        // 대기장소로 이동
        // todo 대기장소 도착 확인
        if (unit.TargetWaypoint2 == null && unit.WaitWaypoint != null)
        {
            Debug.Log("Move WaitWaypoint " + unit.TargetWaypoint2);
            // 공격대상까지 이동할 waypoint 설정
            unit.TargetWaypoint2 = WaypointManager.Inst.WaypointPool.Get();
            unit.TargetWaypoint2.transform.position = unit.WaitWaypoint.transform.position;

            return unitStates[(int)Types.UnitFSMType.Move];
        }

        return null;
    }
}
