using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileAbstract : MonoBehaviour
{
    public AttackData AttackData;
    public AttackTargetData AttackTargetData;

    abstract public void InitByTarget(GameObject target);
    abstract public void InitByPosition(Vector3 position);
}
