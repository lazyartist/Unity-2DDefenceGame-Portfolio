﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Consts {
    // ===== float
    static public float ArriveDistance = 0.01f;

    // ===== tag names
    static public string tagUnit = "Unit";
    static public string tagTower = "Tower";

    // ===== physics layers
    static public int lmUnit = LayerMask.GetMask("Unit");
    //static public int lmATeam = LayerMask.GetMask("ATeam");
    //static public int lmBTeam = LayerMask.GetMask("BTeam");
}