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
    public TowerMenuButton RallyPointMenuButton;

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
                SelectorManager.Inst.UnregisterSelector();
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
        RallyPointMenuButton.Button.onClick.AddListener(() =>
        {
            Tower.SetRallyPointMode(true);
            Hide();
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
                    SelectorManager.Inst.UnregisterSelector();
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
        _checkedTowerMenuButton = null;
        SellTowerMenuButton.Check(false);

        Tower = tower;
        Tower.SetRallyPointMode(false);

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
                towerMenuButton.Text.text = towerUpgradeData.GoldCost.ToString();
                towerMenuButton.Check(false);
            }
        }

        if (tower.Unit != null)
        {
            // Sell button
            _sellCost = Mathf.RoundToInt(tower.TowerUpgradeData.GoldCost * Consts.TowerUnitSellCostRate);
            SellTowerMenuButton.Text.text = _sellCost.ToString();
            SellTowerMenuButton.Check(false);
            SellTowerMenuButton.gameObject.SetActive(true);

            // RallyPoint button
            ChildUnits childUnits = tower.Unit.GetComponent<ChildUnits>();
            if (childUnits != null)
            {
                RallyPointMenuButton.Check(false);
                RallyPointMenuButton.gameObject.SetActive(true);
            }
            else
            {
                RallyPointMenuButton.gameObject.SetActive(false);
            }
        }
        else
        {
            SellTowerMenuButton.gameObject.SetActive(false);
            RallyPointMenuButton.gameObject.SetActive(false);
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
