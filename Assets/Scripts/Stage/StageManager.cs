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
    }

    private void Start()
    {
        StageInfo.Copy(StageData);
        StageInfo.WaveCount = 0;
    }

    private void Update()
    {
        if (StageInfo.IsDirty)
        {
            if (StageEvent != null)
            {
                StageEvent(Types.StageEventType.Changed);
            }
            StageInfo.Clean();
        }
    }
}
