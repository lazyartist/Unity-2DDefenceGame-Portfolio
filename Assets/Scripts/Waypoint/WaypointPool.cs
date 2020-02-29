using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPool : MonoBehaviour {
    public GameObject WaypointPoolContainer;
    public WaypointData WaypointData;

    public Waypoint WaypointPrefab;
    public int WaypointCount = 99;
    public Waypoint[] Waypoints;

    private int _endIndex = -1;// 항상 마지막 waypoint를 가리킨다.

    void Awake()
    {
        Waypoints = new Waypoint[WaypointCount];
    }

    public Waypoint Get()
    {
        ++_endIndex;
        // todo range assert
        if(_endIndex >= WaypointCount)
        {
            Debug.LogAssertion("waypoint pool range error!!!");
        }

        if(Waypoints[_endIndex] == null)
        {
            Waypoint waypoint = Instantiate<Waypoint>(WaypointPrefab, WaypointPoolContainer.transform);
            waypoint.WaypointData = WaypointData;
            waypoint.gameObject.layer = LayerMask.NameToLayer(WaypointData.LayerName);
            Waypoints[_endIndex] = waypoint;
        }
        Waypoints[_endIndex].enabled = true;
        //Debug.Log("waypoint get _endIndex " + _endIndex);
        return Waypoints[_endIndex];
    }

    public void Release(Waypoint waypoint)
    {
        if (waypoint == null) return;

        // waypoint 인덱스 검사
        int waypointIndex = -1;
        for (int i = 0; i < Waypoints.Length; i++)
        {
            if(Waypoints[i] == waypoint)
            {
                waypointIndex = i;
                break;
            }
        }

        if(waypointIndex == -1)
        {
            // 객체풀에 있는 waypoint가 아니라 이동 경로에 있는 waypoint이다.
            return;
        }

        // 해제하는 Waypoint를 활성화된 Waypoint 가장 끝으로 보내고 비활성화 시킴
        Waypoint tempWaypoint = Waypoints[waypointIndex];
        tempWaypoint.enabled = false;
        Waypoints[waypointIndex] = Waypoints[_endIndex];
        Waypoints[_endIndex] = tempWaypoint;
        // 활성화된 Waypoint의 끝을 한칸 앞으로 이동
        --_endIndex;
        //Debug.Log("waypoint rel _endIndex " + _endIndex);
    }
}
