using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SingletonBase<StageManager>
{
    public Types.StageEvent StageEvent;
    public StageData StageData;
    public StageInfo StageInfo;

    public GameObject UnitsContainer;
    public GameObject UnitStartPosition;
    public Waypoint StartWaypoint;
    public WaveData WaveData;
    WaveBundle _curWaveBundle;
    int _curWaveBundleIndex = 0;
    WaveInfo _curWave;
    int _curWaveIndex = 0;
    bool _curWaveDone = false;
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
        NextWave();

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

        co = StartCoroutine(Coroutine_CreateUnitsOfWave());
    }

    IEnumerator Coroutine_CreateUnitsOfWave()
    {
        _curWaveIndex = 0;
        while (true)
        {
            if (_curWaveBundle.WaveInfos.Length <= _curWaveIndex)
            {
                Debug.Log("yield return break");
                yield break;
            }

            WaveInfo wave = _curWaveBundle.WaveInfos[_curWaveIndex++];
            int waypointSubIndex = 0;
            for (int i = 0; i < wave.UnitCount; i++)
            {
                Unit unit = Instantiate(wave.UnitPrefab, UnitStartPosition.transform.position, Quaternion.identity, UnitsContainer.transform);
                unit.TargetWaypoint = StartWaypoint;
                unit.TargetWaypointSubIndex = waypointSubIndex++;

                yield return new WaitForSeconds(wave.CreateUnitInterval);
            }

            if (wave.NextWaveInterval > 0)
            {
                Debug.Log("wave.NextWaveInterval " + wave.NextWaveInterval);
                yield return new WaitForSeconds(wave.NextWaveInterval);
            }
            else
            {
                Debug.Log("yield return null");
                yield return null;
            }
        }
    }

    void ClearWaveInfo()
    {
        _curWaveBundleIndex = 0;
        _curWaveIndex = 0;
    }

    void NextWave()
    {
        _curWaveBundle = WaveData.WaveBundles[_curWaveBundleIndex];
        _curWave = _curWaveBundle.WaveInfos[_curWaveIndex];
    }
}
