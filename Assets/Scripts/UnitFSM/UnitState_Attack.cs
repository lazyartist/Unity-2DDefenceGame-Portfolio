using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Attack : AUnitState
{
    private float _coolTime = 0.0f;
    private bool _isPlayingAttackAni;

    // implements AUnitState
    public override void EnterState(Unit unit)
    {
        _unit = unit;

        unit.Toward(unit.AttackTargetUnit.transform.position);
        _coolTime = 0.0f;
        unit.UnitEvent += OnUnitEventHandler;
        if(unit.AttackData.ProjectilePrefab == null)
        {
            // 근거리 공격 : 공격을 통보하여 함께 싸움
            unit.AttackTargetUnit.Notify(Types.UnitNotifyType.Attack, unit);
        }
        else
        {
            // 원거리 공격 : 공격을 통보하지 않음
        }
    }
    public override void ExitState(Unit unit)
    {
        unit.UnitEvent -= OnUnitEventHandler;
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        // 공격 애니가 끝날때까지 기다린다
        if (_isPlayingAttackAni == false)
        {
            // 공격대상이 없으면 Idle 상태로 전환
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
    void OnUnitEventHandler(Types.UnitEventType unitBodyEventType)
    {
        Debug.Log("UnitEventListener " + unitBodyEventType);

        switch (unitBodyEventType)
        {
            case Types.UnitEventType.None:
                break;
            case Types.UnitEventType.Attack:
                Attack();
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
            default:
                break;
        }
    }
    virtual public void Attack()
    {
        if (_unit.AttackTargetUnit != null && _unit.AttackTargetUnit.IsDied == false)
        {
            if (_unit.AttackData.ProjectilePrefab == null)
            {
                _unit.AttackTargetUnit.TakeDamage(_unit.AttackData);
            }
            else
            {
                ProjectileAbstract projectile = Instantiate(_unit.AttackData.ProjectilePrefab, _unit.ProjectileSpawnPosition.transform.position, Quaternion.identity, _unit.ProjectileSpawnPosition.transform);
                projectile.AttackData = _unit.AttackData;
                projectile.InitByTarget(_unit.AttackTargetUnit.gameObject);
            }
        }
    }
}
