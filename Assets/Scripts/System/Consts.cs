﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Consts
{
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

    public static string[] audios = { "Hit", "aa" };

    private static int _unitNumber = 0;
    public static int GetUnitNumber()
    {
        return ++_unitNumber;
    }
}
