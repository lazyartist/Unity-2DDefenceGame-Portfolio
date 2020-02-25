using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStageInfo : MonoBehaviour {
    public Text HealthText;
    public Text GoldText;
    public Text WaveText;

    StageData _stageData;
    StageInfo _stageInfo;

    void Start () {
        _stageData = StageManager.Inst.StageData;
        _stageInfo = StageManager.Inst.StageInfo;
        StageManager.Inst.StageEvent += OnStageEvent;

        _UpdateStageInfo();
    }
	
    void _UpdateStageInfo()
    {
        HealthText.text = _stageInfo.Health.ToString();
        GoldText.text = _stageInfo.Gold.ToString();
        WaveText.text = "공격 " + (_stageInfo.WavePhaseIndex + 1).ToString() + "/" + StageManager.Inst.StageInfo.WavePhaseCount.ToString();
    }

    public void OnStageEvent(Types.StageEventType stageEventType)
    {
        switch (stageEventType)
        {
            case Types.StageEventType.None:
                break;
            case Types.StageEventType.Changed:
                _UpdateStageInfo();
                break;
            default:
                break;
        }
    }
}
