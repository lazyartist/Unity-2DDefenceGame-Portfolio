using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MasterSkillData_", menuName = "Create Data/MasterSkillData")]
public class MasterSkillData : ScriptableObject {
    public Sprite Icon;
    public float CoolTime;
    public string Name;

    [Header("Spawn")]
    public int SpawnCount;
    public float SpawnRadius;
    public float SpawnInterval;
    public Vector3 SpawnPositionOffset;
    public Vector3 SpawnAngle;

    [Header("Projectile")]
    public AttackData AttackData;
    public AttackTargetData AttackTargetData;

    [Header("Unit")]
    public Unit UnitPrefab;

    
}
