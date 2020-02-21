using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo
{
    public bool IsDirty { get; private set; }
    public void Clean()
    {
        IsDirty = false;
    }

    int _health;
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            IsDirty = true;
        }
    }

    int _gold;
    public int Gold
    {
        get
        {
            return _gold;
        }
        set
        {
            _gold = value;
            IsDirty = true;
        }
    }

    int _waveCount;
    public int WaveCount
    {
        get
        {
            return _waveCount;
        }
        set
        {
            _waveCount = value;
            IsDirty = true;
        }
    }

    public void Copy(StageData stageData)
    {
        Health = stageData.Health;
        Gold = stageData.Gold;
        //WaveCount = stageData.WaveCount;
    }
}
