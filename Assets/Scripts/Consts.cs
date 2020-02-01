using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Consts {
    // ===== tag names
    static public string tagUnit = "Unit";
    static public string tagTower = "Tower";

    // ===== physics layers
    static public int lmUnit = LayerMask.GetMask("Unit");
    static public int lmUnitBody = LayerMask.GetMask("UnitBody");
    static public int lmUnitAttackArea = LayerMask.GetMask("UnitAttackArea");
}
