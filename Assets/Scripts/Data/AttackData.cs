using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData_", menuName = "Create Data/AttackData")]
public class AttackData : ScriptableObject
{
    public Types.UnitPlaceType[] TargetUnitTypes;

    public AProjectile ProjectilePrefab;
    public float ProjectileSpeed;

    public float Power = 2f;
    public float AttackRange = 1f;
    public bool IsStartDelayForCoolTime = true;
    public float CoolTime = 1.5f;
    public CCData CCData;

    [Header("Audio")]
    //public Types.AudioChannelType StartAudioChannelType = Types.AudioChannelType.None;
    public string StartAudioName;
    //public Types.AudioChannelType FireAudioChannelType = Types.AudioChannelType.None;
    public string FireAudioName;
    //public Types.AudioChannelType HitAudioChannelType = Types.AudioChannelType.None;
    public string HitAudioName;
    public float AudioVolume = 1.0f;
}
