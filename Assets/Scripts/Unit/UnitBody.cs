using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBody : MonoBehaviour
{
    public Consts.UnitEventListener UnitEventListener;

    public Transform UnitBodyContainer;
    public float DirectionX = 1.0f;

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
        UnitEventListener(Consts.UnitEventType.Attack);
    }

    public void DiedComplete()
    {
        UnitEventListener(Consts.UnitEventType.DiedComplete);
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
