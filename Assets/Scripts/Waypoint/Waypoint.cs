using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour{
    public Waypoint NextWaypoint;
    public int OrderNumber;
    public int SubPositionCount;
    public float SubPositionRadius = 1f;

    public Vector3 GetPosition(int subPositionIndex)
    {
        Vector3 subPosition = Vector3.zero;
        if(SubPositionCount > subPositionIndex)
        {
            Vector3 direction = Vector3.right;
            subPosition = Quaternion.Euler(0f, 0f, 30f + 120f * subPositionIndex) * direction * SubPositionRadius;
        }

        return transform.position + subPosition;
    }
}
