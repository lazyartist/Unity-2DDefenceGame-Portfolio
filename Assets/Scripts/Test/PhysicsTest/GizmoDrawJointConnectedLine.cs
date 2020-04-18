using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoDrawJointConnectedLine : MonoBehaviour {
    public Joint2D Joint2D;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, Joint2D.connectedBody.transform.position);
    }
}
