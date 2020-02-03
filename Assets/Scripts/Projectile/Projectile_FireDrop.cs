using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile_FireDrop : ProjectileAbstract
{
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    override public void InitByTarget(GameObject target)
    {
        _animator.SetTrigger("Attack");
    }

    override public void InitByPosition(Vector3 position)
    {
        _animator.SetTrigger("Attack");
    }

    void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, AttackData.AttackRange, Consts.lmUnit);
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];
            if (collider.tag == Consts.tagUnit)
            {
                collider.GetComponent<Unit>().TakeDamage(AttackData);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AttackData.AttackRange);
    }
}