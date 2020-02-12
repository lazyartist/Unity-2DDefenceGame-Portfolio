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

    override public Types.SelectResult Select()
    {
        base.Select();
        //Unit.Select();
        _selectResult.CursorType = Types.CursorType.None;
        _selectResult.IsFlag = false;
        _selectResult.SelectResultType = Types.SelectResultType.Select;
        return _selectResult;
    }

    override public Types.SelectResult SelectNext(Selector selector, Vector3 position, bool isOnWay)
    {
        _selectResult.CursorType = Types.CursorType.Success;
        _selectResult.IsFlag = true;
        _selectResult.SelectResultType = Types.SelectResultType.Deselect;
        return _selectResult;
    }

    override public void Deselect()
    {
        base.Deselect();
    }
}
