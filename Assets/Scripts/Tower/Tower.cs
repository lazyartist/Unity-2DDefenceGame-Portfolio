using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public TowerUpgradeData TowerUpgradeData;
    public Sprite UpgradeIcon;
    public Sprite CheckedIcon;

    public GameObject UnitContainer;
    public Unit Unit;
    public GameObject MenuButtonsContainer;
    public TowerMenuButton TowerMenuButtonPrefab;

    //public Button[] MenuButtons;
    //public Unit[] UnitPrefabs;

    private void Start()
    {
        MenuButtonsContainer.SetActive(false);

        int length = TowerUpgradeData.TowerUpgradeDatas.Length;
        if (length == 0) return;

        float startAngle = 90f;
        switch (length)
        {
            case 1:
                startAngle = 90f;
                break;
            case 2:
                startAngle = 0f;
                break;
            case 3:
                startAngle = 360f / 3f;
                break;
            case 4:
                startAngle = 360f / 4f;
                break;
            default:
                break;
        }
        for (int i = 0; i < length; i++)
        {
            TowerUpgradeData towerUpgradeData = TowerUpgradeData.TowerUpgradeDatas[i];
            Vector3 localPosition = Quaternion.Euler(0f, 0f, (startAngle + 360f / length) * i) * Vector3.right;
            TowerMenuButton towerMenuButton = Instantiate(TowerMenuButtonPrefab, localPosition, Quaternion.identity, MenuButtonsContainer.transform);
            towerMenuButton.TowerUpgradeData = towerUpgradeData;
            towerMenuButton.name = "towerMenuButton" + i;
            towerMenuButton.Button.onClick.AddListener(() => OnClick_MenuButton(towerMenuButton));
            towerMenuButton.GetComponent<DataProvider>().Data = towerUpgradeData;
            towerMenuButton.IconImage.sprite = towerUpgradeData.IconSprite;
            towerMenuButton.Text.text = towerUpgradeData.Cost.ToString();
        }

        //UnityAction a = new UnityAction();

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
        //TowerMenuButton.

        MenuButtonsContainer.SetActive(true);
    }

    public void Deselect()
    {
        MenuButtonsContainer.SetActive(false);
    }

    public void OnClick_MenuButton(TowerMenuButton _towerMenuButton)
    {
        Debug.Log(_towerMenuButton);
        Unit = Instantiate(_towerMenuButton.TowerUpgradeData.UnitPrefab, UnitContainer.transform);
        Unit.GetComponent<Rigidbody2D>().simulated = false;
        Unit.transform.localPosition = Vector3.zero;
        Unit.gameObject.SetActive(true);
    }
}
