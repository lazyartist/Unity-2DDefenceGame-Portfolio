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
    float _minimapSizeMultifier;

    void Start()
    {
        CameraManager.Inst.CameraEvent += OnCameraEvent;
        _cameraManager = CameraManager.Inst;
        _camera = CameraManager.Inst.Camera;

        // 미니맵 카메라와 동일한 비율로 미니맵의 크기를 조정한다
        float miniMapCameraHeight = CameraManager.Inst.MinimapCamera.orthographicSize * 2f;
        float miniMapCameraWidth = miniMapCameraHeight * CameraManager.Inst.MinimapCamera.aspect;
        MinimapRawImage.rectTransform.sizeDelta = new Vector2(MinimapRawImage.rectTransform.rect.width, MinimapRawImage.rectTransform.rect.width * miniMapCameraHeight / miniMapCameraWidth);
        _minimapSizeMultifier = MinimapRawImage.rectTransform.rect.height / _cameraManager.ValidMapAreaSize.y;
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
        Vector2 minimapCameraRectSize = new Vector2(_camera.orthographicSize * 2f * _camera.aspect * _minimapSizeMultifier, _camera.orthographicSize * 2f * _minimapSizeMultifier);
        MiniMapCameraRectImage.rectTransform.sizeDelta = minimapCameraRectSize;
        Vector3 minimapAxis = new Vector3(MinimapRawImage.rectTransform.rect.width - minimapCameraRectSize.x, MinimapRawImage.rectTransform.rect.height - minimapCameraRectSize.y, 0f) * 0.5f;
        MiniMapCameraRectImage.transform.localPosition = minimapAxis + _camera.transform.position * _minimapSizeMultifier;

        CameraMagnificationText.text = ((int)(_cameraManager.MinCameraSize / _camera.orthographicSize * 100f)).ToString() + "%";
    }
}
