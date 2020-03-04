using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyPoint : MonoBehaviour
{
    public Vector3 CurPosition;
    public bool IsArrived;

    int _positionIndex;
    List<Vector3> _positions;
    
    public Vector3 GetPosition()
    {
        return _positions[_positionIndex];
    }

    public Vector3 NextPosition()
    {
        CurPosition = _positions[++_positionIndex];
        return CurPosition;
    }

    public void ResetPositions()
    {
        _positionIndex = 0;
    }
    
    //public bool Arrive()
    //{
    //    --_positionIndex;
    //}
}
