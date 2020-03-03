using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniMap : MonoBehaviour
{
    public RawImage MinimapRawImage;
    public Image MiniMapCameraRectImage;
    public Text CameraMagnificationText;

    CameraManager _cameraManager;
    Camera _camera;
    float _minimapMultifier;

    void Start()
    {
        CameraManager.Inst.CameraEvent += OnCameraEvent;
        _cameraManager = CameraManager.Inst;
        _camera = CameraManager.Inst.Camera;

        // 미니맵 카메라와 동일한 비율로 미니맵의 크기를 조정한다
        float miniMapCameraHeight = CameraManager.Inst.MinimapCamera.orthographicSize * 2f;
        float miniMapCameraWidth = miniMapCameraHeight * CameraManager.Inst.MinimapCamera.aspect;
        MinimapRawImage.rectTransform.sizeDelta = new Vector2(MinimapRawImage.rectTransform.rect.width, MinimapRawImage.rectTransform.rect.width * miniMapCameraHeight / miniMapCameraWidth);
        _minimapMultifier = MinimapRawImage.rectTransform.rect.height / _cameraManager.ValidMapAreaSize.y;
    }

    void Update()
    {
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
                UpdateCamera();
                break;
            default:
                break;
        }
    }

    void UpdateCamera()
    {
        float miniMapCameraRectWidth = _camera.orthographicSize * 2f * _camera.aspect * _minimapMultifier;
        float miniMapCameraRectHeight = _camera.orthographicSize * 2f * _minimapMultifier;
        MiniMapCameraRectImage.rectTransform.sizeDelta = new Vector2(miniMapCameraRectWidth, miniMapCameraRectHeight);
        MiniMapCameraRectImage.transform.localPosition = _camera.transform.position * _minimapMultifier;

        CameraMagnificationText.text = ((int)(_camera.orthographicSize / _cameraManager.MaxCameraSize * 100f)).ToString() + "%";
    }
}
