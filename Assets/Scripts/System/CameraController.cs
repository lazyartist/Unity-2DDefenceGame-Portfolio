using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static Rect ValidMapAreaRect; // 맵 유효영역

    public Camera Camera;
    public Vector3 ValidMapAreaCenter;
    public Vector2 ValidMapAreaSize;

    public float MaxCameraSize = 5;
    public float MinCameraSize = 3;
    public float StartCameraSize = 5;
    public float MapPixelPerUnit = 100;
    public float ZoomSpeedMultiplierByTouch = 1f;

    float _targetCameraSize;
    float _cameraSizeRange;
    float _cameraSizeChangeLerpT;
    Vector3 _lastCameraPosition;
    Rect _validCameraArea;

    private void Awake()
    {
        _cameraSizeRange = MaxCameraSize - MinCameraSize;
        ValidMapAreaRect = new Rect(ValidMapAreaSize.x * 0.5f * -1f, ValidMapAreaSize.y * 0.5f * -1f, ValidMapAreaSize.x, ValidMapAreaSize.y);
    }

    virtual protected void Start()
    {
        InputManager.Inst.InputEvent += OnInputEvent_InputManager;
        _targetCameraSize = StartCameraSize;
        UpdateValidCameraArea();
        SetCameraPositionInValidArea();
    }

    void Update()
    {
        if (Camera.orthographicSize != _targetCameraSize)
        {
            Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize, _targetCameraSize, _cameraSizeChangeLerpT);
            if (Mathf.Abs(_targetCameraSize - Camera.orthographicSize) < 0.01)
            {
                Camera.orthographicSize = _targetCameraSize;
            }
            else
            {
                _cameraSizeChangeLerpT += 0.05f;
            }
            UpdateValidCameraArea();
            SetCameraPositionInValidArea();
        }
    }

    void OnInputEvent_InputManager(Types.InputEventType inputEventType, Vector3 value)
    {
        switch (inputEventType)
        {
            case Types.InputEventType.None:
                break;
            case Types.InputEventType.Down:
                _lastCameraPosition = Camera.transform.position;
                break;
            case Types.InputEventType.DownCanceled:
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
                ChangeTargetSize(value.x);
                break;
            case Types.InputEventType.ZoomByTouch:
                {
                    float deltaCameraSize = _cameraSizeRange * ZoomSpeedMultiplierByTouch * (value.x / Screen.width);
                    _targetCameraSize += deltaCameraSize;
                    _targetCameraSize = Mathf.Max(MinCameraSize, Mathf.Min(MaxCameraSize, _targetCameraSize));
                    _cameraSizeChangeLerpT = 1f;
                }

                break;
            default:
                break;
        }
    }

    void ChangeTargetSize(float value)
    {
        //_targetCameraSize += value;
        //_targetCameraSize = Mathf.Max(MinCameraSize, Mathf.Min(MaxCameraSize, _targetCameraSize));
        _targetCameraSize = value > 0f ? MinCameraSize : MaxCameraSize;
        _cameraSizeChangeLerpT = 0f;
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

    void OnDrawGizmos()
    {
        // 카메라 유효영역
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(ValidMapAreaCenter, new Vector3(ValidMapAreaSize.x, ValidMapAreaSize.y, 1f));
        // 맵 유효영역
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(new Vector3(CameraController.ValidMapAreaRect.xMin, CameraController.ValidMapAreaRect.yMax, 0f), new Vector3(CameraController.ValidMapAreaRect.xMax, CameraController.ValidMapAreaRect.yMin, 0f));
    }
}
