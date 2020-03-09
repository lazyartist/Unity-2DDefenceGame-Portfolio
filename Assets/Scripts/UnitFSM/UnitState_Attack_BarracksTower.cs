using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UnitState_Attack_BarracksTower : AUnitState
{
    ChildUnitCreator _childUnitCreator;
    bool _isPlayingAttackAni;

    void Awake()
    {
        _childUnitCreator = GetComponent<ChildUnitCreator>();
    }

    // implements AUnitState
    public override void EnterState()
    {
        _unit.UnitEvent += OnUnitEventHandler;
    }

    public override void ExitState()
    {
        _unit.UnitEvent -= OnUnitEventHandler;
    }

    public override AUnitState UpdateState()
    {
        // 공격 애니가 끝날때까지 기다린다
        if (_isPlayingAttackAni == false)
        {
            // 사망 유닛의 쿨타임이 지났다면 생성 시작
            if (_childUnitCreator.ShortestCoolTimeDiedUnitIndex != -1 && (Time.time - _childUnitCreator.UnitDiedTimes[_childUnitCreator.ShortestCoolTimeDiedUnitIndex]) > _unit.GetAttackData().CoolTime)
            {
                _unit.UnitBody.Animator.SetTrigger("Attack");
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
        // 쿨타임이 지난 모든 유닛을 생성
        _childUnitCreator.CreateAllUnits(_unit.GetAttackData().CoolTime);
        //for (int i = 0; i < _childUnitCreator.MaxUnitCount; i++)
        //{
        //    if(_childUnitCreator.DiedUnitIndex == -1)
        //    {
        //        break;
        //    }

        //    if ((Time.time - _childUnitCreator.UnitDiedTimes[_childUnitCreator.DiedUnitIndex]) > _unit.GetAttackData().CoolTime)
        //    {
        //        _childUnitCreator.CreateUnits(_childUnitCreator.DiedUnitIndex);
        //    }
        //}
    }
}
