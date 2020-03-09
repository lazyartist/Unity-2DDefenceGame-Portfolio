using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Types {
    public delegate void UnitEvent(UnitEventType unitEventType, Unit unit);
    public delegate void TowerEvent(TowerEventType towerEventType, TowerBuilder tower);
    public delegate void InputEvent(InputEventType inputEventType, Vector3 value);
    public delegate void MasterSkillEvent(MasterSkillEventType masterSkillEventType);
    public delegate void StageEvent(StageEventType stageEventType);
    public delegate void CameraEvent(CameraEventType cameraEventType);

    public enum UnitEventType
    {
        None, Live, AttackStart, AttackEnd, AttackFire, Die, DiedComplete, AttackStopped
    }

    public enum TowerEventType
    {
        None, Created, Sold,
    }

    public enum InputEventType
    {
        None, Down, DownCanceled, Up, Swipe, Zoom, ZoomByTouch
    }

    public enum MasterSkillEventType
    {
        None, Selected, Unselected
    }

    public enum StageEventType
    {
        None, PlayerInfoChanged, WaveInfoChanged, HeroUnitChanged
    }

    public enum CameraEventType
    {
        None, CameraSizeChanged,
    }

    public enum TeamType
    {
        None, A, B
    }

    public enum UnitPlaceType
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

    public enum AudioType
    {
        None, Battle, Specify
    }

    public enum AudioChannelType
    {
        None, Bgm, Effect
    }

    public enum UnitSortingLayerType
    {
        Unit_Ground, Unit_Air
    }

    public enum PathFindResultType
    {
        Success, Fail, EqualStartAndEnd, TooShort
    }

    public enum MapMaskChannelType
    {
        Block, Way, b, a
    }

    [System.Serializable]
    public struct IntTuple2
    {
        public int x, y;
    }

    public enum InfoType {
        None, Unit, Tower, MasterSkill, 
    }

    public enum UnitMovePointType {
        RallyPoint, MovePoint, WayPoint
    }

    public enum UnitTargetRangeCenterType
    {
        RallyPoint, UnitCenter
    }
}
