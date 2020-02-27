using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector_MasterSkill : Selector
{
    public UIMasterSkillMenu UIMasterSkillMenu;

    Coroutine _coroutine;
    AttackData _attackData;
    AttackTargetData _attackTargetData;

    override protected void Start()
    {
        UIMasterSkillMenu = GetComponent<UIMasterSkillMenu>();
        UIMasterSkillMenu.MasterSkillEvent += OnMasterSkillEvent_UIMasterSkillMenu;
        base.Start();
    }

    void OnApplicationQuit()
    {
        UIMasterSkillMenu.MasterSkillEvent -= OnMasterSkillEvent_UIMasterSkillMenu;
    }

    void OnDestroy()
    {
        UIMasterSkillMenu.MasterSkillEvent -= OnMasterSkillEvent_UIMasterSkillMenu;
    }

    override public Types.SelectResult SelectEnter()
    {
        base.SelectEnter();
        _selectResult.CursorType = Types.CursorType.None;
        _selectResult.IsFlag = false;
        _selectResult.SelectResultType = Types.SelectResultType.None;
        return _selectResult;
    }

    override public Types.SelectResult SelectUpdate(Vector3 targetPosition, bool isOnWay)
    {
        if (UIMasterSkillMenu.SelectedMasterSkillButton != null)
        {
            _selectResult.CursorType = isOnWay ? Types.CursorType.Success : Types.CursorType.Fail;
            _selectResult.SelectResultType = isOnWay ? Types.SelectResultType.Unregister : Types.SelectResultType.None;
            _selectResult.IsFlag = false;
            _selectResult.IsSpread = false;

            if (isOnWay)
            {
                UIMasterSkillMenu.SelectedMasterSkillButton.Reset();
                _attackData = UIMasterSkillMenu.SelectedMasterSkillButton.MasterSkillData.AttackData;
                _attackTargetData = UIMasterSkillMenu.SelectedMasterSkillButton.MasterSkillData.AttackTargetData;
                if (_attackData.ProjectileCount <= 1)
                {
                    CreateProjectile(targetPosition);
                }
                else
                {
                    if(_coroutine != null)
                    {
                        StopCoroutine(_coroutine);
                    }
                    _coroutine = StartCoroutine(Coroutine_CreateProjectile(targetPosition));
                }
            }
        }
        else
        {
            _selectResult.CursorType = Types.CursorType.None;
            _selectResult.SelectResultType = Types.SelectResultType.Unregister;
            _selectResult.IsFlag = false;
            _selectResult.IsSpread = false;
        }

        return _selectResult;
    }

    void CreateProjectile(Vector3 targetPosition)
    {

        Vector3 startPosition = targetPosition + _attackData.ProjectileSpawnPositionOffset;
        //Vector3 startAngle = _attackData.ProjectileSpawnAngle;
        Quaternion startAngle = Quaternion.Euler(_attackData.ProjectileSpawnAngle.x, _attackData.ProjectileSpawnAngle.y, _attackData.ProjectileSpawnAngle.z);
        AProjectile projectile = Instantiate<AProjectile>(_attackData.ProjectilePrefab, startPosition, startAngle, transform);
        projectile.Init(_attackData, _attackTargetData, null, targetPosition);
    }

    IEnumerator Coroutine_CreateProjectile(Vector3 position)
    {
        for (int i = 0; i < _attackData.ProjectileCount; i++)
        {
            Vector3 positionOffset = Random.insideUnitCircle * _attackData.ProjectileSpawnRadius;
            CreateProjectile(position + positionOffset);
            yield return new WaitForSeconds(_attackData.ProjectileSpawnInterval);
        }
    }

    override public void SelectExit()
    {
        UIMasterSkillMenu.Deselect();
        base.SelectExit();
    }

    override protected void UpdateSelected()
    {
        // 선택 상태를 따로 구현한다.
        //base.UpdateSelected();
    }

    void OnMasterSkillEvent_UIMasterSkillMenu(Types.MasterSkillEventType masterSkillEventType)
    {
        switch (masterSkillEventType)
        {
            case Types.MasterSkillEventType.None:
                break;
            case Types.MasterSkillEventType.Selected:
                SelectorManager.Inst.RegisterSelector(this);
                break;
            case Types.MasterSkillEventType.Unselected:
                SelectorManager.Inst.UnregisterSelector();
                break;
            default:
                break;
        }
    }
}
