using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector_Unit : Selector
{
    public Unit Unit;

    override protected void Start()
    {
        base.Start();
        Unit = GetComponent<Unit>();
    }

    override public Types.SelectResult SelectEnter()
    {
        base.SelectEnter();
        //Unit.Select();
        _selectResult.CursorType = Types.CursorType.None;
        _selectResult.SelectResultType = Types.SelectResultType.Select;
        _selectResult.IsFlag = false;
        _selectResult.IsSpread = true;

        UICanvas.Inst.ShowUnitInfo(Unit);

        return _selectResult;
    }

    override public Types.SelectResult SelectUpdate(Selector selector, Vector3 position, bool isOnWay)
    {
        _selectResult.CursorType = Types.CursorType.None;
        _selectResult.SelectResultType = Types.SelectResultType.Deselect;
        _selectResult.IsFlag = false;
        _selectResult.IsSpread = true;
        return _selectResult;
    }

    override public void SelectExit()
    {
        base.SelectExit();

        UICanvas.Inst.HideUnitInfo();
    }
}
