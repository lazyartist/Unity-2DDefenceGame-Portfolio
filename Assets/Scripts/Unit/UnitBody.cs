using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo UnitBody -> UnitSprite
public class UnitBody : MonoBehaviour
{
    public Unit Unit;
    public Transform UnitBodyContainer;
    [HideInInspector]
    public Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }
    // todo Attack -> AniEvent_Attack
    public void Attack()
    {
        Unit.DispatchUnitEvent(Types.UnitEventType.Attack, Unit);
    }
    public void AttackStart()
    {
        Debug.Log("AttackStart");
        Unit.DispatchUnitEvent(Types.UnitEventType.AttackStart, Unit);
    }
    public void AttackEnd()
    {
        Debug.Log("AttackEnd");
        Unit.DispatchUnitEvent(Types.UnitEventType.AttackEnd, Unit);
    }

    public void DiedComplete()
    {
        Unit.DispatchUnitEvent(Types.UnitEventType.DiedComplete, Unit);
    }

    public void Toward(Vector3 direction)
    {
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
