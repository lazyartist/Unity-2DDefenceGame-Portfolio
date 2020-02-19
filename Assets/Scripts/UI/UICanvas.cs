using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : SingletonBase<UICanvas>
{
    public UITowerMenu UITowerMenu;
    public Text UnitInfoText;

    private void Start()
    {
        InputManager.Inst.InputEvent += _OnInputEvent_InputManager;
    }

    private void OnApplicationQuit()
    {
        _ClearnUp();
    }

    private void OnDestroy()
    {
        _ClearnUp();
    }

    void _ClearnUp()
    {
        InputManager.Inst.InputEvent -= _OnInputEvent_InputManager;
    }

    public void ShowTowerMenu(Tower tower, bool isShow)
    {
        if (isShow)
        {
            UITowerMenu.Show(tower);
        }
        else
        {
            UITowerMenu.Hide();
        }
    }

    public void ShowUnitInfo(Unit unit)
    {
        UnitInfoText.text = unit.ToString();
    }
    public void HideUnitInfo()
    {
        UnitInfoText.text = "";
    }

    void _OnInputEvent_InputManager(Types.InputEventType inputEventType, Vector3 value)
    {
        switch (inputEventType)
        {
            case Types.InputEventType.None:
                break;
            case Types.InputEventType.Down:
                break;
            case Types.InputEventType.Up:
                break;
            case Types.InputEventType.Swipe:
                _UpdatePosition();
                break;
            case Types.InputEventType.Zoom:
                _HideUI();
                break;
            default:
                break;
        }
    }

    void _UpdatePosition()
    {
        if (UITowerMenu.gameObject.activeSelf && UITowerMenu.Tower != null)
        {
            UITowerMenu.Show(UITowerMenu.Tower);
        }
    }

    void _HideUI()
    {
        if (UITowerMenu.gameObject.activeSelf)
        {
            UITowerMenu.Hide();
        }
    }
}
