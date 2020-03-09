using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManagerGizmos : MonoBehaviour
{
    void OnDrawGizmos()
    {
        StageManager sp = GetComponent<StageManager>();
        if(sp != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(sp.StageData.FirstHeroRallyPoint, 0.05f);
        }
    }
}
