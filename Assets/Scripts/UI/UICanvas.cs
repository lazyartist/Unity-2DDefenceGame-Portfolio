using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : SingletonBase<UICanvas> {
    public UITowerMenu UITowerMenu;
    public Text UnitInfoText;

    public void ShowTowerMenu(Tower tower, bool isShow) {
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
}
