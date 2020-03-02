using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapManager : SingletonBase<MapManager>
{
    public SpriteRenderer MapSR;
    public SpriteRenderer MapMaskSR;

    Vector3 _mapMaskPivot;

    private void Awake()
    {
        base.Awake();
        Init();
    }

    public void Init()
    {
        _mapMaskPivot = new Vector3(MapMaskSR.sprite.pivot.x, MapMaskSR.sprite.pivot.y, 0f);
    }

    public bool IsMask(Vector3 worldPosition, Types.MapMaskChannelType colorChannelType, Color color)
    {
        Color maskColor = GetMaskColor(worldPosition);
        //Debug.Log("maskColor " + maskColor);
        return maskColor[(int)colorChannelType] == color[(int)colorChannelType];
    }

    public Color GetMaskColor(Vector3 worldPosition)
    {
        Vector3 xyOnMapMaskSR = _mapMaskPivot + ((worldPosition - MapMaskSR.transform.position) * (MapMaskSR.sprite.pixelsPerUnit / MapMaskSR.gameObject.transform.localScale.x));
        return MapMaskSR.sprite.texture.GetPixel((int)xyOnMapMaskSR.x, (int)xyOnMapMaskSR.y);
    }
}
