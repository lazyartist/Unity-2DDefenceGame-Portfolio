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

    Coroutine _coroutine_wave;

    protected override void Awake()
    {
        base.Awake();
        StageInfo = new StageInfo();
    }

    void Start()
    {
        StageInfo.Copy(StageData);
        StageInfo.WavePhaseIndex = 0;

        ClearWaveInfo();
        SpawnHeroUnit();
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

        UpdateStageClear();
    }

    void UpdateStageClear()
    {
        if (StageInfo.Health <= 0)
        {
            UICanvas.Inst.ShowInfo("Defeat");
        }
        else
        {
            if (StageInfo.IsAllWavePhaseDone && UnitsContainer.transform.childCount == 0)
            {
                UICanvas.Inst.ShowInfo("Clear");
            }
        }
    }

    // Wave =====
    public void RunWave()
    {
        if (_coroutine_wave != null)
        {
            StopCoroutine(_coroutine_wave);
        }

        ClearWaveInfo();
        _coroutine_wave = StartCoroutine(Coroutine_Wave());
    }

    IEnumerator Coroutine_Wave()
    {
        while (true)
        {
            Wave wave = StageData.WavePhases[StageInfo.WavePhaseIndex].Waves[StageInfo.WaveIndex];
            int waypointSubIndex = Consts.WaypointSubIndexStart;
            for (int i = 0; i < wave.UnitCount; i++)
            {
                Unit unit = Instantiate(wave.UnitPrefab, UnitStartPosition.transform.position, Quaternion.identity, UnitsContainer.transform);
                unit.TeamData = StageData.EnemyTeamData;
                unit.AttackTargetData = StageData.EnemyAttackTargetData;
                unit.TargetWaypoint = StartWaypoint;
                unit.TargetWaypointSubIndex = waypointSubIndex++;
                unit.gameObject.SetActive(true);

                yield return new WaitForSeconds(Consts.CreateUnitInterval);
            }

            ++StageInfo.WaveIndex;
            if (StageData.WavePhases[StageInfo.WavePhaseIndex].Waves.Length == StageInfo.WaveIndex)
            {
                StageInfo.IsWavePhaseDone = true;
            }

            if (wave.NextWaveInterval > 0)
            {
                Debug.Log("wave.NextWaveInterval " + wave.NextWaveInterval);
                yield return new WaitForSeconds(wave.NextWaveInterval);
            }

            if (StageInfo.IsWavePhaseDone && TryNextWavePhase() == false)
            {
                StageInfo.IsAllWavePhaseDone = true;
                yield break;
            }
        }
    }

    void ClearWaveInfo()
    {
        StageInfo.WavePhaseIndex = 0;
        StageInfo.WaveIndex = 0;
        StageInfo.IsWavePhaseDone = false;
        StageInfo.IsAllWavePhaseDone = false;
    }

    bool TryNextWavePhase()
    {
        if (StageData.WavePhases.Length <= StageInfo.WavePhaseIndex + 1)
        {
            return false;
        }
        else
        {
            ++StageInfo.WavePhaseIndex;
            StageInfo.WaveIndex = 0;
            StageInfo.IsWavePhaseDone = false;
            return true;
        }
    }
    // Wave ===== end

    // HeroUnit =====
    void OnUnitEvent_HeroUnit(Types.UnitEventType unitEventType, Unit unit)
    {
        switch (unitEventType)
        {
            case Types.UnitEventType.None:
                break;
            case Types.UnitEventType.AttackStart:
                break;
            case Types.UnitEventType.AttackEnd:
                break;
            case Types.UnitEventType.AttackFire:
                break;
            case Types.UnitEventType.Die:
                if (StageInfo.HeroUnit != null)
                {
                    StageInfo.HeroUnit.UnitEvent -= OnUnitEvent_HeroUnit;
                    StageInfo.HeroUnit = null;
                }
                StageInfo.LastHeroUnitPosition = unit.transform.position;
                StageInfo.HeroUnitDiedTime = Time.time;
                StartCoroutine(Coroutine_RespawnHeroUnit());
                break;
            case Types.UnitEventType.DiedComplete:
                break;
            case Types.UnitEventType.AttackStopped:
                break;
            default:
                break;
        }
    }

    IEnumerator Coroutine_RespawnHeroUnit()
    {
        yield return new WaitForSeconds(StageData.HeroUnitRespawnCoolTime);
        SpawnHeroUnit();
    }

    void SpawnHeroUnit()
    {
        StageInfo.HeroUnit = Instantiate(StageData.HeroUnitPrefab, StageInfo.LastHeroUnitPosition, Quaternion.identity, UnitsContainer.transform);
        StageInfo.HeroUnit.SetRallyPoint(StageInfo.LastHeroUnitPosition);
        StageInfo.HeroUnit.UnitEvent += OnUnitEvent_HeroUnit;
    }
    // HeroUnit ===== end
}
