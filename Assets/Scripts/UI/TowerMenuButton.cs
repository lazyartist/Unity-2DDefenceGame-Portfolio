using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenuButton : MonoBehaviour
{
    public Button Button;
    public Text Text;
    public Image IconImage;
    public Image CheckedImage;
    public TowerUpgradeData TowerUpgradeData;
    public Material GrayscaleMaterial;

    public bool IsChecked;
    public void Check(bool check)
    {
        IsChecked = check;
        IconImage.enabled = !IsChecked;
        CheckedImage.enabled = IsChecked;
    }

    bool _canBuy = true;
    public bool CanBuy
    {
        get { return _canBuy; }
        set
        {
            _canBuy = value;
            Material material = _canBuy ? null : GrayscaleMaterial;
            Button.image.material = material;
            IconImage.material = material;
            CheckedImage.material = material;
        }
    }
}
