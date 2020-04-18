using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 아무것도 하지 않는 UnitRenderOrder 클래스
// 부모 유닛이 sortingOrder를 결정할 경우 사용
public class UnitRenderOrder_None : MonoBehaviour, IUnitRenderOrder
{
    public void Init(string sortingLayerName) { }
    public void CalcRenderOrder() { }
}
