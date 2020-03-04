using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector_MasterSkill : Selector
{
    public UIMasterSkillMenu UIMasterSkillMenu;

    Coroutine _coroutine;
    MasterSkillData _masterSkillData;

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
        MapManager.Inst.SetHighLightWay(true);
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
                _masterSkillData = UIMasterSkillMenu.SelectedMasterSkillButton.MasterSkillData;
                // projectile
                // Unit이 있을 경우 Unit을 생성
                if (_masterSkillData.UnitPrefab != null)
                {
                    if (_masterSkillData.SpawnCount <= 1)
                    {
                        CreateUnit(targetPosition);
                    }
                    else
                    {
                        if (_coroutine != null)
                        {
                            StopCoroutine(_coroutine);
                        }
                        _coroutine = StartCoroutine(Coroutine_CreateUnit(targetPosition));
                    }
                }
                // Unit이 없을 경우 Projectile 생성
                else if (_masterSkillData.AttackData != null)
                {
                    if (_masterSkillData.SpawnCount <= 1)
                    {
                        CreateProjectile(targetPosition);
                    }
                    else
                    {
                        if (_coroutine != null)
                        {
                            StopCoroutine(_coroutine);
                        }
                        _coroutine = StartCoroutine(Coroutine_CreateProjectile(targetPosition));
                    }
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
        Vector3 startPosition = targetPosition + _masterSkillData.SpawnPositionOffset;
        Quaternion startAngle = Quaternion.Euler(_masterSkillData.SpawnAngle.x, _masterSkillData.SpawnAngle.y, _masterSkillData.SpawnAngle.z);
        AProjectile projectile = Instantiate<AProjectile>(_masterSkillData.AttackData.ProjectilePrefab, startPosition, startAngle, transform);
        projectile.gameObject.SetActive(true);
        projectile.Init(StageManager.Inst.StageData.PlayerTeamData, _masterSkillData.AttackData, null, targetPosition);
    }

    IEnumerator Coroutine_CreateProjectile(Vector3 position)
    {
        for (int i = 0; i < _masterSkillData.SpawnCount; i++)
        {
            // 투사체는 원 안의 랜덤한 위치에 생성
            Vector3 positionOffset = Random.insideUnitCircle * _masterSkillData.SpawnRadius;
            positionOffset.y *= 0.7f; // 타원안에 생성
            CreateProjectile(position + positionOffset);
            yield return new WaitForSeconds(_masterSkillData.SpawnInterval);
        }
    }

    void CreateUnit(Vector3 targetPosition)
    {
        Vector3 startPosition = targetPosition + _masterSkillData.SpawnPositionOffset;
        Quaternion startAngle = Quaternion.Euler(_masterSkillData.SpawnAngle.x, _masterSkillData.SpawnAngle.y, _masterSkillData.SpawnAngle.z);
        Unit unit = Instantiate<Unit>(_masterSkillData.UnitPrefab, startPosition, startAngle, UIMasterSkillMenu.UnitContainer.transform);
        unit.gameObject.SetActive(true);
        unit.AttackDatas[0] = _masterSkillData.AttackData;
        unit.AttackDataIndex = 0;
        unit.UnitMovePoint.SetRallyPoint(startPosition);
    }

    IEnumerator Coroutine_CreateUnit(Vector3 position)
    {
        float startAngle = Random.Range(-30f, 30f);
        for (int i = 0; i < _masterSkillData.SpawnCount; i++)
        {
            // 유닛은 원 둘레에 일정한 각도로 생성, 시작 각도는 램덤
            Vector3 positionOffset = Quaternion.Euler(0f, 0f, startAngle + (360f / _masterSkillData.SpawnCount) * i) * Vector3.right * _masterSkillData.SpawnRadius;
            positionOffset.y *= 0.7f; // 타원안에 생성
            CreateUnit(position + positionOffset);
            yield return new WaitForSeconds(_masterSkillData.SpawnInterval);
        }
    }

    override public void SelectExit()
    {
        UIMasterSkillMenu.Deselect();
        base.SelectExit();
        MapManager.Inst.SetHighLightWay(false);
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
