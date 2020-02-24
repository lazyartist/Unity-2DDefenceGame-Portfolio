using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGizmos : MonoBehaviour {
    public WaypointGizmosData WaypointGizmosData;

    void OnDrawGizmos()
    {
        Waypoint wp = GetComponent<Waypoint>();
        if (wp != null && wp.NextWaypoint != null)
        {
            Gizmos.color = WaypointGizmosData.Color;
            Gizmos.DrawWireSphere(transform.position, wp.SubPositionRadius);
            Gizmos.DrawLine(transform.position, wp.NextWaypoint.transform.position);
        }
    }

}
