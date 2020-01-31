using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Attack : AUnitState
{
    private float _coolTime = 0.0f;

    // implements AUnitState
    public override void EnterState(Unit unit)
    {
        unit.Toward(unit.AttackTargetUnit.transform.position);
        _coolTime = 0.0f;
    }
    public override void ExitState(Unit unit)
    {
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        // 공격대상이 없으면 -> Idle
        if (unit.AttackTargetUnit == null || unit.AttackTargetUnit.IsDied || unit.IsAttackTargetInAttackArea() == false)
        {
            return unitStates[(int)Consts.UnitFSMType.Idle];
        }

        if (_coolTime <= 0.0f)
        {
            unit.UnitBody.Animator.SetTrigger("Attack");
            _coolTime = unit.UnitData.AttackCoolTime;
        }
        _coolTime -= Time.deltaTime;

        return null;
    }
}
