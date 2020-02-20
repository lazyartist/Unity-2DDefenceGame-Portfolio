using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MasterSkillButton : MonoBehaviour/*, IPointerClickHandler*/
{
    public MasterSkillData MasterSkillData;
    public Image IconBg;
    public Image Icon;
    public Sprite ButtonFrameSprite;
    public Sprite ButtonFrameActivateSprite;
    public Toggle Toggle;
    public float CoolTimeInterval = 0.05f;

    float _coolTime = 0f;
    Coroutine _coroutine_UpdateCoolTime;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(MasterSkillData masterSkillData)
    {
        MasterSkillData = masterSkillData;
        IconBg.sprite = masterSkillData.Icon;
        Icon.sprite = masterSkillData.Icon;
        //_coolTime = masterSkillData.CoolTime;
        Ready();
    }

    public void Ready()
    {
        Toggle.interactable = true;
        //_coolTime = MasterSkillData.CoolTime;
        if (_coroutine_UpdateCoolTime != null)
        {
            StopCoroutine(_coroutine_UpdateCoolTime);
        }
    }

    public void Reset()
    {
        Toggle.interactable = false;
        _coolTime = 0f;

        if (_coroutine_UpdateCoolTime != null)
        {
            StopCoroutine(_coroutine_UpdateCoolTime);
        }
        _coroutine_UpdateCoolTime = StartCoroutine(Coroutine_UpdateCoolTime());
    }

    public void OnClick()
    {
        if (Toggle.isOn)
        {
            UICanvas.Inst.ShowInfo(MasterSkillData.Name);
        }
        else
        {
            UICanvas.Inst.HideInfo();
        }
    }

    IEnumerator Coroutine_UpdateCoolTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(CoolTimeInterval);
            _coolTime += CoolTimeInterval;
            UpdateIcon();

            if (_coolTime >= MasterSkillData.CoolTime)
            {
                Ready();
                break;
            }
        }

        yield break;
    }

    void UpdateIcon()
    {
        Icon.fillAmount = _coolTime / MasterSkillData.CoolTime;
    }
}
