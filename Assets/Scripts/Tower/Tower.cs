using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour {
    public GameObject UnitContainer;
    public Unit Unit;
    public GameObject MenuButtonsContainer;
    public Button[] MenuButtons;
    public Unit[] UnitPrefabs;

    private void Start()
    {
        MenuButtonsContainer.SetActive(false);
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
    }

    public void Select()
    {
        MenuButtonsContainer.SetActive(true);
    }

    public void Deselect()
    {
        MenuButtonsContainer.SetActive(false);
    }

    public void OnClick_MenuButton(int index)
    {
        Debug.Log(index);
        if(index == 0)
        {
            Unit = Instantiate(UnitPrefabs[index], UnitContainer.transform);
            Unit.transform.localPosition = Vector3.zero;
            Unit.gameObject.SetActive(true);
        }
    }
}
