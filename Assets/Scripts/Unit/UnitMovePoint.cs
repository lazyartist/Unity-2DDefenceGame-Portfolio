using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovePoint : MonoBehaviour
{
    public Types.UnitMovePointType UnitMovePointType;
    public Waypoint WayPoint;
    public Vector3 RallyPoint;
    public bool IsArrived;

    int _wayPointSubIndex;
    List<Vector3> _movePositions = new List<Vector3>(); // 목적지 0번 인덱스
    int _movePointIndex;
    int _movePointEndIndex;

    public void SetWayPoint(Waypoint wayPoint, int wayPointSubIndex)
    {
        SetUnitMovePointType(Types.UnitMovePointType.WayPoint);
        WayPoint = wayPoint;
        _wayPointSubIndex = wayPointSubIndex;
        IsArrived = false;
    }

    public void SetMovePoint(List<Vector3> positions, Vector3 startPosition, Vector3 endPosition)
    {
        SetUnitMovePointType(Types.UnitMovePointType.MovePoint);
        _movePointIndex = 0;

        int movePointIndex = 0;
        AddMovePosition(movePointIndex, startPosition);
        if (positions != null)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                AddMovePosition(++movePointIndex, positions[i]);
            }
        }
        AddMovePosition(++movePointIndex, endPosition);
        _movePointEndIndex = movePointIndex;
        IsArrived = false;
    }

    void AddMovePosition(int index, Vector3 position)
    {
        if (_movePositions.Count <= index)
        {
            _movePositions.Add(position);
        }
        else
        {
            _movePositions[index] = position;
        }
    }

    public void SetRallyPoint(Vector3 position)
    {
        SetUnitMovePointType(Types.UnitMovePointType.RallyPoint);
        RallyPoint = position;
        IsArrived = false;
    }

    void SetUnitMovePointType(Types.UnitMovePointType unitMovePointType)
    {
        UnitMovePointType = unitMovePointType;
    }

    public Vector3 GetPosition()
    {
        switch (UnitMovePointType)
        {
            case Types.UnitMovePointType.WayPoint:
                return WayPoint.GetPosition(_wayPointSubIndex);
            case Types.UnitMovePointType.RallyPoint:
                return RallyPoint;
            case Types.UnitMovePointType.MovePoint:
                return _movePositions[_movePointIndex];
            default:
                break;
        }
        return new Vector3();
    }

    public bool IsArrivedPosition(Vector3 position)
    {
        return IsArrivedPosition(position, GetPosition());
    }

    public bool IsArrivedPosition(Vector3 position, Vector3 targetPosition)
    {
        float distance = Vector3.Distance(targetPosition, position);
        if (distance < Consts.MoveArrivedDistance)
        {
            return true;
        }

        return false;
    }

    public bool IsArrivedRallyPoint(Vector3 position)
    {
        return IsArrivedPosition(position, RallyPoint);
    }

    public bool TryNextPosition()
    {
        bool success = false;
        switch (UnitMovePointType)
        {
            case Types.UnitMovePointType.WayPoint:
                if (WayPoint != null && WayPoint.NextWaypoint != null)
                {
                    WayPoint = WayPoint.NextWaypoint;
                    success = true;
                }
                break;
            case Types.UnitMovePointType.RallyPoint:
                {
                    success = false;
                }
                break;
            case Types.UnitMovePointType.MovePoint:
                {
                    if (_movePointIndex + 1 <= _movePointEndIndex)
                    {
                        success = true;
                        ++_movePointIndex;
                    };
                }
                break;
            default:
                break;
        }

        if (success == false)
        {
            IsArrived = true;
        }

        return success;
    }
}
