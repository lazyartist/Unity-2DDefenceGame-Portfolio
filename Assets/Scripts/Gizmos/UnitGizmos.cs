using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGizmos : MonoBehaviour {

    private void OnDrawGizmos()
    {
        Unit unit = GetComponent<Unit>();

        if(unit != null)
        {
            // 유닛 위치
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 0.05f);
            // 유닛 공격 타겟 범위
            Gizmos.color = unit.TeamType == Consts.TeamType.A ? unit.ATeamColor : unit.BTeamColor;
            Gizmos.DrawWireSphere(transform.position, unit.UnitData.TargetRange);
            // 유닛 공격 범위
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + unit.AttackArea.offset, unit.AttackArea.size);
        }
    }
}
