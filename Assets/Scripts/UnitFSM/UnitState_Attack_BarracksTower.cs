using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UnitState_Attack_BarracksTower : AUnitState
{
    ChildUnits _childUnits;
    float _coolTime = 0.0f;
    bool _isPlayingAttackAni;

    void Awake()
    {
        _childUnits = GetComponent<ChildUnits>();
    }

    // implements AUnitState
    public override void EnterState()
    {
        _coolTime = 0.0f;
        _unit.UnitEvent += OnUnitEventHandler;
    }

    public override void ExitState()
    {
        _unit.UnitEvent -= OnUnitEventHandler;
    }

    public override AUnitState UpdateState()
    {
        // todo 유닛 개별 쿨타임으로  생성, 미리 생성하고 코루틴으로 나중에 활성화한다.
        // 공격 애니가 끝날때까지 기다린다
        if (_isPlayingAttackAni == false)
        {
            if (_coolTime <= 0.0f && _childUnits.ExistNullUnit())
            {
                Debug.Log("ChildUnit " + _unit);
                _unit.UnitBody.Animator.SetTrigger("Attack");
                _coolTime = _unit.GetAttackData().CoolTime;
                //_coolTime = _unit.UnitData.AttackCoolTime;
            }
        }
        _coolTime -= Time.deltaTime;

        return null;
    }

    void OnUnitEventHandler(Types.UnitEventType unitBodyEventType, Unit unit)
    {
        Debug.Log("UnitEventListener " + unitBodyEventType);

        switch (unitBodyEventType)
        {
            case Types.UnitEventType.None:
                break;
            case Types.UnitEventType.AttackFire:
                AttackFire();
                break;
            case Types.UnitEventType.AttackStart:
                _isPlayingAttackAni = true;
                break;
            case Types.UnitEventType.AttackEnd:
                _isPlayingAttackAni = false;
                break;
            case Types.UnitEventType.Die:
                break;
            case Types.UnitEventType.DiedComplete:
                break;
            case Types.UnitEventType.AttackStopped:
                break;
            default:
                break;
        }
    }

    virtual public void AttackFire()
    {
        _childUnits.CreateUnits();
    }
}
