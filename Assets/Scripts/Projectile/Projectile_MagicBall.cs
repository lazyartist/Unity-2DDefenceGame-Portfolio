using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile_MagicBall : AProjectile
{
    private Animator _animator;
    private bool _isMoving = false;
    private Vector3 _lastTargetPosition;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_isMoving)
        {
            Vector3 targetPosition = (_targetUnit == null || _targetUnit.IsDied) ? _lastTargetPosition : _targetUnit.GetCenterPosition();
            _lastTargetPosition = targetPosition;

            Vector3 direction = targetPosition - transform.position;
            float distance = direction.magnitude;
            direction.Normalize();
            Vector3 move = direction * (AttackData.ProjectileSpeed * Time.deltaTime);
            if (move.magnitude >= distance || move.magnitude < 0.01f)
            {
                // 목표지점에 도착
                transform.position = targetPosition;
                _isMoving = false;
                Hit();
            }
            else
            {
                transform.position += move;

                // Angle
                float rad = Mathf.Atan2(direction.y, direction.x);
                transform.rotation = Quaternion.Euler(0f, 0f, rad * Mathf.Rad2Deg);
            }
        }
    }

    public override void MoveToTarget()
    {
        _lastTargetPosition = _targetPosition;
        _animator.SetTrigger("Charge");
        _isMoving = false;
    }

    void AniEvent_Fire()
    {
        _isMoving = true;
    }

    void Hit()
    {
        _animator.SetTrigger("Hit");
        if (_targetUnit != null)
        {
            _targetUnit.GetComponent<Unit>().TakeDamage(AttackData);
            transform.SetParent(_targetUnit.transform);
        }
    }

    void OnDrawGizmos()
    {
        if (_isMoving)
        {
            Vector3 targetPosition = _targetUnit == null ? _lastTargetPosition : _targetUnit.transform.position;
            Gizmos.DrawLine(targetPosition, transform.position);
        }

        if (AttackData != null)
        {
            Gizmos.DrawWireSphere(transform.position, AttackData.AttackRange);
        }
    }
}

