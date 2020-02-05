using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {
    public bool Selected = false;
    public SpriteRenderer SelectSR;

    // Use this for initialization
    virtual protected void Start()
    {
        Selected = false;
        UpdateSelected();
    }

    // 이 Selector가 관리하는 유닛을 선택한다.
    // return 선택 성공 여부
    virtual public bool Select()
    {
        Selected = true;
        UpdateSelected();
        return true;
    }

    // 이 Selector가 관리하는 유닛의 선택을 해제한다.
    virtual public void Deselect()
    {
        Selected = false;
        UpdateSelected();
    }

    private void UpdateSelected()
    {
        SelectSR.enabled = Selected;
    }
}
