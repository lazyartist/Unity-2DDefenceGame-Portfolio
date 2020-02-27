using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMasterSkillMenu : MonoBehaviour
{
    public Types.MasterSkillEvent MasterSkillEvent;

    public MasterSkillData[] MasterSkillDatas;
    public MasterSkillButton MasterSkillButtonPrefab;
    public MasterSkillButton SelectedMasterSkillButton;
    public ToggleGroup ToggleGroup;
    public GameObject UnitContainer;
    public HorizontalLayoutGroup HorizontalLayoutGroup;

    MasterSkillButton[] MasterSkillButtons;

    void Start()
    {
        MasterSkillButtons = new MasterSkillButton[MasterSkillDatas.Length];
        for (int i = 0; i < MasterSkillDatas.Length; i++)
        {
            MasterSkillButton masterSkillButton = Instantiate<MasterSkillButton>(MasterSkillButtonPrefab);
            MasterSkillButtons[i] = masterSkillButton;
            masterSkillButton.transform.SetParent(HorizontalLayoutGroup.transform);
            masterSkillButton.Init(MasterSkillDatas[i]);
            masterSkillButton.Toggle.isOn = false;
            masterSkillButton.Toggle.group = ToggleGroup;
            masterSkillButton.Toggle.onValueChanged.AddListener((bool isOn) =>
            {
                if (isOn)
                {
                    SelectedMasterSkillButton = masterSkillButton;
                    UICanvas.Inst.ShowInfo(masterSkillButton.MasterSkillData.Name);
                    MasterSkillEvent(Types.MasterSkillEventType.Selected);
                }
                else if (SelectedMasterSkillButton == masterSkillButton)
                {
                    SelectedMasterSkillButton = null;
                    MasterSkillEvent(Types.MasterSkillEventType.Unselected);
                    UICanvas.Inst.HideInfo();
                }
                Debug.Log(ToggleGroup.ActiveToggles());
            });
        }
    }

    public void Deselect()
    {
        if(SelectedMasterSkillButton != null)
        {
            SelectedMasterSkillButton.Toggle.isOn = false;
        }
        UICanvas.Inst.HideInfo();
    }
}
