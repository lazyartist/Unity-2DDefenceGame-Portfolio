using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MasterSkillData_", menuName = "SO/Create MasterSkillData")]
public class MasterSkillData : ScriptableObject {
    public Sprite Icon;
    public float CoolTime;
    public string Name;
    public AttackData AttackData;
}
