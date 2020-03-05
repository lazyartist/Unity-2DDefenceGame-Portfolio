using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tower : MonoBehaviour
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

        ShowRange(DraftUnitRangeSR, UnitContainer.transform.position, towerUpgradeData.UnitPrefab);
    }

    public void CreateUnit(int index)
    {
        DeleteUnit();

        TowerUpgradeData towerUpgradeData = TowerUpgradeData.TowerUpgradeDatas[index];
        Unit = Instantiate(towerUpgradeData.UnitPrefab, UnitContainer.transform);
        //Rigidbody2D rigidbody2D = Unit.GetComponent<Rigidbody2D>();
        //if(rigidbody2D != null)
        //{
        //    rigidbody2D.simulated = false;
        //}
        Unit.transform.localPosition = Vector3.zero;
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
            if(_childUnitCreator != null)
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
            ShowRange(UnitRangeSR, Unit.transform.position, Unit);
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

    public void ShowRange(SpriteRenderer rangeSR, Vector3 unitPosition, Unit unit)
    {
        rangeSR.transform.position = unitPosition + unit.UnitCenterOffset;
        rangeSR.transform.localScale = new Vector3(unit.UnitData.TargetRange * 2f, unit.UnitData.TargetRange * 2f, 1);
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
            ShowRange(UnitRangeSR, Unit.transform.position, Unit);
        }
    }

    void DispatchEvent(Types.TowerEventType towerEventType)
    {
        if(TowerEvent != null)
        {
            TowerEvent(towerEventType, this);
        }
    }
}
