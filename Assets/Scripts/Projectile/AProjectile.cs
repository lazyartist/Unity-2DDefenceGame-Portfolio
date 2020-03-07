using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProjectile : MonoBehaviour
{
    public TeamData TeamData;
    public AttackData AttackData;

    protected Unit _targetUnit;
    protected Vector3 _targetCenterPosition;
    protected Vector3 _targetBottomPosition;
    protected LayerMask _targetLayerMask;

    virtual public void Init(TeamData teamData, AttackData attackData, Unit targetUnit, Vector3 targetPosition)
    {
        TeamData = teamData;
        AttackData = attackData;
        _targetUnit = targetUnit;
        if(_targetUnit == null)
        {
            _targetCenterPosition = targetPosition;
            _targetBottomPosition = targetPosition;
        }
        else
        {
            _targetCenterPosition = _targetUnit.GetCenterPosition();
            _targetBottomPosition = _targetUnit.transform.position;
        }

        _targetLayerMask = 0;
        for (int i = 0; i < AttackData.TargetUnitTypes.Length; i++)
        {
            _targetLayerMask |= LayerMask.GetMask(TeamData.EnemyTeamType.ToString() + AttackData.TargetUnitTypes[i].ToString());
        }

        MoveToTarget();
    }
    abstract public void MoveToTarget();
}
