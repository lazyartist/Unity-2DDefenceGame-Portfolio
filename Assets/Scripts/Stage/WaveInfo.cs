using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveInfo
{
    public Unit UnitPrefab;
    public int UnitCount;
    public float CreateUnitInterval;
    public float NextWaveInterval;
}
