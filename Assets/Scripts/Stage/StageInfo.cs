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
        LastHeroRallyPoint = stageData.FirstHeroRallyPoint;
        TimeScale = stageData.StartTimeScale;
    }

    public bool IsPlayerDirty { get; private set; }
    public void Clean()
    {
        IsPlayerDirty = false;
        IsWaveDirty = false;
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
            IsPlayerDirty = true;
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
            IsPlayerDirty = true;
        }
    }
    // Player ===== end

    // Wave =====
    public bool IsWaveDirty;
    public int WavePhaseIndex;
    public int WavePhaseCount;
    public int WaveIndex = 0;
    public bool IsWaveStarted = false;
    public bool IsWavePhaseDone = false;
    public bool IsAllWavePhaseDone = false;
    // Wave ===== end

    // HeroUnit
    public Unit HeroUnit;
    public float HeroUnitDiedTime = 0f;
    public Vector3 LastHeroRallyPoint;
    // HeroUnit ===== end

    // TimeScale =====
    float _timeScale;
    public float TimeScale
    {
        get
        {
            return _timeScale;
        }
        set
        {
            _timeScale = value;
            IsPlayerDirty = true;
        }
    }
    // TimeScale ===== end
}
