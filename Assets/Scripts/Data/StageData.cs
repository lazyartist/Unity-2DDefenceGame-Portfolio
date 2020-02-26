﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData_", menuName = "SO/Create StageData")]
public class StageData : ScriptableObject
{
    [Header("Team")]
    public TeamData PlayerTeamData;
    public TeamData EnemyTeamData;
    public AttackTargetData EnemyAttackTargetData;

    [Header("Wave")]
    public WavePhase[] WavePhases;

    [Header("HeroUnit")]
    public Unit HeroUnitPrefab;
    public float HeroUnitRespawnCoolTime;
    public Vector3 FirstHeroUnitPosition;

    [Header("Player")]
    public int Health = 20;
    public int Gold = 100;
}
