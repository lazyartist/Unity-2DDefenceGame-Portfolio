﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector_HeroUnit : Selector
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
        _selectResult.SelectResultType = Types.SelectResultType.Register;
        _selectResult.IsFlag = false;
        _selectResult.IsSpread = true;

        UICanvas.Inst.HeroUnitButton.Select(true);
        UICanvas.Inst.ShowUnitInfo(Unit);

        return _selectResult;
    }

    override public Types.SelectResult SelectUpdate(Vector3 position, bool isOnWay)
    {
        if (isOnWay)
        {
            Unit.SetRallyPoint(position);
        }
        _selectResult.CursorType = isOnWay ? Types.CursorType.Success : Types.CursorType.Fail;
        _selectResult.SelectResultType = isOnWay ? Types.SelectResultType.Unregister : Types.SelectResultType.None;
        _selectResult.IsFlag = true;
        _selectResult.IsSpread = false;
        return _selectResult;
    }

    override public void SelectExit()
    {
        base.SelectExit();

        UICanvas.Inst.HeroUnitButton.Select(false);
        UICanvas.Inst.HideUnitInfo();
    }

}