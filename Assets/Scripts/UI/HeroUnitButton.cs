using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeroUnitButton : MonoBehaviour/*, IPointerClickHandler*/
{
    public Image IconBg;
    public Image Icon;
    public Toggle Toggle;

    bool _enableToggleEvent = true;
    float _coolTime = 0f;
    Coroutine _coroutine_UpdateCoolTime;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        Toggle.isOn = false;
        Toggle.onValueChanged.AddListener((bool isOn) =>
        {
            if (_enableToggleEvent == false) return;

            if (Toggle.isOn)
            {
                Selector_HeroUnit selector = StageManager.Inst.StageInfo.HeroUnit.GetComponent<Selector_HeroUnit>();
                if (selector != null)
                {
                    SelectorManager.Inst.RegisterSelector(selector);
                }
            }
            else
            {
                SelectorManager.Inst.UnregisterSelector();
            }
        });

        IconBg.sprite = StageManager.Inst.StageData.HeroUnitIcon;
        Icon.sprite = StageManager.Inst.StageData.HeroUnitIcon;
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

            if (_coolTime >= StageManager.Inst.StageData.HeroUnitRespawnCoolTime)
            {
                Ready();
                break;
            }
        }

        yield break;
    }

    void UpdateIcon()
    {
        Icon.fillAmount = _coolTime / StageManager.Inst.StageData.HeroUnitRespawnCoolTime;
    }

    public void Select(bool isSelect)
    {
        _enableToggleEvent = false;
        Toggle.isOn = isSelect;
        _enableToggleEvent = true;
    }
}

