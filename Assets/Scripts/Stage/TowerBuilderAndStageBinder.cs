using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilderAndStageBinder : MonoBehaviour {
    public TowerBuilder TowerBuilder;

	void Start () {
        TowerBuilder = GetComponent<TowerBuilder>();
        if(TowerBuilder != null)
        {
            TowerBuilder.TowerEvent += OnTowerEvent;
        }
    }

    void OnApplicationQuit()
    {
        CleanUp();
    }

    void OnDestroy()
    {
        CleanUp();
    }

    void CleanUp()
    {
        if(TowerBuilder != null)
        {
            TowerBuilder.TowerEvent -= OnTowerEvent;
            TowerBuilder = null;
        }
    }

    void OnTowerEvent(Types.TowerEventType towerEventType, TowerBuilder tower)
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
