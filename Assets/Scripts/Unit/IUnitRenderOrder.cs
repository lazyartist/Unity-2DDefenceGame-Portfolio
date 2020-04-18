using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitRenderOrder
{
    void Init(string sortingLayerName);
    void CalcRenderOrder();
}
