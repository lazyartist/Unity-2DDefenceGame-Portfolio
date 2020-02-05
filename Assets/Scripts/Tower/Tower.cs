using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour {
    public Button StubButton;
    public SpriteRenderer SelectionSR;
    public GameObject UnitContainer;

    //public TeamData TeamData;
    //public UnitData UnitData;
    //public AttackTargetData AttackTargetData;
    //public AttackData AttackData;

    private void Start()
    {
        SelectionSR.enabled = false;
        SelectSystem.Inst.SelectionEvent += _OnSelectionEvent;
    }

    private void OnApplicationQuit()
    {
        CleanUpUnit();
    }

    private void OnDestroy()
    {
        CleanUpUnit();
    }

    private void CleanUpUnit()
    {
        SelectSystem.Inst.SelectionEvent -= _OnSelectionEvent;
    }

    private void _OnSelectionEvent(Types.SelectionEventType selectionEventType, GameObject gameObject)
    {
        Select();
    }

    public void OnClick_StubButton()
    {
        SelectSystem.Inst.Notify(gameObject);
    }

    public bool Select()
    {
        SelectionSR.enabled = true;
        return true;
    }

    public bool Unselect()
    {
        SelectionSR.enabled = false;
        return true;
    }
}
