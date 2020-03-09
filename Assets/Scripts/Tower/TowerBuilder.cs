using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TowerBuilder : MonoBehaviour
{
    public Types.TowerEvent TowerEvent;

    public TowerUpgradeData TowerUpgradeData;
    public TowerUpgradeData RootTowerUpgradeData;
    public GameObject UnitContainer;
    public GameObject UIPosition;
    public Unit Unit;
    public SpriteRenderer UnitThumbSR;
    public SpriteRenderer UnitRangeSR;
    public SpriteRenderer DraftUnitRangeSR;
    public bool IsRallyPointModeOn;

    void Start()
    {
        TowerUpgradeData = RootTowerUpgradeData;
        Deselect();
    }

    void OnApplicationQuit()
    {
        DeleteUnit();
    }

    void OnDestroy()
    {
        DeleteUnit();
    }

    void CleanUp()
    {
        DeleteUnit();
        TowerUpgradeData = null;
        RootTowerUpgradeData = null;
    }

    public void ShowDraftUnit(int index)
    {
        TowerUpgradeData towerUpgradeData = TowerUpgradeData.TowerUpgradeDatas[index];
        UnitThumbSR.enabled = true;
        UnitThumbSR.sprite = towerUpgradeData.UnitSprite;

        // 현재 유닛이 생성되지 않은 상태이기 때문에 유닛의 UnitCenter 좌표를 얻을 수 없다.
        // 따라서 UnitCenter의 로컬좌표를 UnitContainer의 좌표에 더해서 rangeCenterPosition을 구함.
        Vector3 rangeCenterPosition = UnitContainer.transform.position + (towerUpgradeData.UnitPrefab.UnitCenter.transform.position - towerUpgradeData.UnitPrefab.transform.position);
        ShowRange(DraftUnitRangeSR, rangeCenterPosition, towerUpgradeData.UnitPrefab.UnitData);
    }

    public void CreateUnit(int index)
    {
        DeleteUnit();

        TowerUpgradeData towerUpgradeData = TowerUpgradeData.TowerUpgradeDatas[index];
        Unit = Instantiate(towerUpgradeData.UnitPrefab, UnitContainer.transform);
        Unit.transform.localPosition = Vector3.zero;
        Unit.UnitMovePoint.RallyPoint = Unit.transform.position;
        Unit.gameObject.SetActive(true);

        TowerUpgradeData = towerUpgradeData;
        DispatchEvent(Types.TowerEventType.Created);
    }

    public void SellUnit()
    {
        DeleteUnit();
        DispatchEvent(Types.TowerEventType.Sold);
        TowerUpgradeData = RootTowerUpgradeData;
    }
    public void DeleteUnit()
    {
        // todo 타워의 종류에 따라 정리작업을 다르게 해줌
        if (Unit != null)
        {
            ChildUnitCreator _childUnitCreator = Unit.gameObject.GetComponent<ChildUnitCreator>();
            if (_childUnitCreator != null)
            {
                _childUnitCreator.ClearAllEnemyUnits();
            }

            Destroy(Unit.gameObject);
        }
        Unit = null;
    }

    public void Select()
    {
        UICanvas.Inst.ShowTowerMenu(this, true);
        if (Unit != null)
        {
            ShowRange(UnitRangeSR, Unit.UnitCenter.transform.position, Unit.UnitData);
        }
    }

    public void Deselect()
    {
        //HideRange();
        UnitThumbSR.enabled = false;
        UnitRangeSR.enabled = false;
        DraftUnitRangeSR.enabled = false;
        IsRallyPointModeOn = false;
        UICanvas.Inst.ShowTowerMenu(this, false);
    }

    public void ShowRange(SpriteRenderer rangeSR, Vector3 position, UnitData unitData)
    {
        rangeSR.transform.position = position;
        rangeSR.transform.localScale = new Vector3(unitData.ShortTargetRange * 2f, unitData.ShortTargetRange * 2f, 1);
        rangeSR.enabled = true;
    }

    //public void HideRange()
    //{
    //}

    public void SetRallyPointMode(bool isOn)
    {
        IsRallyPointModeOn = isOn;
        UnitRangeSR.enabled = isOn;
        if (isOn == true && Unit != null)
        {
            ShowRange(UnitRangeSR, Unit.UnitCenter.transform.position, Unit.UnitData);
        }
    }

    void DispatchEvent(Types.TowerEventType towerEventType)
    {
        if (TowerEvent != null)
        {
            TowerEvent(towerEventType, this);
        }
    }
}
