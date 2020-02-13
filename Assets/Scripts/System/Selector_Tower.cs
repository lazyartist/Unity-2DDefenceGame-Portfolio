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
        if (Tower.IsRallyPointModeOn)
        {
            // barracks rallypoint
            _selectResult.CursorType = isOnWay ? Types.CursorType.Success : Types.CursorType.Fail;
            _selectResult.IsFlag = true;
            _selectResult.SelectResultType = Types.SelectResultType.Deselect;

            ChildUnits childUnits = Tower.Unit.GetComponent<ChildUnits>();
            childUnits.RallyPoint.transform.position = position;
            childUnits.SetRallyPointOfAllUnits();
        }
        else
        {
            _selectResult.CursorType = Types.CursorType.None;
            _selectResult.IsFlag = false;
            _selectResult.SelectResultType = Types.SelectResultType.Deselect;
        }

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
    }
}
