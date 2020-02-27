using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData_", menuName = "Create Data/AttackData")]
public class AttackData : ScriptableObject {
    public AProjectile ProjectilePrefab;
    public float ProjectileSpeed;



    public float Power = 2f;
    public float AttackRange = 1f;

    public CCData CCData;
}
