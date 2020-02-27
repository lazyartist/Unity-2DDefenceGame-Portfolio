using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSetData_", menuName = "Create Data/AttackSetData")]
public class AttackSetData : ScriptableObject {
    public AttackData[] AttackDatas;
    [Range(0,1)]
    public float[] AttackWeights;

    //public AProjectile ProjectilePrefab;
    //public float ProjectileSpeed;

    //public int ProjectileCount;
    //public float ProjectileSpawnRadius;
    //public float ProjectileSpawnInterval;
    //public Vector3 ProjectileSpawnPositionOffset;
    //public Vector3 ProjectileSpawnAngle;

    //public float Power = 2f;
    //public float AttackRange = 1f;

    //public CCData CCData;
}
