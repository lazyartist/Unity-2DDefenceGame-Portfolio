using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData_", menuName = "Create Data/UnitData")]
public class UnitData : ScriptableObject
{
    public string UnitTypeName;
    public Types.UnitPlaceType UnitPlaceType;

    public float Health = 10f;
    public float MoveSpeed = 2f;
    public float TargetRange = 1f;
    public int Gold;
    public int StageHealthDamage = 1;

    public Types.UnitTargetRangeCenterType UnitTargetRangeCenterType;
    public Types.UnitSortingLayerType UnitSortingLayerType;
}
