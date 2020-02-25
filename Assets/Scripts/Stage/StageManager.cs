using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SingletonBase<StageManager>
{
    public Types.StageEvent StageEvent;
    public StageData StageData;
    public StageInfo StageInfo;

    public TeamData EnemyTeamData;
    public AttackTargetData EnemyAttackTargetData;

    public GameObject UnitsContainer;
    public GameObject UnitStartPosition;
    public Waypoint StartWaypoint;

    public WaveData WaveData;
    public int WaveBundleIndex = 0;
    WaveBundle _waveBundle;
    WaveInfo _wave;
    int _waveIndex = 0;
    bool _isWaveBundleDone = false;
    Coroutine co;

    protected override void Awake()
    {
        base.Awake();
        StageInfo = new StageInfo();
    }

    void Start()
    {
        StageInfo.Copy(StageData);
        StageInfo.WaveCount = 0;

        ClearWaveInfo();
        SetWaveBundle();

        CreateUnitsOfWave();
    }

    void Update()
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

    void CreateUnitsOfWave()
    {
        if (co != null)
        {
            StopCoroutine(co);
        }

        ClearWaveInfo();
        co = StartCoroutine(Coroutine_CreateUnitsOfWave());
    }

    IEnumerator Coroutine_CreateUnitsOfWave()
    {
        while (true)
        {
            WaveInfo wave = _waveBundle.WaveInfos[_waveIndex];
            int waypointSubIndex = Consts.WaypointSubIndexStart;
            for (int i = 0; i < wave.UnitCount; i++)
            {
                Unit unit = Instantiate(wave.UnitPrefab, UnitStartPosition.transform.position, Quaternion.identity, UnitsContainer.transform);
                unit.TeamData = EnemyTeamData;
                unit.AttackTargetData = EnemyAttackTargetData;
                unit.TargetWaypoint = StartWaypoint;
                unit.TargetWaypointSubIndex = waypointSubIndex++;

                yield return new WaitForSeconds(wave.CreateUnitInterval);
            }

            ++_waveIndex;
            if (_waveBundle.WaveInfos.Length == _waveIndex)
            {
                _isWaveBundleDone = true;
            }

            if (wave.NextWaveInterval > 0)
            {
                Debug.Log("wave.NextWaveInterval " + wave.NextWaveInterval);
                yield return new WaitForSeconds(wave.NextWaveInterval);
            }

            if (_isWaveBundleDone && TryNextWaveBundle() == false)
            {
                yield break;
            }
        }
    }

    void ClearWaveInfo()
    {
        WaveBundleIndex = 0;
        _waveIndex = 0;
        _isWaveBundleDone = false;
    }

    void SetWaveBundle()
    {
        _waveIndex = 0;
        _isWaveBundleDone = false;

        _waveBundle = WaveData.WaveBundles[WaveBundleIndex];
        _wave = _waveBundle.WaveInfos[_waveIndex];

        StageInfo.WaveCount = WaveBundleIndex + 1;
    }

    bool TryNextWaveBundle()
    {
        if (WaveData.WaveBundles.Length <= WaveBundleIndex + 1)
        {
            return false;
        }
        else
        {
            ++WaveBundleIndex;
            SetWaveBundle();
            return true;
        }
    }
}
