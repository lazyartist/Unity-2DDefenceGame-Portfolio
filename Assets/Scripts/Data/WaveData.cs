using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData_", menuName = "SO/Create WaveData")]
public class WaveData : ScriptableObject {
    [SerializeField]
    public WaveBundle[] WaveBundles;
    //public WaveBundle WaveBundle;
    //public WaveInfo WaveInfo;
}
