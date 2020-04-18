using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Attack : AUnitState
{
    [Tooltip("true : Attack 애니 재생, false : Attack 애니 재생하지 않고 바로 AttackFire")]
    public bool IsPlayAttackAni = true;

    protected float _lastAttackFireTime;
    protected AttackData _attackData;
    protected bool _isPlayingAttackAni; // Attack 애니가 재생 중인지 여부

    public override void Init(Unit unit, AUnitState[] unitStates)
    {
        base.Init(unit, unitStates);
    }

    // implements AUnitState
    public override void EnterState()
    {
        if (_unit.HasEnemyUnit() == false)
        {
            // 다른 유닛의 원거리 공격등으로 공격대상이 이미 죽었다
        }
        else
        {
            _unit.UnitEvent += OnUnitEventHandler;
            if (_unit.UnitTargetRangeType == Types.UnitTargetRangeType.Short)
            {
                // 근거리 공격 : 공격을 통보하여 Attack 상태로 전환시킴
                _unit.EnemyUnit.Notify(Types.UnitNotifyType.BeAttackState, _unit);
            }
            else
            {
                // 원거리 공격 : 공격을 통보하지 않음
            }
        }
    }

    public override void ExitState()
    {
        _unit.UnitEvent -= OnUnitEventHandler;
        _unit.ClearEnemyUnit();
        _isPlayingAttackAni = false;
    }

    public override AUnitState UpdateState()
    {
        // 공격 애니가 끝날때까지 기다린다
        if (_isPlayingAttackAni == false)
        {
            // 공격대상이 없으면 Idle 상태로 전환
            if (_unit.IsValidEnemyUnit() == false)
            {
                return unitStates[(int)Types.UnitFSMType.Idle];
            }
            // 공격대상이 공격 범위를 벗어나면
            else if (_unit.IsValidEnemyUnitInRange() == false)
            {
                // EnemyUnit을 제거
                _unit.Notify(Types.UnitNotifyType.ClearEnemyUnit, null);
                if (_unit.TryFindEnemy())
                {
                    // 새로운 적을 찾았다면 Attack 상태 유지
                    //Debug.Log("new enemy");
                    return null;
                }
                else
                {
                    // 새로운 적을 못찾았으면 Idle 상태로 전환
                    //Debug.Log("no enemy");
                    return unitStates[(int)Types.UnitFSMType.Idle];
                }
            }
            // 현재 공격이 원거리 공격이면 근거리 적이 있는지 검사
            else if (_unit.UnitTargetRangeType == Types.UnitTargetRangeType.Long && _unit.TryFindShortRangeEnemy())
            {
                return unitStates[(int)Types.UnitFSMType.Idle];
            }
            // 공격대상이 있고 쿨타임이 지났으면 공격
            else
            {
                float elapsedCoolTime = Time.time - _lastAttackFireTime;
                if (elapsedCoolTime >= _unit.UnitData.AttackCoolTime)
                {
                    _attackData = _unit.GetAttackData();
                    //Debug.Log("Attack " + unit + " to " + unit.EnemyUnit);
                    if (_unit.CanChangeDirection)
                    {
                        _unit.Toward(_unit.EnemyUnit.transform.position);
                    }

                    if (IsPlayAttackAni)
                    {
                        _isPlayingAttackAni = true; // AniEvent 호출 타이밍이 정확하지 않기 때문에 여기서 지정한다.
                        _unit.UnitBody.Animator.SetTrigger(_attackData.AttackAniName);
                        AudioManager.Inst.PlayAttackStart(_attackData);
                    }
                    else
                    {
                        AttackFire();
                    }
                }
            }
        }

        return null;
    }

    virtual protected void OnUnitEventHandler(Types.UnitEventType unitBodyEventType, Unit unit)
    {
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
                {
                    _isPlayingAttackAni = false;
                }
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

    virtual protected void AttackFire()
    {
        AudioManager.Inst.PlayAttackFire(_attackData);

        // 공격이 발사체가 아닌 경우
        if (_attackData.ProjectilePrefab == null)
        {
            Hit();
        }
        // 공격이 발사체인 경우
        else
        {
            AProjectile projectile = Instantiate(_attackData.ProjectilePrefab, _unit.SpawnPosition.transform.position, Quaternion.identity, _unit.SpawnPosition.transform);
            if (_unit.IsValidEnemyUnit())
            {
                // 공격대상이 살아있는 경우
                projectile.Init(_unit.TeamData, _attackData, _unit.EnemyUnit, _unit.EnemyUnit.gameObject.transform.position);
            }
            else
            {
                // todo 다시 살리기
                // 공격대상이 죽은 경우 - 적을 다시 검색한다. 
                //_unit.TryFindEnemyOrNull();

                // 공격대상이 죽은 경우 - 공격 애니가 재생중이기 때문에 발사해야 한다.
                projectile.Init(_unit.TeamData, _attackData, _unit.EnemyUnit/*TryFindEnemyOrNull()가 실패한 경우 null*/, _unit.LastEnemyPosition);
            }
        }

        _lastAttackFireTime = Time.time;
    }

    protected void Hit()
    {
        // 공격이 히트하는 순간 다른 유닛의 공격에 의해 공격대상이 이미 죽었을 수 있다
        if (_unit.HasEnemyUnit())
        {
            AudioManager.Inst.PlayAttackHit(_attackData);
            _unit.EnemyUnit.TakeDamage(_attackData);
        }
    }
}
