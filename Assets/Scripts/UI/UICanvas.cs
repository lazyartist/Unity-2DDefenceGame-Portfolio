using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : SingletonBase<UICanvas>
{
    public UITowerMenu UITowerMenu;
    public Text UnitInfoText;
    public HeroUnitButton HeroUnitButton;

    void Start()
    {
        InputManager.Inst.InputEvent += OnInputEvent_InputManager;
    }

    void OnInputEvent_InputManager(Types.InputEventType inputEventType, Vector3 value)
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
                UpdatePosition();
                break;
            case Types.InputEventType.Zoom:
                HideTowerMenu();
                break;
            default:
                break;
        }
    }

    void UpdatePosition()
    {
        if (UITowerMenu.gameObject.activeSelf && UITowerMenu.Tower != null)
        {
            UITowerMenu.Show(UITowerMenu.Tower);
        }
    }

    public void ShowTowerMenu(TowerBuilder tower, bool isShow)
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

    void HideTowerMenu()
    {
        if (UITowerMenu.gameObject.activeSelf)
        {
            UITowerMenu.Hide();
        }
    }
}
