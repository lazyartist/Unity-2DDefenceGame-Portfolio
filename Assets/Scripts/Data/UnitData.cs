using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData_", menuName = "SO/Create UnitData")]
public class UnitData : ScriptableObject
{
    public float Health = 10f;
    public float Speed = 2f;

    public float TargetRange = 1f;
    public float AttackCoolTime = 2f;

    //public Types.AttackRangeType AttackRangeType;
    //public Vector2 AttackRangeOffset;
    //public Vector2 AttackRangeSize;
}
