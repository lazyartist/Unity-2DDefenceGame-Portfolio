using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Unit.DispatchUnitEvent(Types.UnitEventType.AttackStart);
    }

    public void AniEvent_AttackFire()
    {
        Unit.DispatchUnitEvent(Types.UnitEventType.AttackFire);
    }

    public void AniEvent_AttackEnd()
    {
        Unit.DispatchUnitEvent(Types.UnitEventType.AttackEnd);
    }

    public void DiedComplete()
    {
        Unit.DispatchUnitEvent(Types.UnitEventType.DiedComplete);
    }

    public void Toward(Vector3 direction)
    {
        float rotationY = direction.x < 0 ? 180.0f : 0.0f;
        UnitBodyContainer.rotation = Quaternion.Euler(UnitBodyContainer.rotation.x, rotationY, UnitBodyContainer.rotation.z);
    }
}
