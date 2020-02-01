﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSpawnTest : MonoBehaviour {
    public GameObject UnitContainer;
    public Unit UnitPrefab;
    public Waypoint StartWaypoint;
    public float SpawnDelayMin = 1f;
    public float SpawnDelayMax = 2f;
    public Text UnitCountText;

    private void Start()
    {
        //StartCoroutine(RandomSpawnCoRoutine());
        StartCoroutine(WaypointSpawnCoroutine());
    }

    private void Update()
    {
        UnitCountText.text = UnitContainer.transform.childCount.ToString();
    }

    IEnumerator RandomSpawnCoRoutine()
    {
        while (true)
        {
            Unit unit = Instantiate<Unit>(UnitPrefab);

            unit.TeamType = Random.Range(0, 2) == 0 ? Types.TeamType.A : Types.TeamType.B;

            unit.transform.position = new Vector3(Random.Range(-4, 4), Random.Range(-2, 2), 0f);

            yield return new WaitForSeconds(Random.Range(1.5f, 3f));
        }
    }

    IEnumerator WaypointSpawnCoroutine()
    {
        int unitNumber = 0;
        int pathCount = WaypointManager.Inst.PathCount;
        while (true)
        {
            Unit unit = Instantiate<Unit>(UnitPrefab, StartWaypoint.transform.position, Quaternion.identity, UnitContainer.transform);

            unit.name = unit.name + unitNumber++;

            unit.AutoAttack = true;
            unit.AutoMoveToTarget = true;
            unit.AutoMoveToWaypoint = true;

            //unit.TargetWaypoint = StartWaypoint;
            unit.TargetWaypoint = WaypointManager.Inst.StartWaypoints[Random.Range(0, pathCount)];

            unit.TeamType = Types.TeamType.B;
            //unit.transform.position = new Vector3(Random.Range(-4, 4), Random.Range(-2, 2), 0f);

            yield return new WaitForSeconds(Random.Range(SpawnDelayMin, SpawnDelayMax));
        }
    }
}
