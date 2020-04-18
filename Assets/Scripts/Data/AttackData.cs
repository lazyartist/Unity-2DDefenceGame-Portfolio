using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData_", menuName = "Create Data/AttackData")]
[System.Serializable]
public class AttackData : ScriptableObject
{
    public Types.UnitPlaceType[] TargetUnitTypes;
    public string AttackAniName = "Attack0";

    public AProjectile ProjectilePrefab;
    public float ProjectileSpeed;

    public float Power = 2f;
    public float AttackRange = 1f;
    public bool IsStartDelayForCoolTime = true;
    public float CoolTime = 1.5f;
    public CCData CCData;

    [Header("Audio")]
    public string StartAudioName;
    public string FireAudioName;
    public string HitAudioName;
    public float AudioVolume = 1.0f;

    public AttackLogicBase AttackLogicBase;
}
