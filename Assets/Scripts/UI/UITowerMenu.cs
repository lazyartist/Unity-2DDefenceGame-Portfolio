﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITowerMenu : MonoBehaviour
{
    public Sprite UpgradeIcon;
    public Sprite CheckedIcon;
    public int MaxTowerButtonCount;
    public float TowerMenuButtonRadius;
    public TowerMenuButton TowerMenuButtonPrefab;

    public Tower Tower;
    public TowerUpgradeData TowerUpgradeData;
    public TowerMenuButton[] TowerMenuButtons;
    public TowerMenuButton SellTowerMenuButton;

    private TowerMenuButton _checkedTowerMenuButton;

    void Start()
    {
        this.gameObject.SetActive(false);

        SellTowerMenuButton.Button.onClick.AddListener(() =>
        {
            if (SellTowerMenuButton.IsChecked)
            {
                Tower.ClearUnit();
                SelectSystem.Inst.Deselect();
            }
            else
            {
                if (_checkedTowerMenuButton != null)
                {
                    _checkedTowerMenuButton.Check(false);
                }
                SellTowerMenuButton.Check(true);
                _checkedTowerMenuButton = SellTowerMenuButton;
            }
        });

        TowerMenuButtons = new TowerMenuButton[MaxTowerButtonCount];
        for (int i = 0; i < MaxTowerButtonCount; i++)
        {
            int index = i;
            TowerMenuButton towerMenuButton = Instantiate(TowerMenuButtonPrefab, transform);
            towerMenuButton.Button.onClick.AddListener(() =>
            {
                if (towerMenuButton.IsChecked)
                {
                    OnClick_TowerMenuButton(index);
                }
                else
                {
                    if (_checkedTowerMenuButton != null)
                    {
                        _checkedTowerMenuButton.Check(false);
                    }
                    towerMenuButton.Check(true);
                    _checkedTowerMenuButton = towerMenuButton;
                }
            });
            towerMenuButton.name = "towerMenuButton" + i;
            TowerMenuButtons[i] = towerMenuButton;
        }
    }

    void Update()
    {
    }

    public void Show(Tower tower)
    {
        Tower = tower;

        _checkedTowerMenuButton = null;
        SellTowerMenuButton.Check(false);

        int length = tower.TowerUpgradeData.TowerUpgradeDatas.Length;
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
                startAngle = 30f;
                break;
            case 4:
                startAngle = 45f;
                break;
            default:
                break;
        }
        for (int i = 0; i < MaxTowerButtonCount; i++)
        {
            TowerMenuButton towerMenuButton = TowerMenuButtons[i];
            bool active = i < length;
            towerMenuButton.gameObject.SetActive(active);
            if (active)
            {
                TowerUpgradeData towerUpgradeData = tower.TowerUpgradeData.TowerUpgradeDatas[i];
                Vector3 localPosition = Quaternion.Euler(0f, 0f, startAngle + (360f / length * i)) * Vector3.right * TowerMenuButtonRadius;
                towerMenuButton.transform.localPosition = localPosition;
                towerMenuButton.TowerUpgradeData = towerUpgradeData;
                towerMenuButton.GetComponent<DataProvider>().Data = towerUpgradeData;
                towerMenuButton.IconImage.sprite = towerUpgradeData.IconSprite;
                towerMenuButton.Text.text = towerUpgradeData.Cost.ToString();
                towerMenuButton.Check(false);
            }
        }

        transform.position = Camera.main.WorldToScreenPoint(Tower.UIPosition.transform.position);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        Tower = null;
        gameObject.SetActive(false);
    }

    public void OnClick_TowerMenuButton(int index)
    {
        Debug.Log(index);
        Tower.CreateUnit(index);
        SelectSystem.Inst.Deselect();
    }
}