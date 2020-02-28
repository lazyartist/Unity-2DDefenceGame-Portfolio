using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeamData_", menuName = "Create Data/TeamData")]
public class TeamData : ScriptableObject {
    public Types.TeamType TeamType;
    public Types.TeamType EnemyTeamType;
    public Color TeamColor;
}
