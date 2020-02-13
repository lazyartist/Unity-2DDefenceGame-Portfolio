using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector_Tower : Selector {
    public Tower Tower;

	override protected void Start () {
        Tower = GetComponent<Tower>();
        base.Start();
    }
	
    override public Types.SelectResult Select()
    {
        base.Select();
        Tower.Select();
        _selectResult.CursorType = Types.CursorType.None;
        _selectResult.IsFlag = false;
        _selectResult.SelectResultType = Types.SelectResultType.Select;
        return _selectResult;
    }

    override public Types.SelectResult SelectNext(Selector selector, Vector3 position, bool isOnWay)
    {
        _selectResult.CursorType = isOnWay ? Types.CursorType.Success : Types.CursorType.Fail;
        _selectResult.IsFlag = false;
        _selectResult.SelectResultType = Types.SelectResultType.Deselect;
        return _selectResult;
    }

    override public void Deselect()
    {
        Tower.Deselect();
        base.Deselect();
    }

    override protected void UpdateSelected()
    {
        //base.UpdateSelected();

        //if(Tower.Unit == null)
        //{
        //    SelectedSR.enabled = false;
        //}
        //else
        //{
        //    SelectedSR.transform.position = Tower.Unit.transform.position + Tower.Unit.UnitCenterOffset;
        //    SelectedSR.transform.localScale = new Vector3(Tower.Unit.UnitData.TargetRange * 2f, Tower.Unit.UnitData.TargetRange * 2f, 1);
        //    SelectedSR.enabled = Selected;
        //}
    }
}
