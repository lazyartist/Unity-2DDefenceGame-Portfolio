using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile_MagicBall : AProjectile
{
    private Animator _animator;
    private bool _isMoving = false;
    private GameObject _target;
    private Vector3 _lastTargetPosition;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_isMoving)
        {
            Vector3 targetPosition = _target == null ? _lastTargetPosition : _target.transform.position;

            Vector3 direction = targetPosition - transform.position;
            float distance = direction.magnitude;
            direction.Normalize();
            Vector3 move = direction * (AttackData.ProjectileSpeed * Time.deltaTime);
            if(move.magnitude >= distance || move.magnitude < 0.01f)
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

    override public void Init(AttackData attackData, AttackTargetData attackTargetData, GameObject target, Vector3 position)
    {
        AttackData = attackData;
        AttackTargetData = attackTargetData;
        _target = target;
        InitByPosition(position);
    }

    override public void InitByTarget(GameObject target)
    {
        _target = target;
        InitByPosition(target.transform.position);
    }

    override public void InitByPosition(Vector3 position)
    {
        _lastTargetPosition = position;
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
        if (_target != null)
        {
            _target.GetComponent<Unit>().TakeDamage(AttackData);
            transform.SetParent(_target.transform);
        }
    }

    void OnDrawGizmos()
    {
        if (_isMoving)
        {
            Vector3 targetPosition = _target == null ? _lastTargetPosition : _target.transform.position;
            Gizmos.DrawLine(targetPosition, transform.position);
        }

        if (AttackData != null) {
            Gizmos.DrawWireSphere(transform.position, AttackData.AttackRange);
        }
    }
}

