using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TradeCupcakeTowers_Upgrading : TradeCupcakeTowers
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (currentActiveTower != null && currentActiveTower.upgradingCost <= PlayerManager.Inst.Sugar)
        {
            return;
        }

        PlayerManager.Inst.ChangeSugar(-currentActiveTower.upgradingCost);

        currentActiveTower.Upgrade();
    }
}
