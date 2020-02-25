using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint NextWaypoint;
    public int OrderNumber;
    public WaypointData WaypointData;
    
    public Vector3 GetPosition(int subPositionIndex)
    {
        if (subPositionIndex == 0)
        {
            return transform.position;
        }
        else
        {
            Vector3 subPosition = Vector3.zero;
            if (WaypointData.SubPositionCount > subPositionIndex)
            {
                Vector3 direction = Vector3.right;
                subPosition = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + 360f / WaypointData.SubPositionCount * subPositionIndex) * direction * WaypointData.SubPositionRadius;
            }

            return transform.position + subPosition;
        }
    }
}
