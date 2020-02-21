using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAndStageBinder : MonoBehaviour {
    public Tower Tower;

	void Start () {
        Tower = GetComponent<Tower>();
        if(Tower != null)
        {
            Tower.TowerEvent += _OnTowerEvent;
        }
    }

    private void OnApplicationQuit()
    {
        CleanUp();
    }

    private void OnDestroy()
    {
        CleanUp();
    }

    void CleanUp()
    {
        if(Tower != null)
        {
            Tower.TowerEvent -= _OnTowerEvent;
            Tower = null;
        }
    }

    void _OnTowerEvent(Types.TowerEventType towerEventType, Tower tower)
    {
        switch (towerEventType)
        {
            case Types.TowerEventType.None:
                break;
            case Types.TowerEventType.Created:
                StageManager.Inst.StageInfo.Gold -= tower.TowerUpgradeData.GoldCost;
                break;
            case Types.TowerEventType.Sold:
                StageManager.Inst.StageInfo.Gold += tower.TowerUpgradeData.GoldCost;
                break;
            default:
                break;
        }
    }
}
