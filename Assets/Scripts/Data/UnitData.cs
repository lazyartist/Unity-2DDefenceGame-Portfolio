﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData_", menuName = "Create Data/UnitData")]
public class UnitData : ScriptableObject
{
    public Types.UnitType UnitType;
    public float Health = 10f;
    public float MoveSpeed = 2f;

    public float TargetRange = 1f;
    public float AttackCoolTime = 2f;// todo remove

    public int Gold;
    public int StageHealthDamage = 1;

    public SortingLayer SortingLayer;
}
