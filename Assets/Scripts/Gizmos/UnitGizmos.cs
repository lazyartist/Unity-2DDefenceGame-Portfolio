using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGizmos : MonoBehaviour {

    private void OnDrawGizmos()
    {
        Unit unit = GetComponent<Unit>();

        if(unit != null)
        {
            Gizmos.color = unit.TeamType == Consts.TeamType.A ? unit.ATeamColor : unit.BTeamColor;
            Gizmos.DrawWireSphere(transform.position, unit.UnitData.TargetRange);
        }
    }
}
