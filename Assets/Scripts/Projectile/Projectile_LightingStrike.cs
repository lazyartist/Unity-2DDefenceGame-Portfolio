using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile_LightingStrike : AProjectile
{
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void MoveToTarget()
    {
        transform.position = _targetPosition;
        _animator.SetTrigger("Hit");
        Hit();
    }

    void Hit()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, AttackData.AttackRange, AttackTargetData.AttackTargetLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];
            collider.GetComponent<Unit>().TakeDamage(AttackData);
        }
    }

    void OnDrawGizmos()
    {
        if (AttackData != null)
        {
            Gizmos.DrawWireSphere(transform.position, AttackData.AttackRange);
        }
    }
}