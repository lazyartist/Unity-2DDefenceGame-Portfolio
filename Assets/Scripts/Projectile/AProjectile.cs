using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProjectile : MonoBehaviour
{
    public AttackData AttackData;
    public AttackTargetData AttackTargetData;
    protected Unit _targetUnit;
    protected Vector3 _targetPosition;

    virtual public void Init(AttackData attackData, AttackTargetData attackTargetData, Unit targetUnit, Vector3 targetPosition)
    {
        AttackData = attackData;
        AttackTargetData = attackTargetData;
        _targetUnit = targetUnit;
        _targetPosition = targetPosition;
        MoveToTarget();
    }
    abstract public void MoveToTarget();
}
