﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector_Tower : Selector
{
    public TowerBuilder Tower;

    override protected void Start()
    {
        Tower = GetComponent<TowerBuilder>();
        base.Start();
    }

    override public Types.SelectResult SelectEnter()
    {
        base.SelectEnter();
        Tower.Select();
        _selectResult.CursorType = Types.CursorType.None;
        _selectResult.SelectResultType = Types.SelectResultType.Register;
        _selectResult.IsFlag = false;
        _selectResult.IsSpread = false;
        return _selectResult;
    }

    override public Types.SelectResult SelectUpdate(Vector3 position, bool isOnWay)
    {
        if (Tower.IsRallyPointModeOn)
        {
            // barracks rallypoint
            _selectResult.CursorType = isOnWay ? Types.CursorType.Success : Types.CursorType.Fail;
            _selectResult.SelectResultType = Types.SelectResultType.Unregister;
            _selectResult.IsFlag = isOnWay;
            _selectResult.IsSpread = false;

            if (isOnWay)
            {
                ChildUnitCreator childUnitCreator = Tower.Unit.GetComponent<ChildUnitCreator>();
                childUnitCreator.RallyPointInLocal = position - Tower.Unit.transform.position;
                childUnitCreator.SetRallyPointToAllUnits();
            }
        }
        else
        {
            _selectResult.CursorType = Types.CursorType.None;
            _selectResult.SelectResultType = Types.SelectResultType.Unregister;
            _selectResult.IsFlag = false;
            _selectResult.IsSpread = true;
        }

        return _selectResult;
    }

    override public void SelectExit()
    {
        Tower.Deselect();
        base.SelectExit();
    }

    override protected void UpdateSelected()
    {
        //base.UpdateSelected();
    }
}
