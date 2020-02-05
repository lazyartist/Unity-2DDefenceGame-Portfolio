using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector_Tower : Selector {
    public Tower Tower;

	override protected void Start () {
        base.Start();
        Tower = GetComponent<Tower>();
    }
	
    override public bool Select()
    {
        bool result = base.Select();
        Tower.Select();

        return result;
    }

    override public void Deselect()
    {
        Tower.Deselect();
        base.Deselect();
    }
}
