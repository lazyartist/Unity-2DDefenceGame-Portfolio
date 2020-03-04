using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo
{
    public void Copy(StageData stageData)
    {
        Health = stageData.Health;
        Gold = stageData.Gold;
        WavePhaseCount = stageData.WavePhases.Length;
        LastHeroUnitPosition = stageData.FirstHeroUnitPosition;
    }

    public bool IsDirty { get; private set; }
    public void Clean()
    {
        IsDirty = false;
    }

    // Player =====
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
    // Player ===== end

    // Wave =====
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
    public int WaveIndex = 0;
    public bool IsWaveStarted = false;
    public bool IsWavePhaseDone = false;
    public bool IsAllWavePhaseDone = false;
    // Wave ===== end

    // HeroUnit
    public Unit HeroUnit;
    public float HeroUnitDiedTime = 0f;
    public Vector3 LastHeroUnitPosition;
    // HeroUnit ===== end
}
