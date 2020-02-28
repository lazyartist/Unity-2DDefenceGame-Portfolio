using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProjectile : MonoBehaviour
{
    public TeamData TeamData;
    public AttackData AttackData;

    protected Unit _targetUnit;
    protected Vector3 _targetPosition;
    protected LayerMask _targetLayerMask;

    virtual public void Init(TeamData teamData, AttackData attackData, Unit targetUnit, Vector3 targetPosition)
    {
        TeamData = teamData;
        AttackData = attackData;
        _targetUnit = targetUnit;
        _targetPosition = targetPosition;

        _targetLayerMask = 0;
        for (int i = 0; i < AttackData.TargetUnitTypes.Length; i++)
        {
            _targetLayerMask |= LayerMask.GetMask(TeamData.EnemyTeamType.ToString() + AttackData.TargetUnitTypes[i].ToString());
        }

        MoveToTarget();
    }
    abstract public void MoveToTarget();
}
