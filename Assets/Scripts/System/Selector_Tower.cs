using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector_Tower : Selector
{
    public Tower Tower;

    override protected void Start()
    {
        Tower = GetComponent<Tower>();
        base.Start();
    }

    override public Types.SelectResult SelectEnter()
    {
        base.SelectEnter();
        Tower.Select();
        _selectResult.CursorType = Types.CursorType.None;
        _selectResult.SelectResultType = Types.SelectResultType.Select;
        _selectResult.IsFlag = false;
        _selectResult.IsSpread = false;
        return _selectResult;
    }

    override public Types.SelectResult SelectUpdate(Selector selector, Vector3 position, bool isOnWay)
    {
        if (Tower.IsRallyPointModeOn)
        {
            // barracks rallypoint
            _selectResult.CursorType = isOnWay ? Types.CursorType.Success : Types.CursorType.Fail;
            _selectResult.SelectResultType = Types.SelectResultType.Deselect;
            _selectResult.IsFlag = isOnWay;
            _selectResult.IsSpread = false;

            if (isOnWay)
            {
                ChildUnits childUnits = Tower.Unit.GetComponent<ChildUnits>();
                childUnits.RallyPoint.transform.position = position;
                childUnits.SetRallyPointOfAllUnits();
            }
        }
        else
        {
            _selectResult.CursorType = Types.CursorType.None;
            _selectResult.SelectResultType = Types.SelectResultType.Deselect;
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
