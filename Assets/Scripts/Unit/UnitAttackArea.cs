using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// todo deprecated UnitAttackArea
public class UnitAttackArea : MonoBehaviour {
    public Unit ParentUnit;
    public Unit TargetUnit;
    public bool Attackable { get; private set; }

    private BoxCollider2D collider2d;

    private void Awake()
    {
        collider2d = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(ParentUnit.AttackTargetUnit == null)
        {
            Attackable = false;
            TargetUnit = null;
        }
        else if(ParentUnit.AttackTargetUnit.gameObject == collision.gameObject)
        {
            Attackable = true;

            // todo 추후 제거
            TargetUnit = collision.gameObject.GetComponent<Unit>();
        }

        //Debug.Log("OnTriggerEnter2D UnitAttackArea " + Attackable);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ParentUnit.AttackTargetUnit == null)
        {
            Attackable = false;
            TargetUnit = null;
        }
        else if (ParentUnit.AttackTargetUnit.gameObject == collision.gameObject)
        {
            Attackable = true;

            // todo 추후 제거
            TargetUnit = collision.gameObject.GetComponent<Unit>();
        }

        //Debug.Log("OnTriggerEnter2D UnitAttackArea " + Attackable);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ParentUnit.AttackTargetUnit == null)
        {
            Attackable = false;
            TargetUnit = null;
        }
        else if(ParentUnit.AttackTargetUnit.gameObject == collision.gameObject)
        {
            Attackable = false;
            TargetUnit = null;
        }

        //Debug.Log("OnTriggerExit2D UnitAttackArea");
    }
}
