using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeamData_", menuName = "SO/Create TeamData")]
public class TeamData : ScriptableObject {
    public Types.TeamType TeamType;
    public Color TeamColor;
}
