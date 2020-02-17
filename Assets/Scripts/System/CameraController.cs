using System.Collections;
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
                    Vector3 cameraPosition = _lastCameraPosition + swipeDistance;
                    Rect r = new Rect();
                    r.yMax = ValidMapAreaCenter.y + (ValidMapAreaSize.y * 0.5f - Camera.orthographicSize);
                    r.yMin = ValidMapAreaCenter.y - (ValidMapAreaSize.y * 0.5f - Camera.orthographicSize);
                    if (cameraPosition.y > r.yMax)
                    {
                        cameraPosition.y = r.yMax;
                    }
                    else if (cameraPosition.y < r.yMin)
                    {
                        cameraPosition.y = r.yMin;
                    }

                    Camera.transform.position = cameraPosition;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(ValidMapAreaCenter, new Vector3(ValidMapAreaSize.x, ValidMapAreaSize.y, 1f));
    }
}
