using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera Camera;
    public float MaxCameraSize = 5;
    public float MinCameraSize = 3;
    public float MapPixelPerUnit = 100;

    float _targetCameraSize;
    float _cameraSizeChangeT;
    Vector3 _lastCameraPosition;

    virtual protected void Start()
    {
        InputManager.Inst.MouseEvent += _OnMouseEvent_InputManager;
        _targetCameraSize = Camera.orthographicSize;
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
        }
    }

    private void _OnMouseEvent_InputManager(Types.MouseEventType mouseEventType, Vector3 value)
    {
        switch (mouseEventType)
        {
            case Types.MouseEventType.None:
                break;
            case Types.MouseEventType.Down:
                _lastCameraPosition = Camera.transform.position;
                break;
            case Types.MouseEventType.Up:
                break;
            case Types.MouseEventType.Swipe:
                {
                    Vector3 swipeDistance = (value / MapPixelPerUnit) * (Camera.orthographicSize / MaxCameraSize) * -1;
                    Camera.transform.position = _lastCameraPosition + swipeDistance;
                }
                break;
            case Types.MouseEventType.Zoom:
                _targetCameraSize = (value.x > 0) ? MinCameraSize : MaxCameraSize;
                _cameraSizeChangeT = 0f;
                break;
            default:
                break;
        }
    }
}
