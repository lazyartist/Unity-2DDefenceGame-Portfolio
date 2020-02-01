using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasterSkillButton : MonoBehaviour, IPointerClickHandler {
    public Types.MasterSkillType MasterSkillType;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        MasterSkillManager.Inst.MasterSkillType = MasterSkillType;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MasterSkillManager.Inst.MasterSkillType = MasterSkillType;
    }
}
