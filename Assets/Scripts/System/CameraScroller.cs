using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroller : MonoBehaviour {
    public Camera Camera;

    Vector3 _lastCameraPosition;

    virtual protected void Start()
    {
        InputManager.Inst.MouseEvent += _OnMouseEvent_InputManager;
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
                    Vector3 swipeDistance = value / 100f * -1;
                    Camera.transform.position = _lastCameraPosition + swipeDistance;
                }
                break;
            default:
                break;
        }
    }
}
