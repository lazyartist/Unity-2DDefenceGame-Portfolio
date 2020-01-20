using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGizmos : MonoBehaviour {
    public WaypointGizmosData WaypointGizmosData;



    private void OnDrawGizmos()
    {
        Gizmos.color = WaypointGizmosData.Color;
        Gizmos.DrawWireSphere(transform.position, WaypointGizmosData.Size);

        Waypoint wp = GetComponent<Waypoint>();
        if (wp != null && wp.NextWaypoint != null)
        {
            Gizmos.DrawLine(transform.position, wp.NextWaypoint.transform.position);
        }
    }

}
