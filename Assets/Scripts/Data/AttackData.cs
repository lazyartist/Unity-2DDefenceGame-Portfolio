using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData_", menuName = "SO/Create AttackData")]
public class AttackData : ScriptableObject {
    public float Power = 2f;
    public float ExplosionRange = 1f;

    public Consts.SkillType SkillType;
    public float SkillTime;
    public float SkillValue;
}
