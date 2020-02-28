﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Types {
    public delegate void UnitEvent(UnitEventType unitEventType, Unit unit);
    public delegate void TowerEvent(TowerEventType towerEventType, Tower tower);
    public delegate void InputEvent(InputEventType inputEventType, Vector3 value);
    public delegate void MasterSkillEvent(MasterSkillEventType masterSkillEventType);
    public delegate void StageEvent(StageEventType stageEventType);

    public enum UnitEventType
    {
        None, AttackStart, AttackEnd, AttackFire, Die, DiedComplete, AttackStopped
    }

    public enum TowerEventType
    {
        None, Created, Sold,
    }

    public enum InputEventType
    {
        None, Down, Up, Swipe, Zoom
    }

    public enum MasterSkillEventType
    {
        None, Selected, Unselected
    }

    public enum StageEventType
    {
        None, StageInfoChanged, HeroUnitChanged
    }

    public enum TeamType
    {
        None, A, B
    }

    public enum UnitType
    {
        None, Ground, Air, Tower
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
        None, Wait, BeAttackState, ClearEnemyUnit
    }

    public enum CursorType
    {
        None, Success, Fail
    }
    public enum SelectResultType
    {
        None, // 선택관련 아무런 작업을 하지 않음
        Register, // 선택
        Unregister, // 선택 해제
    }
    public struct SelectResult
    {
        public SelectResultType SelectResultType;
        public CursorType CursorType;
        public bool IsFlag; // 커서에 깃발 표시 여부
        public bool IsSpread; // 다음 Selector에게 이벤트 전파할지 여부

        static public SelectResult Create() {
            SelectResult selectResult;
            selectResult.SelectResultType = SelectResultType.None;
            selectResult.CursorType = CursorType.None;
            selectResult.IsFlag = false;
            selectResult.IsSpread = true;
            return selectResult;
        }
    }

}
