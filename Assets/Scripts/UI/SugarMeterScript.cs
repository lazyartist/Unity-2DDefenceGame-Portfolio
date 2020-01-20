using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SugarMeterScript : MonoBehaviour {
    public Text SugarMeterText;

	void Awake () {
        PlayerManager.Inst.Event_Sugar_Changed.AddListener(OnSugarChanged);
    }

    void OnDisable()
    {
        //PlayerManager.Inst.Event_Sugar_Changed.RemoveListener(OnSugarChanged);
    }

    void OnSugarChanged()
    {
        SugarMeterText.text = PlayerManager.Inst.Sugar.ToString();
    }
}
