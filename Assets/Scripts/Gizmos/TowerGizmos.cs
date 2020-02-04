using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGizmos : MonoBehaviour {

    private void OnDrawGizmos()
    {
        Tower tower = GetComponent<Tower>();

        if(tower != null)
        {
            // 타워 위치
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 0.05f);
        }
    }
}
