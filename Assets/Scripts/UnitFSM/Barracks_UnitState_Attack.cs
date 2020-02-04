using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// todo naming UnitState_Attack_Barracks -> Barracks_UnitState_Attack
public class Barracks_UnitState_Attack : AUnitState
{
    private ChildUnits _childUnits;
    private float _coolTime = 0.0f;
    private bool _isPlayingAttackAni;

    private void Awake()
    {
        _childUnits = GetComponent<ChildUnits>();
    }

    // implements AUnitState
    public override void EnterState(Unit unit)
    {
        _unit = unit;

        _coolTime = 0.0f;
        unit.UnitEvent += _OnUnitEventHandler;
    }
    public override void ExitState(Unit unit)
    {
        unit.UnitEvent -= _OnUnitEventHandler;
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
    void _OnUnitEventHandler(Types.UnitEventType unitBodyEventType, Unit unit)
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
        for (int i = 0; i < _childUnits.MaxUnitCount; i++)
        {
            if(_childUnits.Units[i] == null)
            {
                Unit childUnit = Instantiate(_childUnits.ChildUnitPrefab, _unit.transform.position + _unit.UnitCenterOffset, Quaternion.identity, _unit.transform);
                childUnit.UnitEvent += _OnUnitEventHandler_ChildUnit;
                childUnit.WaitWaypoint = WaypointManager.Inst.WaypointPool.Get();
                Vector3 position = Quaternion.Euler(0f, 0f, (360f / 3f) * i) * (Vector3.up * 0.3f);
                childUnit.WaitWaypoint.transform.position = _unit.ProjectileSpawnPosition.transform.position + position;
                _childUnits.Units[i] = childUnit;
            }
        }
    }
    void _OnUnitEventHandler_ChildUnit(Types.UnitEventType unitBodyEventType, Unit unit)
    {
        Debug.Log("ChildUnitEventListener " + unitBodyEventType);

        switch (unitBodyEventType)
        {
            case Types.UnitEventType.None:
                break;
            case Types.UnitEventType.Attack:
                //Attack();
                break;
            case Types.UnitEventType.AttackStart:
                //_isPlayingAttackAni = true;
                break;
            case Types.UnitEventType.AttackEnd:
                //_isPlayingAttackAni = false;
                break;
            case Types.UnitEventType.Die:
                unit.UnitEvent -= _OnUnitEventHandler_ChildUnit;
                break;
            case Types.UnitEventType.DiedComplete:
                break;
            default:
                break;
        }
    }
}
