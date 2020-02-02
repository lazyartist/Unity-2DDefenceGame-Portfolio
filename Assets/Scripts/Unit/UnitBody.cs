using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBody : MonoBehaviour
{
    public Types.UnitEventListener UnitBodyEventListener;

    public Transform UnitBodyContainer;
    public float DirectionX = 1.0f;
    public bool PlayingAttackAni = false;

    [HideInInspector]
    public SpriteRenderer SpriteRenderer;
    [HideInInspector]
    public Animator Animator;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        UnitBodyEventListener(Types.UnitBodyEventType.Attack);
    }
    public void AttackStart()
    {
        Debug.Log("AttackStart");
        PlayingAttackAni = true;
    }
    public void AttackEnd()
    {
        Debug.Log("AttackEnd");
        PlayingAttackAni = false;
    }

    public void DiedComplete()
    {
        UnitBodyEventListener(Types.UnitBodyEventType.DiedComplete);
    }

    public void Toward(Vector3 direction)
    {
        DirectionX = direction.x < 0 ? 1.0f : 0f;
        float rotationY = direction.x < 0 ? 180.0f : 0.0f;
        UnitBodyContainer.rotation = Quaternion.Euler(UnitBodyContainer.rotation.x, rotationY, UnitBodyContainer.rotation.z);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Unit.OnTriggerEnter2DByUnitBody(collision);
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Unit.OnTriggerStay2DByUnitBody(collision);
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    Unit.OnTriggerExit2DByUnitBody(collision);
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("OnTriggerEnter2D UnitBody");
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log("OnTriggerStay2D UnitBody");
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    Debug.Log("OnTriggerExit2D UnitBody");
    //}
}
