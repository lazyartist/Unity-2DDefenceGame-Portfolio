using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGizmos : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Waypoint wp = GetComponent<Waypoint>();
        if (wp != null)
        {
            Gizmos.color = wp.WaypointData.GizmoColor;
            Gizmos.DrawWireSphere(transform.position, wp.WaypointData.SubPositionRadius);

            for (int i = 0; i < wp.WaypointData.SubPositionCount; i++)
            {
                Vector3 subPosition = Vector3.right;
                Gizmos.color = wp.WaypointData.SubPositionColors[i];
                var a = wp.transform.rotation.eulerAngles;
                subPosition = Quaternion.Euler(0f, 0f, (wp.transform.rotation.eulerAngles.z) + 120f * i) * Vector3.right;
                subPosition *= wp.WaypointData.SubPositionRadius;
                Gizmos.DrawWireCube(transform.position + subPosition, new Vector3(0.2f, 0.2f, 0f));
            }
        }
        if (wp != null && wp.NextWaypoint != null)
        {
            Gizmos.color = wp.WaypointData.GizmoColor;
            Gizmos.DrawLine(transform.position, wp.NextWaypoint.transform.position);
        }
    }
}
