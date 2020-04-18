using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMiniMap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RawImage MinimapRawImage;
    public Image MiniMapCameraRectImage;
    public Text CameraMagnificationText;
    public float MiniMapCameraRectMoveSpeedMultipier = 1f;

    CameraManager _cameraManager;
    Camera _camera;
    Camera _miniMapCamera;
    float _miniMapSizeMultifier;
    bool _isPointerDown;
    Vector3 _pointerDownPosition;
    Vector3 _miniMapScreenAxis;
    // todo MinimapRawImage.rectTransform.rect

    void Start()
    {
        CameraManager.Inst.CameraEvent += OnCameraEvent;
        _cameraManager = CameraManager.Inst;
        _camera = CameraManager.Inst.Camera;
        _miniMapCamera = CameraManager.Inst.MinimapCamera;

        // 미니맵 카메라와 동일한 비율로 미니맵의 크기를 조정한다
        float miniMapCameraHeight = CameraManager.Inst.MinimapCamera.orthographicSize * 2f;
        float miniMapCameraWidth = miniMapCameraHeight * CameraManager.Inst.MinimapCamera.aspect;
        MinimapRawImage.rectTransform.sizeDelta = new Vector2(MinimapRawImage.rectTransform.rect.width, MinimapRawImage.rectTransform.rect.width * miniMapCameraHeight / miniMapCameraWidth);
        _miniMapSizeMultifier = MinimapRawImage.rectTransform.rect.height / _cameraManager.ValidMapAreaSize.y;
        _miniMapScreenAxis = MinimapRawImage.rectTransform.position;
    }

    void Update()
    {
        if (_isPointerDown)
        {
            // 미니맵 위에서의 좌표를 좌하~우상:(0,0)~(1,1) 사이의 값으로 구한다.
            Vector3 pointerDownPosition = _pointerDownPosition - _miniMapScreenAxis;
            pointerDownPosition = new Vector3(pointerDownPosition.x / MinimapRawImage.rectTransform.rect.width, pointerDownPosition.y / MinimapRawImage.rectTransform.rect.height, 0f);
            Vector3 pointerPosition = Input.mousePosition - _miniMapScreenAxis;
            pointerPosition = new Vector3(pointerPosition.x / MinimapRawImage.rectTransform.rect.width, pointerPosition.y / MinimapRawImage.rectTransform.rect.height, 0f);
            // 뷰포트를 통해 월드 좌표를 얻어온다.
            Vector3 worldPointerDownPosition = _miniMapCamera.ViewportToWorldPoint(pointerDownPosition);
            Vector3 worldPointerPosition = _miniMapCamera.ViewportToWorldPoint(pointerPosition);

            InputManager.Inst.DispatchEvent(Types.InputEventType.Swipe, (worldPointerPosition - worldPointerDownPosition) * -1f);
        }
    }

    void ZoomMap(bool zoomIn)
    {
        InputManager.Inst.InputEvent(Types.InputEventType.Zoom, new Vector3(zoomIn ? 1f : -1f, 0f, 0f));
    }

    void OnCameraEvent(Types.CameraEventType cameraEventType)
    {
        switch (cameraEventType)
        {
            case Types.CameraEventType.None:
                break;
            case Types.CameraEventType.CameraSizeChanged:
                UpdateCameraRect();
                break;
            default:
                break;
        }
    }

    void UpdateCameraRect()
    {
        Vector2 minimapCameraRectSize = new Vector2(_camera.orthographicSize * 2f * _camera.aspect * _miniMapSizeMultifier, _camera.orthographicSize * 2f * _miniMapSizeMultifier);
        MiniMapCameraRectImage.rectTransform.sizeDelta = minimapCameraRectSize;
        Vector3 _minimapAxis = new Vector3(MinimapRawImage.rectTransform.rect.width - minimapCameraRectSize.x, MinimapRawImage.rectTransform.rect.height - minimapCameraRectSize.y, 0f) * 0.5f;
        MiniMapCameraRectImage.transform.localPosition = _minimapAxis + (_camera.transform.position - _cameraManager.ValidMapAreaCenter) * _miniMapSizeMultifier;

        CameraMagnificationText.text = ((int)(_cameraManager.MinCameraSize / _camera.orthographicSize * 100f)).ToString() + "%";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
        _pointerDownPosition = Input.mousePosition;
        InputManager.Inst.DispatchEvent(Types.InputEventType.Down, _pointerDownPosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;
        InputManager.Inst.DispatchEvent(Types.InputEventType.Up, eventData.position);
    }
}
