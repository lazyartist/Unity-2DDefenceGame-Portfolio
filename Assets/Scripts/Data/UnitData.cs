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
    public Types.UnitTargetRangeType DefaultUnitTargetRangeType;

    [Header("AttackDatas[Short, Long]")]
    public AttackDataList[] AttackDatasLists;
    public float AttackCoolTime = 1f;

    [Header("Status")]
    public float Health = 10f;
    public float MoveSpeed = 2f;
    public int Gold;
    public int StageHealthDamage = 1;

}
