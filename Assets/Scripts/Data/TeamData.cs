using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeamData_", menuName = "SO/Create TeamData")]
public class TeamData : ScriptableObject {
    public Color[] TeamColors;
    public LayerMask[] TeamLayerMask;
    public LayerMask[] EnemyTeamLayerMask;
}
