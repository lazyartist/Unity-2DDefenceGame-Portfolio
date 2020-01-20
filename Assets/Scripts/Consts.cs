using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Consts {
    public enum TeamType
    {
        A, B
    }

    static public TeamType PTeamType = TeamType.A;
    static public TeamType ETeamType = TeamType.B;

    public enum SkillType
    {
        None, Stun, Slow
    }

    public enum MasterSkillType
    {
        None, Fire, Rain

    }

    public enum AttackRangeType
    {
        None, Box, Circle
    }

    // ===== tag names
    static public string tUnit = "Unit";
    static public string tTower = "Tower";

    // ===== physics layer
    static public int lmUnit = LayerMask.GetMask("Unit");
    static public int lmUnitBody = LayerMask.GetMask("UnitBody");
    static public int lmUnitAttackArea = LayerMask.GetMask("UnitAttackArea");


}
