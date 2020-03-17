using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Consts
{
    static Consts()
    {
        Debug.Log("Consts " +Time.time );
        InitUnitLayerMask();
    }

    // ===== float
    public static float TowerUnitSellCostRate = 0.8f;
    public static int WaypointSubIndexStart = 1;
    public static float CreateUnitInterval { get { return 0.5f; } internal set { } }
    public static float CoolTimeUpdateInterval { get { return 0.05f; } internal set { } }
    public static float UnitRenderOrderPrecision = 100f;
    public static float MoveArrivedDistance = 0.01f;
    public static float PixelPerUnit = 100f;
    public static float ColorNearlyEqual = 0.5f;

    // ===== tag names
    public static string tagUnit = "Unit";
    public static string tagTower = "Tower";

    // === map mask
    public static Color MapMaskColor_Way = Color.green;
    public static Color MapMaskColor_Block = Color.red;

    // === unit layer mask
    static Dictionary<Types.TeamType, List<LayerMask>> _unitLayerMasks;
    public static LayerMask GetUnitLayerMask(Types.TeamType teamType, Types.UnitPlaceType unitPlaceType)
    {
        return _unitLayerMasks[teamType][(int)unitPlaceType];
    }
    static void InitUnitLayerMask()
    {
        _unitLayerMasks = new Dictionary<Types.TeamType, List<LayerMask>>();
        for (int i = 0; i < (int)Types.TeamType.Count; i++)
        {
            Types.TeamType teamType = (Types.TeamType)Enum.Parse(typeof(Types.TeamType), i.ToString());
            string teamTypeAsString = teamType.ToString();
            List<LayerMask> layerMasks = new List<LayerMask>();
            for (int ii = 0; ii < (int)Types.UnitPlaceType.Count; ii++)
            {
                string maskName = teamTypeAsString + Enum.Parse(typeof(Types.UnitPlaceType), ii.ToString()).ToString();
                int mask = LayerMask.GetMask(maskName);
                layerMasks.Add(mask);
            }
            _unitLayerMasks.Add(teamType, layerMasks);
        }
    }

    // === Unit Number
    private static int _unitNumber = 0;
    public static int GetUnitNumber()
    {
        return ++_unitNumber;
    }
}
