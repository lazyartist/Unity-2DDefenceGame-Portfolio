using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {
    public bool Selected = false;
    public SpriteRenderer SelectedSR;
    protected Types.SelectResult _selectResult;

    virtual protected void Start()
    {
        Selected = false;
        UpdateSelected();
    }

    // 선택
    virtual public Types.SelectResult Select()
    {
        Selected = true;
        UpdateSelected();
        return _selectResult;
    }

    // 선택 이후 클릭에 대한 처리
    virtual public Types.SelectResult SelectNext(Selector selector, Vector3 position, bool isOnWay)
    {
        return _selectResult;
    }

    // 선택 해제
    virtual public void Deselect()
    {
        Selected = false;
        UpdateSelected();
    }

    virtual protected void UpdateSelected()
    {
        SelectedSR.enabled = Selected;
    }
}
