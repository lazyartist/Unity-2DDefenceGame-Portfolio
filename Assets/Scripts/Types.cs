using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Types {
    public delegate void UnitBodyEvent(UnitBodyEventType unitEventType);

    public enum UnitBodyEventType
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

    //public enum AttackRangeType
    //{
    //    None, Target, Box, Circle
    //}

    public enum UnitFSMType
    {
        Idle, Move, Attack, Wait, Died, Hurt
    }
    //public static readonly int UnitFSMTypeCount = System.Enum.GetNames(typeof(UnitFSMType)).Length;

    public enum UnitNotifyType
    {
        None, Wait, Attack, AttackTargetUnitDied
    }

    [System.Serializable]
    public struct AttackArea_
    {
        public Vector3 offset;
        public Vector3 size;
    }
}
