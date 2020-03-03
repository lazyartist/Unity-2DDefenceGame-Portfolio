using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector_HeroUnit : Selector
{
    public Unit Unit;

    override protected void Start()
    {
        base.Start();
        Unit = GetComponent<Unit>();
        Unit.UnitEvent += OnUnitEvent;
    }

    override public Types.SelectResult SelectEnter()
    {
        base.SelectEnter();
        //Unit.Select();
        _selectResult.CursorType = Types.CursorType.None;
        _selectResult.SelectResultType = Types.SelectResultType.Register;
        _selectResult.IsFlag = false;
        _selectResult.IsSpread = true;

        UICanvas.Inst.HeroUnitButton.Select(true);

        return _selectResult;
    }

    override public Types.SelectResult SelectUpdate(Vector3 position, bool isOnWay)
    {
        if (isOnWay)
        {
            Types.PathFindResultType pathFindResultType;
            List<Vector3> path = PathFindingManager.Inst.GetPathOrNull(Unit.transform.position, position, out pathFindResultType);
            Debug.Log("pathFindResultType " + pathFindResultType);
            switch (pathFindResultType)
            {
                case Types.PathFindResultType.Success:
                    {
                        // todo here waypoint 관리 필요
                        if (Unit.RallyWaypoint == null)
                        {
                            Unit.RallyWaypoint = WaypointManager.Inst.WaypointPool.Get();
                        }
                        if (Unit.TargetWaypoint == null)
                        {
                            Unit.TargetWaypoint = WaypointManager.Inst.WaypointPool.Get();
                        }
                        Unit.TargetWaypoint.transform.position = Unit.gameObject.transform.position;

                        Waypoint targetPosition = Unit.TargetWaypoint;
                        for (int i = 0; i < path.Count; i++)
                        {
                            Vector3 pos = path[i];
                            Waypoint nextTargetPosition = WaypointManager.Inst.WaypointPool.Get();
                            nextTargetPosition.transform.position = pos;

                            nextTargetPosition.NextWaypoint = null;
                            targetPosition.NextWaypoint = nextTargetPosition;
                            targetPosition = nextTargetPosition;
                        }
                        // 첫 위치는 현재 위치를 넣는다
                        Unit.TargetWaypoint.NextWaypoint.transform.position = Unit.gameObject.transform.position; ;
                        // 마지막 위치는 타겟 위치를 넣는다
                        targetPosition.transform.position = position;
                        Unit.RallyWaypoint.transform.position = position;
                        Unit.ClearEnemyUnit();
                    }
                    break;
                case Types.PathFindResultType.Fail:
                    break;
                case Types.PathFindResultType.EqualStartAndEnd:
                    {
                        Unit.SetRallyPoint(position);
                    }
                    break;
                default:
                    break;
            }
        }
        _selectResult.CursorType = isOnWay ? Types.CursorType.Success : Types.CursorType.Fail;
        _selectResult.SelectResultType = isOnWay ? Types.SelectResultType.Unregister : Types.SelectResultType.None;
        _selectResult.IsFlag = true;
        _selectResult.IsSpread = false;
        return _selectResult;
    }

    override public void SelectExit()
    {
        base.SelectExit();

        UICanvas.Inst.HeroUnitButton.Select(false);
    }

    void OnUnitEvent(Types.UnitEventType unitEventType, Unit unit)
    {
        switch (unitEventType)
        {
            case Types.UnitEventType.None:
                break;
            case Types.UnitEventType.AttackStart:
                break;
            case Types.UnitEventType.AttackEnd:
                break;
            case Types.UnitEventType.AttackFire:
                break;
            case Types.UnitEventType.Die:
                if (Selected)
                {
                    SelectorManager.Inst.UnregisterSelector();
                }
                break;
            case Types.UnitEventType.DiedComplete:
                break;
            case Types.UnitEventType.AttackStopped:
                break;
            default:
                break;
        }
    }
}
