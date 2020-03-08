using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData_", menuName = "Create Data/StageData")]
public class StageData : ScriptableObject
{
    [Header("Team")]
    public TeamData PlayerTeamData;
    public TeamData EnemyTeamData;

    [Header("Wave")]
    public WavePhase[] WavePhases;

    [Header("HeroUnit")]
    public Unit HeroUnitPrefab;
    public Sprite HeroUnitIcon;
    public float HeroUnitRespawnCoolTime;
    public Vector3 FirstHeroUnitPosition;

    [Header("Player")]
    public int Health = 20;
    public int Gold = 100;
    public float StartTimeScale = 1.0f;
    public float MaxTimeScale = 4.0f;
}
