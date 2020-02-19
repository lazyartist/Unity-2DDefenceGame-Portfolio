using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MasterSkillButton : MonoBehaviour, IPointerClickHandler {
    public MasterSkillData MasterSkillData;

    public Image IconBg;
    public Image Icon;
    public Sprite ButtonFrameSprite;
    public Sprite ButtonFrameActivateSprite;
    public Toggle Toggle;

    //public Types.MasterSkillType MasterSkillType;
    // Use this for initialization

    float _coolTime = 0f;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void OnClick()
    {
        if (Toggle.isOn)
        {
            UICanvas.Inst.ShowInfo(MasterSkillData.Name);
        } else
        {
            UICanvas.Inst.HideInfo();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //MasterSkillManager.Inst.MasterSkillType = MasterSkillType;
    }
}
