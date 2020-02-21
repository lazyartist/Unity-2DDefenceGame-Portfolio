using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SingletonBase<StageManager> {
    public Types.StageEvent StageEvent;

    public StageData StageData;
    public StageInfo StageInfo;

    protected override void Awake()
    {
        base.Awake();
        StageInfo = new StageInfo();
        StageInfo.Copy(StageData);
        StageInfo.WaveCount = 0;
    }

    private void Start()
    {
    }

    public void DispatchEvent(Types.StageEventType stageEventType)
    {
        if (StageEvent != null)
        {
            StageEvent(stageEventType);
        }
    }
}
