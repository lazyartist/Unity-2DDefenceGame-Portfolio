using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Consts {
    public delegate void UnitEventListener(UnitEventType unitEventType);

    public enum UnitEventType
    {
        None, Attack, DiedComplete
    }

    public enum TeamType
    {
        A, B
    }

    public enum CCType // Crowd Control
    {
        None, Stun, Slow
    }

    public enum MasterSkillType
    {
        None, Fire, Rain
    }

    public enum AttackRangeType
    {
        None, Target, Box, Circle
    }

    public enum UnitFSMType
    {
        Idle, Move, Attack, Wait, Died
    }
    //public static readonly int UnitFSMTypeCount = System.Enum.GetNames(typeof(UnitFSMType)).Length;

    [System.Serializable]
    public struct AttackArea_
    {
        public Vector3 offset;
        public Vector3 size;
    }

    // ===== tag names
    static public string tUnit = "Unit";
    static public string tTower = "Tower";

    // ===== physics layers
    static public int lmUnit = LayerMask.GetMask("Unit");
    static public int lmUnitBody = LayerMask.GetMask("UnitBody");
    static public int lmUnitAttackArea = LayerMask.GetMask("UnitAttackArea");


}
