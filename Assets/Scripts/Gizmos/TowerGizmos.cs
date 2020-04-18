using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGizmos : MonoBehaviour {

    void OnDrawGizmos()
    {
        TowerBuilder tower = GetComponent<TowerBuilder>();

        if(tower != null)
        {
            // 타워 가운데 위치
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 0.05f);
            // 타워 타겟 범위
            //Gizmos.color = tower.TeamData.TeamColor;
            //Gizmos.DrawWireSphere(transform.position, tower.UnitData.TargetRange);
        }
    }
}
