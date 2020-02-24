﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile_FireDrop : AProjectile
{
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    //override public void Init(AttackData attackData, AttackTargetData attackTargetData, GameObject target, Vector3 position)
    //{
    //    AttackData = attackData;
    //    AttackTargetData = attackTargetData;
    //    //_target = target;
    //    InitByPosition(position);
    //}

    //override public void InitByTarget(GameObject target)
    //{
    //    _animator.SetTrigger("Attack");
    //}

    //override public void InitByPosition(Vector3 position)
    //{
    //    _animator.SetTrigger("Attack");
    //}

    public override void MoveToTarget()
    {
        throw new System.NotImplementedException();
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

    void OnDrawGizmos()
    {
        if (AttackData != null)
        {
            Gizmos.DrawWireSphere(transform.position, AttackData.AttackRange); 
        }
    }
}