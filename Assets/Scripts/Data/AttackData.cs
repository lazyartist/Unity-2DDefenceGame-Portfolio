using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData_", menuName = "SO/Create AttackData")]
public class AttackData : ScriptableObject {
    public Consts.AttackRangeType AttackRangeType;
    public Consts.AttackArea_ AttackArea;

    public float Power = 2f;
    public float ExplosionRange = 1f;

    public CCData CCData;
    //public Consts.CCType CCType;
    //public float CCTime;
    //public float CCValue;
}
