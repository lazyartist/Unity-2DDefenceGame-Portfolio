using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAndStageBinder : MonoBehaviour
{
    public Unit Unit;

    void Start()
    {
        Unit = GetComponent<Unit>();
        if (Unit != null)
        {
            Unit.UnitEvent += OnUnitEvent;
        }
    }

    void OnApplicationQuit()
    {
        CleanUp();
    }

    void OnDestroy()
    {
        CleanUp();
    }

    void CleanUp()
    {
        if (Unit != null)
        {
            Unit.UnitEvent -= OnUnitEvent;
            Unit = null;
        }
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
                if (unit.GoalComplete == false)
                {
                    StageManager.Inst.StageInfo.Gold += unit.UnitData.Gold;
                }
                CleanUp();
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
