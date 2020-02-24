using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Attack : AUnitState
{
    public bool IsPlayAttackAni = true;

    private float _lastAttackFireTime = 0.0f;
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
            // 공격대상이 공격 범위를 벗어나면 AttackTargetUnit을 제거하고 Idle 상태로 전환
            else if (unit.IsValidAttackTargetUnitInRange() == false)
            {
                unit.Notify(Types.UnitNotifyType.ClearAttackTarget, null);
                return unitStates[(int)Types.UnitFSMType.Idle];
            }
            // 공격대상이 있고 쿨타임이 지났으면 공격
            else if (Time.time - _lastAttackFireTime >= unit.UnitData.AttackCoolTime)
            {
                //Debug.Log("Attack " + unit + " to " + unit.AttackTargetUnit);
                if (unit.CanChangeDirection)
                {
                    unit.Toward(unit.AttackTargetUnit.transform.position);
                }

                if (IsPlayAttackAni)
                {
                    unit.UnitBody.Animator.SetTrigger("Attack");
                }
                else
                {
                    AttackFire();
                }
            }
        }

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
        // 공격이 발사체가 아닌 경우
        if (_unit.AttackData.ProjectilePrefab == null)
        {
            Hit();
        }
        // 공격이 발사체인 경우
        else
        {
            AProjectile projectile = Instantiate(_unit.AttackData.ProjectilePrefab, _unit.SpawnPosition.transform.position, Quaternion.identity, _unit.SpawnPosition.transform);
            projectile.AttackData = _unit.AttackData;
            projectile.AttackTargetData = _unit.AttackTargetData;
            //projectile.Init(_unit.AttackData, _unit.AttackTargetData, _unit.IsValidAttackTargetUnit() ? _unit.AttackTargetUnit.gameObject : null, _unit.LastAttackTargetPosition);
            // 공격대상이 살아있는 경우
            if (_unit.IsValidAttackTargetUnit())
            {
                projectile.Init(_unit.AttackData, _unit.AttackTargetData, _unit.AttackTargetUnit, _unit.AttackTargetUnit.gameObject.transform.position);
                //projectile.Init(_unit.AttackData, _unit.AttackTargetData, _unit.AttackTargetUnit.gameObject, _unit.AttackTargetUnit.gameObject.transform.position);
                //projectile.InitByTarget(_unit.AttackTargetUnit.gameObject);
            }
            else
            {
                projectile.Init(_unit.AttackData, _unit.AttackTargetData, null, _unit.LastAttackTargetPosition);
                //projectile.InitByPosition(_unit.LastAttackTargetPosition);
            }
        }

        _lastAttackFireTime = Time.time;
    }

    void Hit()
    {
        // 공격이 히트하는 순간 다른 유닛의 공격에 의해 공격대상이 이미 죽었을 수 있다
        if (_unit.HasAttackTargetUnit())
        {
            _unit.AttackTargetUnit.TakeDamage(_unit.AttackData);
        }
    }
}
