using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyUnitZone : MonoBehaviour
{
    public BoxCollider2D BoxCollider2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit unit = collision.GetComponent<Unit>();
        if (unit != null)
        {
            unit.Die();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(gameObject.transform.position + new Vector3(BoxCollider2D.offset.x, BoxCollider2D.offset.y, 0f), BoxCollider2D.size);
    }
}
