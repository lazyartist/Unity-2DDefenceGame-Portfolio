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

    override public bool Select()
    {
        bool result = base.Select();
        return result;
    }

    override public void Deselect()
    {
        base.Deselect();
    }
}
