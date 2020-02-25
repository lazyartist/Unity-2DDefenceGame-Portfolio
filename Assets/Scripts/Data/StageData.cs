using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData_", menuName = "SO/Create StageData")]
public class StageData : ScriptableObject
{
    public TeamData EnemyTeamData;
    public AttackTargetData EnemyAttackTargetData;
    public WavePhase[] WavePhases;

    public int Health = 20;
    public int Gold = 100;
}
