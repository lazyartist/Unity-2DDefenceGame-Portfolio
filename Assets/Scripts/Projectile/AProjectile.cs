using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProjectile : MonoBehaviour
{
    public AttackData AttackData;
    public AttackTargetData AttackTargetData;

    abstract public void Init(AttackData attackData, AttackTargetData attackTargetData, GameObject target, Vector3 position);
    abstract public void InitByTarget(GameObject target);
    abstract public void InitByPosition(Vector3 position);
}
