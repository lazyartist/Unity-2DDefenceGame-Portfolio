using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Attack : AUnitState
{
    private float _coolTime = 0.0f;

    // implements AUnitState
    public override void EnterState(Unit unit)
    {
        _unit = unit;

        unit.Toward(unit.AttackTargetUnit.transform.position);
        _coolTime = 0.0f;
        unit.AttackTargetUnit.Notify(Types.UnitNotifyType.Attack, unit);
        unit.UnitBody.UnitBodyEventHandler += OnUnitBodyEventHandler;
    }
    public override void ExitState(Unit unit)
    {
        unit.UnitBody.UnitBodyEventHandler -= OnUnitBodyEventHandler;
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        // 공격 애니가 끝날때까지 기다린다
        if (unit.UnitBody.PlayingAttackAni == false)
        {
            // 공격대상이 없으면 Idle 상태로 전환
            //if (unit.AttackTargetUnit == null || unit.AttackTargetUnit.IsDied || unit.IsAttackTargetInAttackArea() == false)
            if (unit.AttackTargetUnit == null || unit.AttackTargetUnit.IsDied)
            {
                return unitStates[(int)Types.UnitFSMType.Idle];
            }
            // 공격대상이 있고 쿨타임이 지났으면 공격
            else if (_coolTime <= 0.0f)
            {
                Debug.Log("Attack " + unit + " to " + unit.AttackTargetUnit);
                unit.UnitBody.Animator.SetTrigger("Attack");
                _coolTime = unit.UnitData.AttackCoolTime;
            }
        }
        _coolTime -= Time.deltaTime;

        return null;
    }
    void OnUnitBodyEventHandler(Types.UnitBodyEventType unitBodyEventType)
    {
        Debug.Log("UnitEventListener " + unitBodyEventType);

        switch (unitBodyEventType)
        {
            //case Types.UnitBodyEventType.None:
            //break;
            case Types.UnitBodyEventType.Attack:
                Attack();
                break;
            //case Types.UnitBodyEventType.DiedComplete:
            //DiedComplete();
            //break;
            default:
                break;
        }
    }
    virtual public void Attack()
    {
        if (_unit.AttackTargetUnit != null && _unit.AttackTargetUnit.IsDied == false)
        {
            _unit.AttackTargetUnit.TakeDamage(_unit.AttackData);
        }
    }
}
