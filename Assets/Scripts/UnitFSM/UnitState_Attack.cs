using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Attack : AUnitState
{
    public bool IsPlayAttackAni = true;

    static string[] _attackTriggerNames = { "Attack0", "Attack1", "Attack2", "Attack3" };

    float[] _lastAttackFireTimes = { 0f, 0f, 0f, 0f };
    bool _isPlayingAttackAni;

    // implements AUnitState
    public override void EnterState(Unit unit)
    {
        // todo move to Init()
        for (int i = 0; i < unit.AttackDatas.Length; i++)
        {
            if (unit.AttackDatas[i].IsStartDelayForCoolTime)
            {
                _lastAttackFireTimes[i] = Time.time;
            }
            else
            {
                _lastAttackFireTimes[i] = 0f;
            }
        }

        if (unit.HasEnemyUnit() == false)
        {
            // 다른 유닛의 원거리 공격등으로 공격대상이 이미 죽었다
        }
        else
        {
            _unit = unit;

            unit.UnitEvent += OnUnitEventHandler;
            if (unit.GetAttackData().ProjectilePrefab == null)
            {
                // 근거리 공격 : 공격을 통보하여 Attack 상태로 전환시킴
                unit.EnemyUnit.Notify(Types.UnitNotifyType.BeAttackState, unit);
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
            if (unit.IsValidEnemyUnit() == false)
            {
                return unitStates[(int)Types.UnitFSMType.Idle];
            }
            // 공격대상이 공격 범위를 벗어나면 EnemyUnit을 제거하고 Idle 상태로 전환
            else if (unit.IsValidEnemyUnitInRange() == false)
            {
                unit.Notify(Types.UnitNotifyType.ClearEnemyUnit, null);
                return unitStates[(int)Types.UnitFSMType.Idle];
            }
            // 공격대상이 있고 쿨타임이 지났으면 공격
            else
            {
                float biggestElapsedCoolTime = 0f;
                int attackDataIndex = 0;
                // 쿨타임이 지나서 공격가능한 공격 인덱스 검색
                for (int i = 0; i < _unit.AttackDatas.Length; i++)
                {
                    // 쿨타임이 가장 오래된 공격부터 실행
                    float elapsedCoolTime = Time.time - _lastAttackFireTimes[i];
                    if (elapsedCoolTime >= _unit.AttackDatas[i].CoolTime)
                    {
                        if (elapsedCoolTime > biggestElapsedCoolTime)
                        {
                            biggestElapsedCoolTime = elapsedCoolTime;
                            attackDataIndex = i;
                        }
                    }
                }

                if (biggestElapsedCoolTime > 0)
                {
                    unit.AttackDataIndex = attackDataIndex;
                    //Debug.Log("Attack " + unit + " to " + unit.EnemyUnit);
                    if (unit.CanChangeDirection)
                    {
                        unit.Toward(unit.EnemyUnit.transform.position);
                    }

                    if (IsPlayAttackAni)
                    {
                        _isPlayingAttackAni = true; // AniEvent 호출 타이밍이 정확하지 않기 때문에 여기서 지정한다.
                        unit.UnitBody.Animator.SetTrigger(_attackTriggerNames[unit.AttackDataIndex]);
                        AudioManager.Inst.PlayAttackStart(_unit.GetAttackData());
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
        AudioManager.Inst.PlayAttackFire(_unit.GetAttackData());

        // 공격이 발사체가 아닌 경우
        AttackData attackData = _unit.GetAttackData();
        if (attackData.ProjectilePrefab == null)
        {
            Hit();
        }
        // 공격이 발사체인 경우
        else
        {
            AProjectile projectile = Instantiate(attackData.ProjectilePrefab, _unit.SpawnPosition.transform.position, Quaternion.identity, _unit.SpawnPosition.transform);
            // 공격대상이 살아있는 경우
            if (_unit.IsValidEnemyUnit())
            {
                projectile.Init(_unit.TeamData, attackData, _unit.EnemyUnit, _unit.EnemyUnit.gameObject.transform.position);
            }
            else
            {
                projectile.Init(_unit.TeamData, attackData, null, _unit.LastEnemyPosition);
            }
        }

        _lastAttackFireTimes[_unit.AttackDataIndex] = Time.time;
    }

    void Hit()
    {
        // 공격이 히트하는 순간 다른 유닛의 공격에 의해 공격대상이 이미 죽었을 수 있다
        if (_unit.HasEnemyUnit())
        {
            AudioManager.Inst.PlayAttackHit(_unit.GetAttackData());
            _unit.EnemyUnit.TakeDamage(_unit.GetAttackData());
        }
    }
}
