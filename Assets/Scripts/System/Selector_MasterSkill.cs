using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector_MasterSkill : Selector
{
    public UIMasterSkillMenu UIMasterSkillMenu;

    override protected void Start()
    {
        UIMasterSkillMenu = GetComponent<UIMasterSkillMenu>();
        UIMasterSkillMenu.MasterSkillEvent += OnMasterSkillEvent_UIMasterSkillMenu;
        base.Start();
    }

    private void OnApplicationQuit()
    {
        UIMasterSkillMenu.MasterSkillEvent -= OnMasterSkillEvent_UIMasterSkillMenu;
    }

    private void OnDestroy()
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

    override public Types.SelectResult SelectUpdate(Selector selector, Vector3 position, bool isOnWay)
    {
        if (UIMasterSkillMenu.SelectedMasterSkillButton != null)
        {
            _selectResult.CursorType = isOnWay ? Types.CursorType.Success : Types.CursorType.Fail;
            _selectResult.SelectResultType = isOnWay ? Types.SelectResultType.Deselect : Types.SelectResultType.None;
            _selectResult.IsFlag = false;
            _selectResult.IsSpread = false;

            if (isOnWay)
            {
                UIMasterSkillMenu.SelectedMasterSkillButton.Reset();
                AProjectile projectile = Instantiate<AProjectile>(UIMasterSkillMenu.SelectedMasterSkillButton.MasterSkillData.AttackData.ProjectilePrefab, position, Quaternion.identity, transform);
                projectile.Init(UIMasterSkillMenu.SelectedMasterSkillButton.MasterSkillData.AttackData, UIMasterSkillMenu.SelectedMasterSkillButton.MasterSkillData.AttackTargetData, null, position);
            }
        }
        else
        {
            _selectResult.CursorType = Types.CursorType.None;
            _selectResult.SelectResultType = Types.SelectResultType.Deselect;
            _selectResult.IsFlag = false;
            _selectResult.IsSpread = false;
        }

        return _selectResult;
    }

    override public void SelectExit()
    {
        UIMasterSkillMenu.Deselect();
        base.SelectExit();
    }

    override protected void UpdateSelected()
    {
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
