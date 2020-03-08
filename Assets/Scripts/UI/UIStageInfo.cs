using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStageInfo : MonoBehaviour
{
    public Text HealthText;
    public Text GoldText;
    public Text WaveText;
    public Button StartWaveButton;
    public Button FastForwardButton;
    public Button PauseButton;
    public Text TimeScaleText;

    StageData _stageData;
    StageInfo _stageInfo;
    bool _isPaused = false;

    void Start()
    {
        _stageData = StageManager.Inst.StageData;
        _stageInfo = StageManager.Inst.StageInfo;
        StageManager.Inst.StageEvent += OnStageEvent;

        StartWaveButton.onClick.AddListener(() =>
        {
            StageManager.Inst.RunWave();
            StartWaveButton.gameObject.SetActive(!StageManager.Inst.StageInfo.IsWaveStarted);
        });

        FastForwardButton.onClick.AddListener(() =>
        {
            Time.timeScale += 1.0f;
            if(Time.timeScale > _stageData.MaxTimeScale)
            {
                Time.timeScale = 1.0f;
            }
            UpdateStageInfo();
        });

        PauseButton.onClick.AddListener(() =>
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0f : 1f;
        });

        UpdateStageInfo();
    }

    void UpdateStageInfo()
    {
        HealthText.text = _stageInfo.Health.ToString();
        GoldText.text = _stageInfo.Gold.ToString();
        WaveText.text = (_stageInfo.WavePhaseIndex + 1).ToString() + "/" + StageManager.Inst.StageInfo.WavePhaseCount.ToString();
        TimeScaleText.text = "x" + Time.timeScale.ToString();
        StartWaveButton.gameObject.SetActive(!StageManager.Inst.StageInfo.IsWaveStarted);
    }

    public void OnStageEvent(Types.StageEventType stageEventType)
    {
        switch (stageEventType)
        {
            case Types.StageEventType.None:
                break;
            case Types.StageEventType.StageInfoChanged:
                UpdateStageInfo();
                break;
            default:
                break;
        }
    }
}
