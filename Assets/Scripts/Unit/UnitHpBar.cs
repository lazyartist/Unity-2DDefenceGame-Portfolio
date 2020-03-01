using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHpBar : MonoBehaviour
{
    public Unit Unit;
    public SpriteRenderer HpBarBgSR;
    public SpriteRenderer HpBarGaugeSR;

    float _hpBarWidth = 0f;

    void Start()
    {
        HpBarGaugeSR.color = Unit.TeamData.TeamColor;
        Unit.UnitEvent += OnUnitEvent;

        _hpBarWidth = Unit.ColliderSize.x;
        HpBarBgSR.transform.localScale = new Vector3(_hpBarWidth, 1f, 1f);
        HpBarGaugeSR.transform.localScale = new Vector3(_hpBarWidth, 1f, 1f);

        this.transform.localPosition = new Vector3(_hpBarWidth * -0.5f, Unit.ColliderOffset.y + Unit.ColliderSize.y * 0.5f + 0.1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        HpBarGaugeSR.transform.localScale = new Vector3(_hpBarWidth * Unit.Health / Unit.UnitData.Health, 1f, 1f);
        //HpBarGaugeSR.size = new Vector2(HpBarBgSR.size.x * Unit.Health / Unit.UnitData.Health, HpBarGaugeSR.size.y);
    }

    void OnUnitEvent(Types.UnitEventType unitEventType, Unit unit)
    {
        switch (unitEventType)
        {
            case Types.UnitEventType.None:
                break;
            case Types.UnitEventType.AttackStart:
                break;
            case Types.UnitEventType.AttackEnd:
                break;
            case Types.UnitEventType.AttackFire:
                break;
            case Types.UnitEventType.Die:
                this.gameObject.SetActive(false);
                break;
            case Types.UnitEventType.DiedComplete:
                break;
            case Types.UnitEventType.AttackStopped:
                break;
            default:
                break;
        }
    }
}
