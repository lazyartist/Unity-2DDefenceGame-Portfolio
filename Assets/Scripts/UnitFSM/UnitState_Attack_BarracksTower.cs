﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UnitState_Attack_BarracksTower : AUnitState
{
    private ChildUnits _childUnits;
    private float _coolTime = 0.0f;
    private bool _isPlayingAttackAni;

    void Awake()
    {
        _childUnits = GetComponent<ChildUnits>();
    }

    // implements AUnitState
    public override void EnterState(Unit unit)
    {
        _unit = unit;

        _coolTime = 0.0f;
        unit.UnitEvent += OnUnitEventHandler;
    }
    public override void ExitState(Unit unit)
    {
        unit.UnitEvent -= OnUnitEventHandler;
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        // todo 유닛 개별 쿨타임으로  생성, 미리 생성하고 코루틴으로 나중에 활성화한다.
        // 공격 애니가 끝날때까지 기다린다
        if (_isPlayingAttackAni == false)
        {
            if (_coolTime <= 0.0f && _childUnits.ExistNullUnit())
            {
                Debug.Log("ChildUnit " + unit);
                unit.UnitBody.Animator.SetTrigger("Attack");
                _coolTime = unit.UnitData.AttackCoolTime;
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
