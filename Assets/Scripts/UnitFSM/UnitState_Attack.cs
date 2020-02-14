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
        if (unit.HasAttackTargetUnit() == false)
        {
            // 다른 유닛의 원거리 공격등으로 공격대상이 이미 죽었다
        }
        else
        {
            _unit = unit;

            _coolTime = 0.0f;
            unit.UnitEvent += OnUnitEventHandler;
            if (unit.AttackData.ProjectilePrefab == null)
            {
                // 근거리 공격 : 공격을 통보하여 Attack 상태로 전환시킴
                unit.AttackTargetUnit.Notify(Types.UnitNotifyType.BeAttackState, unit);
            }
            else
            {
                // 원거리 공격 : 공격을 통보하지 않음
            }
        }
    }
    public override void ExitState(Unit unit)
    {
        unit.UnitEvent -= OnUnitEventHandler;
        _isPlayingAttackAni = false;
        _coolTime = 0.0f;
    }
    public override AUnitState UpdateState(Unit unit, AUnitState[] unitStates)
    {
        // 공격 애니가 끝날때까지 기다린다
        if (_isPlayingAttackAni == false)
        {
            // 공격대상이 없으면 Idle 상태로 전환
            if (unit.IsValidAttackTargetUnit() == false)
            {
                return unitStates[(int)Types.UnitFSMType.Idle];
            }
            // 공격대상이 있고 쿨타임이 지났으면 공격
            else if (_coolTime <= 0.0f)
            {
                //Debug.Log("Attack " + unit + " to " + unit.AttackTargetUnit);
                unit.Toward(unit.AttackTargetUnit.transform.position);
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
            case Types.UnitEventType.AttackStopped:
                break;
            default:
                break;
        }
    }
    virtual public void Attack()
    {
        // 공격이 발사체가 아닌 경우
        if (_unit.AttackData.ProjectilePrefab == null)
        {
            // 공격하는 순간 다른 유닛의 공격에 의해 공격대상이 이미 죽었을 수 있다
            if(_unit.HasAttackTargetUnit())
            {
                _unit.AttackTargetUnit.TakeDamage(_unit.AttackData);
            }
        }
        // 공격이 발사체인 경우
        else
        {
            AProjectile projectile = Instantiate(_unit.AttackData.ProjectilePrefab, _unit.SpawnPosition.transform.position, Quaternion.identity, _unit.SpawnPosition.transform);
            projectile.AttackData = _unit.AttackData;
            projectile.AttackTargetData = _unit.AttackTargetData;
            // 공격대상이 살아있는 경우
            if (_unit.IsValidAttackTargetUnit())
            {
                projectile.InitByTarget(_unit.AttackTargetUnit.gameObject);
            }
            else
            {
                projectile.InitByPosition(_unit.LastAttackTargetPosition);
            }
        }
    }
}
