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
            Unit.UnitEvent += _OnUnitEvent;
        }
    }

    private void OnApplicationQuit()
    {
        CleanUp();
    }

    private void OnDestroy()
    {
        CleanUp();
    }

    void CleanUp()
    {
        if (Unit != null)
        {
            Unit.UnitEvent -= _OnUnitEvent;
            Unit = null;
        }
    }

    void _OnUnitEvent(Types.UnitEventType unitEventType, Unit unit)
    {
        switch (unitEventType)
        {
            case Types.UnitEventType.None:
                break;
            case Types.UnitEventType.AttackStart:
                break;
            case Types.UnitEventType.AttackEnd:
                break;
            case Types.UnitEventType.Attack:
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
