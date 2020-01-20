using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TradeCupcakeTowers_Selling : TradeCupcakeTowers
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (currentActiveTower == null) return;

        PlayerManager.Inst.ChangeSugar(currentActiveTower.sellingValue);

        Destroy(currentActiveTower);
    }
}
