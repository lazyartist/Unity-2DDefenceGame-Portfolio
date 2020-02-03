using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData_", menuName = "SO/Create AttackData")]
public class AttackData : ScriptableObject {
    public ProjectileAbstract ProjectilePrefab;
    //public Types.AttackRangeType AttackRangeType;
    //public Types.AttackArea_ AttackArea;

    public float Power = 2f;
    public float AttackRange = 1f;

    public CCData CCData;
}
