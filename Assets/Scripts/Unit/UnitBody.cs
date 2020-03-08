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
    public SpriteRenderer UnitSR;

    Vector3 _firstLocalPosition;

    void Awake()
    {
        Animator = GetComponent<Animator>();

        _firstLocalPosition = transform.localPosition;
    }

    public void Reset()
    {
        transform.localPosition = _firstLocalPosition;
    }

    public void AniEvent_AttackStart()
    {
        Debug.Log("AniEvent_AttackStart");
        Unit.DispatchUnitEvent(Types.UnitEventType.AttackStart, Unit);
    }

    public void AniEvent_AttackFire()
    {
        Unit.DispatchUnitEvent(Types.UnitEventType.AttackFire, Unit);
    }

    public void AniEvent_AttackEnd()
    {
        Debug.Log("AniEvent_AttackEnd");
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

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Unit.OnTriggerEnter2DByUnitBody(collision);
    //}

    //void OnTriggerStay2D(Collider2D collision)
    //{
    //    Unit.OnTriggerStay2DByUnitBody(collision);
    //}

    //void OnTriggerExit2D(Collider2D collision)
    //{
    //    Unit.OnTriggerExit2DByUnitBody(collision);
    //}

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("OnTriggerEnter2D UnitBody");
    //}

    //void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log("OnTriggerStay2D UnitBody");
    //}

    //void OnTriggerExit2D(Collider2D collision)
    //{
    //    Debug.Log("OnTriggerExit2D UnitBody");
    //}
}
