using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFSM : MonoBehaviour
{
    public AUnitState[] UnitStates;
    public AUnitState FirstUnitState;
    public AUnitState CurUnitState;
    //public Types.UnitFSMType CurUnitFSMType;
    public Unit Unit;

    private void Awake()
    {
        //for (int i = 0; i < UnitStates.Length; i++)
        //{
        //    AUnitState aUnitState = UnitStates[i];
        //    aUnitState.enabled = CurUnitState == aUnitState;
        //    aUnitState.UnitFSMType = (Types.UnitFSMType)i;
        //    aUnitState.Init(Unit, UnitStates);
        //}
    }

    void Start()
    {
        for (int i = 0; i < UnitStates.Length; i++)
        {
            AUnitState aUnitState = UnitStates[i];
            aUnitState.enabled = CurUnitState == aUnitState;
            aUnitState.UnitFSMType = (Types.UnitFSMType)i;
            aUnitState.Init(Unit, UnitStates);
        }

        CurUnitState.EnterState();
    }

    void Update()
    {
        AUnitState unitState = null;
        if (Unit.IsDied == true)
        {
            // 유닛 사망
            unitState = UnitStates[(int)Types.UnitFSMType.Died];
        }
        else if (Unit.TakenCCData.CCType == Types.CCType.Stun && CurUnitState != UnitStates[(int)Types.UnitFSMType.Hurt])
        {
            // 데미지 받음
            unitState = UnitStates[(int)Types.UnitFSMType.Hurt];
        }

        if (unitState == null || unitState == CurUnitState)
        {
            unitState = CurUnitState.UpdateState();
        }

        if (unitState != null && unitState != CurUnitState)
        {
            Transit(unitState);
        }
    }

    public void Reset()
    {
        CurUnitState = FirstUnitState;
    }

    // 상태 전환
    public void Transit(Types.UnitFSMType unitFSMType)
    {
        AUnitState unitState = UnitStates[(int)unitFSMType];
        if (CurUnitState != unitState)
        {
            Transit(unitState);
        }
    }

    void Transit(AUnitState unitState)
    {
        CurUnitState.ExitState();
        CurUnitState.enabled = false;
        unitState.PrevUnitState = CurUnitState;

        CurUnitState = unitState;
        CurUnitState.enabled = true;
        CurUnitState.EnterState();
    }

    public void ClearnUp()
    {
        CurUnitState.ExitState();
    }
}
