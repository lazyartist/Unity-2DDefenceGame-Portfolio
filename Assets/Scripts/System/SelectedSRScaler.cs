using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSRScaler : MonoBehaviour {
    public GameObject UnitContainer;
    public BoxCollider2D BoxCollider2D;

    public Vector2 PositionOffset = new Vector2(0f, 0.03f);
    public Vector2 ScaleOffset = new Vector2(1.5f, 0.35f);

    void Start () {
        transform.localPosition = new Vector3(UnitContainer.transform.localPosition.x * -1f + PositionOffset.x, UnitContainer.transform.localPosition.y * -1f + PositionOffset.y, 0);
        transform.localScale = new Vector3(BoxCollider2D.size.x * ScaleOffset.x, ScaleOffset.y, 1f);
    }
}
