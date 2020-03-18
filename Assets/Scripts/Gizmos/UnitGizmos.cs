using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGizmos : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Unit unit = GetComponent<Unit>();

        if (unit != null)
        {
            // 유닛 위치
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 0.05f);
            // 유닛 공격 타겟 범위
            Gizmos.color = unit.TeamData.TeamColor;
            if (unit.UnitCenter != null)
            {
                Gizmos.DrawWireSphere(unit.UnitCenter.transform.position, unit.GetTargetRange(Types.UnitTargetRangeType.Short));
                Gizmos.DrawWireSphere(unit.UnitCenter.transform.position, unit.GetTargetRange(Types.UnitTargetRangeType.Long));
            }
            else
            {
                Gizmos.DrawWireSphere(unit.UnitCenter.transform.position, unit.GetTargetRange(Types.UnitTargetRangeType.Short));
                Gizmos.DrawWireSphere(unit.UnitCenter.transform.position, unit.GetTargetRange(Types.UnitTargetRangeType.Long));
            }
            // 투사체 생성 지점
            if (unit.SpawnPosition != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(unit.SpawnPosition.transform.position, 0.05f);
            }
            // 유닛의 공격 타겟까지 선 그리기
            if (unit.HasEnemyUnit())
            {
                Gizmos.color = unit.TeamData.TeamColor;
                Gizmos.DrawLine(unit.GetCenterPosition(), unit.EnemyUnit.GetCenterPosition());
            }
            // 랠리포인트
            if (unit.UnitMovePoint.UnitMovePointType == Types.UnitMovePointType.RallyPoint)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(unit.transform.position + unit.UnitMovePoint.RallyPoint, 0.05f);
                Gizmos.DrawLine(unit.transform.position, unit.UnitMovePoint.RallyPoint); 
            }
            // 자식 유닛 랠리포인트
            ChildUnitCreator childUnitCreator = unit.GetComponent<ChildUnitCreator>();
            if (childUnitCreator != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(childUnitCreator.transform.position + childUnitCreator.RallyPointInLocal, 0.05f);
                Gizmos.DrawLine(transform.position, childUnitCreator.transform.position + childUnitCreator.RallyPointInLocal);
            }
        }
    }
}
