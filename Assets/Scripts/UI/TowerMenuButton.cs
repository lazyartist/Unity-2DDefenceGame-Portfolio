using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenuButton : MonoBehaviour {
    public Button Button;
    public Text Text;
    public Image IconImage;
    public Image CheckedImage;
    public TowerUpgradeData TowerUpgradeData;

    public bool IsChecked;
    public void Check(bool check)
    {
        IsChecked = check;
        IconImage.enabled = !IsChecked;
        CheckedImage.enabled = IsChecked;
    }
}
