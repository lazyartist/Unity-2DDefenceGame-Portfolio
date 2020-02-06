using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector_Tower : Selector {
    public Tower Tower;

	override protected void Start () {
        Tower = GetComponent<Tower>();
        base.Start();
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

    override protected void UpdateSelected()
    {
        if(Tower.Unit == null)
        {
            SelectSR.enabled = false;
        }
        else
        {
            SelectSR.transform.position = Tower.Unit.transform.position + Tower.Unit.UnitCenterOffset;
            SelectSR.transform.localScale = new Vector3(Tower.Unit.UnitData.TargetRange * 2f, Tower.Unit.UnitData.TargetRange * 2f, 1);
            SelectSR.enabled = Selected;
        }
    }
}
