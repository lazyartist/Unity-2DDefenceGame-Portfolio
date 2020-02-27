using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackTargetData_", menuName = "Create Data/AttackTargetData")]
public class AttackTargetData : ScriptableObject {
    public LayerMask LayerMask;
    public LayerMask AttackTargetLayerMask;
}
