using System.Collections;
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
    private int _sellCost;

    private void Awake()
    {
        this.gameObject.SetActive(false);
        Init();
    }
    void Start()
    {
    }

    void Update()
    {
    }

    void Init()
    {
        SellTowerMenuButton.Button.onClick.AddListener(() =>
        {
            if (SellTowerMenuButton.IsChecked)
            {
                Tower.DeleteUnit();
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
                    Debug.Log("CreateUnit " + index);
                    Tower.CreateUnit(index);
                    SelectSystem.Inst.Deselect();
                }
                else
                {
                    if (_checkedTowerMenuButton != null)
                    {
                        _checkedTowerMenuButton.Check(false);
                    }
                    towerMenuButton.Check(true);
                    _checkedTowerMenuButton = towerMenuButton;

                    Debug.Log("DraftUnit " + index);
                    Tower.ShowDraftUnit(index);
                }
            });
            towerMenuButton.name = "towerMenuButton" + i;
            TowerMenuButtons[i] = towerMenuButton;
        }
    }

    public void Show(Tower tower)
    {
        gameObject.SetActive(true);
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

        if (tower.Unit != null)
        {
            // sell
            _sellCost = Mathf.RoundToInt(tower.TowerUpgradeData.Cost * Consts.TowerUnitSellCostRate);
            SellTowerMenuButton.Text.text = _sellCost.ToString();
            SellTowerMenuButton.gameObject.SetActive(true);
        }
        else
        {
            SellTowerMenuButton.gameObject.SetActive(false);
        }

        Vector3 uiPosition = Camera.main.WorldToScreenPoint(Tower.UIPosition.transform.position);
        transform.position = new Vector3(uiPosition.x, uiPosition.y, 0f);
        Debug.Log("UItowerMenu position " + transform.position);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Tower = null;
    }
}
