using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile_LightingStrike : AProjectile
{
    public override void MoveToTarget()
    {
        transform.position = _targetCenterPosition;
        _animator.SetTrigger("Hit");
        AudioManager.Inst.PlayAttackStart(AttackData);
        Hit();
    }

    void Hit()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, AttackData.AttackRange, _targetLayerMask);
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