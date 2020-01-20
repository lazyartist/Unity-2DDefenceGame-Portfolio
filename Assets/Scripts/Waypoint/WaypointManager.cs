using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : SingletonBase<WaypointManager>
{
    public GameObject[] WaypointContainers;
    public List<Waypoint> StartWaypoints { get; private set; }
    public int PathCount { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        StartWaypoints = new List<Waypoint>();
        LinkWaypoints();
    }

    public void LinkWaypoints()
    {
        PathCount = WaypointContainers.Length;
        StartWaypoints.Clear();

        for (int i = 0; i < PathCount; i++)
        {
            GameObject waypointContainer = WaypointContainers[i];
            Waypoint startWaypoint = waypointContainer.transform.GetChild(0).GetComponent<Waypoint>();
            StartWaypoints.Add(startWaypoint);
            Waypoint curWaypoint = startWaypoint;

            for (int j = 1; j < waypointContainer.transform.childCount; j++)
            {
                Waypoint wp = waypointContainer.transform.GetChild(j).GetComponent<Waypoint>();
                curWaypoint.NextWaypoint = wp;
                curWaypoint = wp;
            }
        }
        
    }

    //private Waypoint GetWaypoint(int index)
    //{
    //    return WaypointContainer.transform.GetChild(index).GetComponent<Waypoint>();
    //}
}
