using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public TowerUpgradeData TowerUpgradeData;
    public TowerUpgradeData RootTowerUpgradeData;
    public GameObject UnitContainer;
    public GameObject UIPosition;
    public Unit Unit;
    public SpriteRenderer UnitSR;

    private void Start()
    {
        ClearUnit();
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

    public void ShowDraftUnit(int index)
    {
        TowerUpgradeData towerUpgradeData = TowerUpgradeData.TowerUpgradeDatas[index];
        UnitSR.enabled = true;
        UnitSR.sprite = towerUpgradeData.UnitSprite;
    }

    public void CreateUnit(int index)
    {
        ClearUnit();

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
    }

    public void ClearUnit()
    {
        // todo 타워의 종류에 따라 정리작업을 다르게 해줌

        if (Unit != null)
        {
            Destroy(Unit.gameObject);
        }
        Unit = null;
        TowerUpgradeData = RootTowerUpgradeData;
        UnitSR.enabled = false;
    }

    public void Select()
    {
        UICanvas.Inst.ShowTowerMenu(this, true);
    }

    public void Deselect()
    {
        UICanvas.Inst.ShowTowerMenu(this, false);
    }


}
