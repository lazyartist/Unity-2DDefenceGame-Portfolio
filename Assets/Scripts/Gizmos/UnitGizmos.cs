using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGizmos : MonoBehaviour {

    void OnDrawGizmos()
    {
        Unit unit = GetComponent<Unit>();

        if(unit != null)
        {
            // 유닛 위치
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 0.05f);
            // 유닛 공격 타겟 범위
            Gizmos.color = unit.TeamData.TeamColor;
            Gizmos.DrawWireSphere(transform.position + unit.UnitCenterOffset, unit.UnitData.TargetRange);
            // 투사체 생성 지점
            if(unit.SpawnPosition != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(unit.SpawnPosition.transform.position, 0.05f);
            }
            // 랠리포인트
            ChildUnits childUnits = unit.GetComponent<ChildUnits>();
            if (childUnits != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(childUnits.RallyPoint.transform.position, 0.05f);
            }
        }
    }
}
