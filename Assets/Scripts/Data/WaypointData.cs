using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaypointData_", menuName = "Create Data/WaypointData")]
public class WaypointData : ScriptableObject {
    public int SubPositionCount;
    public float SubPositionRadius = 1f;
    public Color GizmoColor;
    public Color[] SubPositionColors;
    public string LayerName;
}
