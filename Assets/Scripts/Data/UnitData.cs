using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData_", menuName = "Create Data/UnitData")]
public class UnitData : ScriptableObject
{
    public string UnitTypeName;

    [Header("Types")]
    public Types.UnitPlaceType UnitPlaceType;
    public Types.UnitTargetRangeCenterType UnitTargetRangeCenterType;
    public Types.UnitSortingLayerType UnitSortingLayerType; // for sprite render

    [Header("AttackDatas[Short, Long]")]
    public AttackDataList[] AttackDatasLists;
    public float []TargetRanges;
    public float ShortTargetRange = 1f; // deprecated
    public float LongTargetRange = 1f; // deprecated
    public float AttackCoolTime = 1f;

    [Header("Status")]
    public float Health = 10f;
    public float MoveSpeed = 2f;
    public int Gold;
    public int StageHealthDamage = 1;

}
