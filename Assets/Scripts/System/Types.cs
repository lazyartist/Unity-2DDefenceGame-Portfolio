using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Types {
    public delegate void UnitEvent(UnitEventType unitEventType, Unit unit);
    public delegate void SelectionEvent(SelectionEventType selectionEventType, GameObject gameObject);
    public delegate void InputEvent(InputEventType inputEventType, Vector3 value);

    public enum UnitEventType
    {
        None, AttackStart, AttackEnd, Attack, Die, DiedComplete, AttackStopped
    }

    public enum SelectionEventType
    {
        None, Selected
    }

    public enum InputEventType
    {
        None, Down, Up, Swipe, Zoom
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
        None, Wait, BeAttackState, ClearAttackTarget
    }

    [System.Serializable]
    public struct AttackArea_
    {
        public Vector3 offset;
        public Vector3 size;
    }

    public enum CursorType
    {
        None, Success, Fail
    }
    public enum SelectResultType
    {
        None, // 선택관련 아무런 작업을 하지 않음
        Select, // 선택
        Deselect, // 선택 해제
    }
    //[System.Serializable]
    public struct SelectResult
    {
        public SelectResultType SelectResultType;
        public CursorType CursorType;
        public bool IsFlag;

    //    public SelectResult(SelectPropagationType selectPropagationType = SelectPropagationType.Done, CursorType cursorType = CursorType.None, bool isFlag = false)
    //    {
    //        SelectPropagationType = SelectPropagationType.Done;
    //        CursorType = CursorType.None;
    //        IsFlag = false;
    //}
    }

}
