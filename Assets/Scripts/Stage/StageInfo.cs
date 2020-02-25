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

    int _wavePhaseIndex;
    public int WavePhaseIndex
    {
        get
        {
            return _wavePhaseIndex;
        }
        set
        {
            _wavePhaseIndex = value;
            IsDirty = true;
        }
    }

    public int WavePhaseCount { get; private set; }

    public void Copy(StageData stageData)
    {
        Health = stageData.Health;
        Gold = stageData.Gold;
        WavePhaseCount = stageData.WavePhases.Length;
    }

    public int WaveIndex = 0;
    public bool IsWavePhaseDone = false;
    public bool IsAllWavePhaseDone = false;
}
