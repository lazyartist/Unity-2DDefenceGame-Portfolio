using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRenderOrder : MonoBehaviour
{
    public SpriteRenderer[] SpriteRenderers;
    Unit _unit;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = new Vector2(transform.position.x + CameraController.ValidMapAreaRect.xMin, transform.position.y + CameraController.ValidMapAreaRect.yMax);
        for (int i = 0; i < SpriteRenderers.Length; i++)
        {
            SpriteRenderer sp = SpriteRenderers[i];
            sp.sortingOrder = (int)(position.x * Consts.UnitRenderOrderPrecision) + (int)(position.y * -1f * Consts.UnitRenderOrderPrecision * 10) + i;
            //Debug.Log("sp.sortingOrder " + sp.sortingOrder);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3( CameraController.ValidMapAreaRect.xMin, CameraController.ValidMapAreaRect.yMax, 0f), new Vector3(CameraController.ValidMapAreaRect.xMax, CameraController.ValidMapAreaRect.yMin, 0f));
    }
}
