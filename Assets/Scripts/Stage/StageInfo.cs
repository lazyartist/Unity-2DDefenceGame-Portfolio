using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo {
    public int Health;
    public int Gold;
    public int WaveCount;

    public void Copy(StageData stageData)
    {
        Health = stageData.Health;
        Gold = stageData.Gold;
        //WaveCount = stageData.WaveCount;
    }
}
