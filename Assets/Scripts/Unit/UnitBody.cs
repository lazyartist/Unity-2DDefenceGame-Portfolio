using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBody : MonoBehaviour
{
    public Unit Unit;
    public Transform UnitBodyContainer;

    [HideInInspector]
    public SpriteRenderer SpriteRenderer;
    [HideInInspector]
    public Animator Animator;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
    }

    //public void PlayAttack()
    //{
    //    Animator.SetTrigger("Attack");
    //}

    public void Attack()
    {
        Unit.Attack();
    }

    public void DiedComplete()
    {
        Unit.DiedComplete();
    }

    public void Toward(bool left)
    {
        if (left)
        {
            UnitBodyContainer.rotation = Quaternion.Euler(UnitBodyContainer.rotation.x, 180f, UnitBodyContainer.rotation.z);
            //UnitBody.transform.localPosition = new Vector3(Mathf.Abs(UnitBody.transform.localPosition.x), UnitBody.transform.localPosition.y, UnitBody.transform.localPosition.z);
        }
        else
        {
            UnitBodyContainer.rotation = Quaternion.Euler(UnitBodyContainer.rotation.x, 0f, UnitBodyContainer.rotation.z);
            //transform.rotation = Quaternion.Euler(UnitBody.transform.rotation.x, 0f, UnitBody.transform.rotation.z);
            //UnitBody.transform.localPosition = new Vector3(-Mathf.Abs(UnitBody.transform.localPosition.x), UnitBody.transform.localPosition.y, UnitBody.transform.localPosition.z);
        }
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
