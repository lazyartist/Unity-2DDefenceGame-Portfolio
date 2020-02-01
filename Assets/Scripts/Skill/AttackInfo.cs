using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackInfo
{
    public float TargetRange = 5f;

    public float AttackPower = 2f;
    public float AttackCoolTime = 2f;
    public float AttackRange = 5f;

    //public float TargetRange;
                            
    //public float AttackPower;
    //public float AttackCoolTime;
    //public float AttackRange;

    public float SkillTime;
    public Types.CCType SkillType;
}
