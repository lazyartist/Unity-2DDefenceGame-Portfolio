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
        StageManager.Inst.StageEvent += _OnStageEvent;

        _UpdateStageInfo();
    }
	
    void _UpdateStageInfo()
    {
        HealthText.text = _stageInfo.Health.ToString();
        GoldText.text = _stageInfo.Gold.ToString();
        WaveText.text = "공격 " + _stageInfo.WaveCount.ToString() + "/" + _stageData.WaveCount.ToString();
    }

    //private void OnApplicationQuit()
    //{
    //    StageManager.Inst.StageEvent -= _OnStageEvent;
    //}

    //private void OnDestroy()
    //{
    //    StageManager.Inst.StageEvent -= _OnStageEvent;
    //}

    public void _OnStageEvent(Types.StageEventType stageEventType)
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
