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

    override public Types.SelectResult Select()
    {
        base.Select();
        //Tower.Select();
        _selectResult.CursorType = Types.CursorType.None;
        _selectResult.IsFlag = false;
        _selectResult.SelectResultType = Types.SelectResultType.Select;
        return _selectResult;
    }

    override public Types.SelectResult SelectNext(Selector selector, Vector3 position, bool isOnWay)
    {
        if(UIMasterSkillMenu.SelectedMasterSkillButton != null)
        {
            _selectResult.CursorType = isOnWay ? Types.CursorType.Success : Types.CursorType.Fail;
            _selectResult.IsFlag = true;
            _selectResult.SelectResultType = Types.SelectResultType.Deselect;

            AProjectile projectile = Instantiate<AProjectile>(UIMasterSkillMenu.SelectedMasterSkillButton.MasterSkillData.AttackData.ProjectilePrefab, position, Quaternion.identity, transform);
            projectile.InitByPosition(position);
        } else
        {
            _selectResult.CursorType = Types.CursorType.None;
            _selectResult.IsFlag = false;
            _selectResult.SelectResultType = Types.SelectResultType.None;
        }
        
        return _selectResult;
    }

    override public void Deselect()
    {
        UIMasterSkillMenu.Deselect();
        base.Deselect();
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
