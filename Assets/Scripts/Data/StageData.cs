using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData_", menuName = "SO/Create StageData")]
public class StageData : ScriptableObject
{
    public int Health = 20;
    public int Gold = 100;
    //public int WaveCount = 10;
}
