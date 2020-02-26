using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Consts
{
    // ===== float
    public static float ArriveDistance = 0.01f;
    public static float TowerUnitSellCostRate = 0.8f;
    public static int WaypointSubIndexStart = 1;
    public static float CreateUnitInterval { get { return 0.5f; } internal set { } }
    public static float CoolTimeUpdateInterval { get { return 0.05f; } internal set { } }

    // ===== tag names
    public static string tagUnit = "Unit";
    public static string tagTower = "Tower";

    // todo remove
    // ===== physics layers
    public static int lmUnit = LayerMask.GetMask("Unit");

    private static int _unitId = 0;
    public static int GetUnitId()
    {
        return ++_unitId;
    }
}
