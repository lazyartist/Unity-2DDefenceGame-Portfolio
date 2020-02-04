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

        _coolTime = 0.0f;
        unit.UnitEvent += OnUnitEventHandler;
        if (unit.AttackData.ProjectilePrefab == null)
        {
            // 근거리 공격 : 공격을 통보하여 Attack 상태로 전환시킴
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
                unit.Toward(unit.AttackTargetUnit.transform.position);
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
        // 공격이 발사체가 아닌 경우
        if (_unit.AttackData.ProjectilePrefab == null)
        {
            _unit.AttackTargetUnit.TakeDamage(_unit.AttackData);
        }
        // 공격이 발사체인 경우
        else
        {
            AProjectile projectile = Instantiate(_unit.AttackData.ProjectilePrefab, _unit.ProjectileSpawnPosition.transform.position, Quaternion.identity, _unit.ProjectileSpawnPosition.transform);
            projectile.AttackData = _unit.AttackData;
            projectile.AttackTargetData = _unit.AttackTargetData;
            // 공격대상이 살아있는 경우
            if (_unit.AttackTargetUnit != null && _unit.AttackTargetUnit.IsDied == false)
            {
                projectile.InitByTarget(_unit.AttackTargetUnit.gameObject);
            }
            else
            {
                projectile.InitByPosition(_unit.LastAttackTargetPosition);
            }
            //Fire();
        }

        //if (_unit.AttackTargetUnit != null && _unit.AttackTargetUnit.IsDied == false)
        //{
        //    if (_unit.AttackData.ProjectilePrefab == null)
        //    {
        //        _unit.AttackTargetUnit.TakeDamage(_unit.AttackData);
        //    }
        //    else
        //    {
        //        Fire();
        //    }
        //}
    }

    //private void Fire()
    //{
    //    ProjectileAbstract projectile = Instantiate(_unit.AttackData.ProjectilePrefab, _unit.ProjectileSpawnPosition.transform.position, Quaternion.identity, _unit.ProjectileSpawnPosition.transform);
    //    projectile.AttackData = _unit.AttackData;
    //    projectile.AttackTargetData = _unit.AttackTargetData;
    //    projectile.InitByTarget(_unit.AttackTargetUnit.gameObject);
    //}
}
