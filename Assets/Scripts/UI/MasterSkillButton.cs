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
    public Toggle Toggle;

    float _coolTime = 0f;
    Coroutine _coroutine_UpdateCoolTime;

    public void Init(MasterSkillData masterSkillData)
    {
        MasterSkillData = masterSkillData;
        IconBg.sprite = masterSkillData.Icon;
        Icon.sprite = masterSkillData.Icon;
        Ready();
    }

    public void Ready()
    {
        Toggle.interactable = true;
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

    IEnumerator Coroutine_UpdateCoolTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(Consts.CoolTimeUpdateInterval);
            _coolTime += Consts.CoolTimeUpdateInterval;
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
