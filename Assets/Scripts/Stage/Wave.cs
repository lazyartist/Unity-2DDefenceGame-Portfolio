using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Wave
{
    public Unit UnitPrefab;
    public int UnitCount;
    public float NextWaveInterval;
    public int WayPointContainerIndex;
}
