﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera Camera;
    public Vector3 ValidMapAreaCenter;
    public Vector2 ValidMapAreaSize;

    public float MaxCameraSize = 5;
    public float MinCameraSize = 3;
    public float MapPixelPerUnit = 100;

    float _targetCameraSize;
    float _cameraSizeChangeT;
    Vector3 _lastCameraPosition;
    Rect _validCameraArea;

    virtual protected void Start()
    {
        InputManager.Inst.InputEvent += _OnInputEvent_InputManager;
        _targetCameraSize = Camera.orthographicSize;
        UpdateValidCameraArea();
        SetCameraPositionInValidArea();
    }

    private void Update()
    {
        if (Camera.orthographicSize != _targetCameraSize)
        {
            Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize, _targetCameraSize, _cameraSizeChangeT);
            if (Mathf.Abs(_targetCameraSize - Camera.orthographicSize) < 0.01)
            {
                Camera.orthographicSize = _targetCameraSize;
            }
            else
            {
                _cameraSizeChangeT += 0.05f;
            }
            UpdateValidCameraArea();
            SetCameraPositionInValidArea();
        }
    }

    private void _OnInputEvent_InputManager(Types.InputEventType inputEventType, Vector3 value)
    {
        switch (inputEventType)
        {
            case Types.InputEventType.None:
                break;
            case Types.InputEventType.Down:
                _lastCameraPosition = Camera.transform.position;
                break;
            case Types.InputEventType.Up:
                break;
            case Types.InputEventType.Swipe:
                {
                    Vector3 swipeDistance = (value / MapPixelPerUnit) * (Camera.orthographicSize / MaxCameraSize) * -1;
                    Camera.transform.position = _lastCameraPosition + swipeDistance;
                    UpdateValidCameraArea();
                    SetCameraPositionInValidArea();
                }
                break;
            case Types.InputEventType.Zoom:
                _targetCameraSize = (value.x > 0) ? MinCameraSize : MaxCameraSize;
                _cameraSizeChangeT = 0f;
                break;
            default:
                break;
        }
    }

    void UpdateValidCameraArea()
    {
        Vector2 validCameraSize = new Vector2(ValidMapAreaSize.x * 0.5f - (Camera.orthographicSize * Camera.aspect), ValidMapAreaSize.y * 0.5f - Camera.orthographicSize);
        _validCameraArea.yMax = ValidMapAreaCenter.y + validCameraSize.y;
        _validCameraArea.yMin = ValidMapAreaCenter.y - validCameraSize.y;
        _validCameraArea.xMin = ValidMapAreaCenter.x - validCameraSize.x;
        _validCameraArea.xMax = ValidMapAreaCenter.x + validCameraSize.x;
    }

    void SetCameraPositionInValidArea()
    {
        Vector3 cameraPosition = Camera.transform.position;
        if (cameraPosition.y > _validCameraArea.yMax)
        {
            cameraPosition.y = _validCameraArea.yMax;
        }
        else if (cameraPosition.y < _validCameraArea.yMin)
        {
            cameraPosition.y = _validCameraArea.yMin;
        }
        if (cameraPosition.x > _validCameraArea.xMax)
        {
            cameraPosition.x = _validCameraArea.xMax;
        }
        else if (cameraPosition.x < _validCameraArea.xMin)
        {
            cameraPosition.x = _validCameraArea.xMin;
        }
        Camera.transform.position = cameraPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(ValidMapAreaCenter, new Vector3(ValidMapAreaSize.x, ValidMapAreaSize.y, 1f));
    }
}