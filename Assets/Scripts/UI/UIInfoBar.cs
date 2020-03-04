using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoBar : MonoBehaviour
{
    public Text InfoText;
    public GameObject UIContainer;

    Selector _selector = null;

    void Start()
    {
    }

    void Update()
    {
        if (SelectorManager.Inst.CurSelector == null)
        {
            _selector = null;
            InfoText.text = "";
        }
        else if (SelectorManager.Inst.CurSelector == _selector)
        {
            // Selector가 변하지 않았다. 그냥 있는다.
        }
        else
        {
            _selector = SelectorManager.Inst.CurSelector;
            switch (_selector.InfoType)
            {
                case Types.InfoType.None:
                    break;
                case Types.InfoType.Unit:
                    {
                        Unit unit = _selector.GetComponent<Unit>();
                        InfoText.text = unit.ToString();
                    }
                    break;
                case Types.InfoType.Tower:
                    {
                        Tower tower = _selector.GetComponent<Tower>();
                        InfoText.text = tower.ToString();
                    }
                    break;
                case Types.InfoType.MasterSkill:
                    {
                        UIMasterSkillMenu uiMasterSkillMenu = _selector.GetComponent<UIMasterSkillMenu>();
                        InfoText.text = uiMasterSkillMenu.ToString();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
