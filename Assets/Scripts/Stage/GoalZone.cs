using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZone : MonoBehaviour
{
    public BoxCollider2D BoxCollider2D;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Unit unit = collision.GetComponent<Unit>();
        if (unit != null)
        {
            unit.GoalComplete = true;
            StageManager.Inst.StageInfo.Health -= unit.UnitData.StageHealthDamage;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(gameObject.transform.position + new Vector3(BoxCollider2D.offset.x, BoxCollider2D.offset.y, 0f), BoxCollider2D.size);
    }
}
