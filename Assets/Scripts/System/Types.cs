using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Types {
    public delegate void UnitEvent(UnitEventType unitEventType, Unit unit);
    public delegate void SelectionEvent(SelectionEventType selectionEventType, GameObject gameObject);

    public enum UnitEventType
    {
        None, AttackStart, AttackEnd, Attack, Die, DiedComplete
    }

    public enum SelectionEventType
    {
        None, Selected
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

    public enum UnitFSMType
    {
        Idle, Move, Attack, Wait, Died, Hurt
    }

    public enum UnitNotifyType
    {
        None, Wait, Attack
    }

    [System.Serializable]
    public struct AttackArea_
    {
        public Vector3 offset;
        public Vector3 size;
    }
}
