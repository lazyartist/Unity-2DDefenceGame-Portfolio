using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : SingletonBase<UICanvas> {
    public UITowerMenu UITowerMenu;

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
}
