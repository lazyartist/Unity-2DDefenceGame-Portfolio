using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRenderOrder : MonoBehaviour
{
    public SpriteRenderer[] SpriteRenderers;
    Unit _unit;

    // Update is called once per frame
    void Update()
    {
        CalcRenderOrder();
    }

    public void CalcRenderOrder()
    {
        Vector2 position = new Vector2(transform.position.x + CameraController.ValidMapAreaRect.xMin, transform.position.y + CameraController.ValidMapAreaRect.yMax);
        for (int i = 0; i < SpriteRenderers.Length; i++)
        {
            SpriteRenderer sp = SpriteRenderers[i];
            sp.sortingOrder = (int)(position.x * Consts.UnitRenderOrderPrecision) + (int)(position.y * -1f * Consts.UnitRenderOrderPrecision * 10) + i;
            //Debug.Log("sp.sortingOrder " + sp.sortingOrder);
        }
    }
}
