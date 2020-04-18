using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapManager : SingletonBase<MapManager>
{
    public SpriteRenderer MapSR;
    public SpriteRenderer MapMaskSR;
    public SpriteRenderer MapHighLightSR;
    public Material DefaultMaterial;
    public Material GrayscaleAndHighLightMaterial;

    Vector3 _mapLocalAxis; // unit
    Vector3 _uvMapPivot; // normalized
    Vector3 _mapSize; // unit

    private void Awake()
    {
        base.Awake();
        Init();
    }

    public void Init()
    {
        // MapSR.sprite.rect : pixel
        // MapSR.size : unit
        _mapSize = new Vector3(MapSR.sprite.rect.width / MapSR.sprite.pixelsPerUnit, MapSR.sprite.rect.height / MapSR.sprite.pixelsPerUnit, 0f);
        _uvMapPivot = new Vector3(MapSR.sprite.pivot.x / MapSR.sprite.rect.width  /*pixel*/, MapSR.sprite.pivot.y / MapSR.sprite.rect.height, 0f);
        _mapLocalAxis = new Vector3(_mapSize.x * _uvMapPivot.x, _mapSize.y * _uvMapPivot.y, 0f);
    }

    public bool IsMask(Vector3 worldPosition, Types.MapMaskChannelType colorChannelType, Color color)
    {
        Color maskColor = GetMaskColor(worldPosition);
        //Debug.Log("maskColor " + maskColor);
        return color[(int)colorChannelType] - maskColor[(int)colorChannelType] < Consts.ColorNearlyEqual;
    }

    public Color GetMaskColor(Vector3 worldPosition)
    {
        Vector3 xy = worldPosition + _mapLocalAxis;
        Vector3 uv = new Vector3(xy.x / _mapSize.x, xy.y / _mapSize.y, 0f); ;
        Color pixel = MapMaskSR.sprite.texture.GetPixelBilinear(uv.x, uv.y);
        return pixel;
    }

    public void SetHighLightWay(bool isSet)
    {
        MapSR.material = isSet ? GrayscaleAndHighLightMaterial : DefaultMaterial;
    }
}
