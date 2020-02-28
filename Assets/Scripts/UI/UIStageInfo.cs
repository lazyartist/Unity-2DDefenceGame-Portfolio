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
    public Button ZoomButton;
    public bool IsZoomIn = true;

    StageData _stageData;
    StageInfo _stageInfo;

    void Start()
    {
        _stageData = StageManager.Inst.StageData;
        _stageInfo = StageManager.Inst.StageInfo;
        StageManager.Inst.StageEvent += OnStageEvent;

        StartWaveButton.onClick.AddListener(() =>
        {
            StageManager.Inst.RunWave();
        });

        ZoomButton.onClick.AddListener(() =>
        {
            ToggleZoomMap();
        });
        ToggleZoomMap();

        UpdateStageInfo();
    }

    void ToggleZoomMap()
    {
        InputManager.Inst.InputEvent(Types.InputEventType.Zoom, new Vector3(IsZoomIn ? 1f : -1f, 0f, 0f));
        IsZoomIn = !IsZoomIn;
        ZoomButton.GetComponentInChildren<Text>().text = IsZoomIn ? "지도 확대" : "지도 축소";// todo translate
    }

    void UpdateStageInfo()
    {
        HealthText.text = _stageInfo.Health.ToString();
        GoldText.text = _stageInfo.Gold.ToString();
        WaveText.text = "공격 " + (_stageInfo.WavePhaseIndex + 1).ToString() + "/" + StageManager.Inst.StageInfo.WavePhaseCount.ToString();// todo translate
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
