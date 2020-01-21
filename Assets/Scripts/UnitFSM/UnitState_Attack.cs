using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Attack : AUnitState
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // implements AUnitState
    public override void EnterState(Unit unit)
    {
        unit.UnitBody.Animator.SetTrigger("Attack");
    }
    public override void ExitState(Unit unit)
    {
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        unit.Toward(unit.TargetWaypoint2.transform.position);
        unit.MoveTo(unit.TargetWaypoint2.transform.position);

        // 웨이포인트에 다다르면 다음 웨이포인트를 타겟 웨이포인트로 지정
        float distance = Vector3.Distance(unit.transform.position, unit.TargetWaypoint2.transform.position);
        if (distance < 0.01f)
        {
            // arrived
            return unitStates[(int)Consts.UnitFSMType.Idle];
        }
        return null;
    }
}
