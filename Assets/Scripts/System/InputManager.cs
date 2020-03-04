using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : SingletonBase<InputManager>
{
    public Types.InputEvent InputEvent;
    public float SwipeStartDistanceOver = 2f;

    bool _isMouseDown = false;
    bool _isSwiping = false;
    Vector3 _mouseDownPosition;
    float _prevTouchDistance;

    //PC에서는 포인터 아이디가 -1이고 모바일에서는 0이므로 다르게 값을 넣어준다
#if UNITY_EDITOR || UNITY_STANDALONE || UNITYPLAYER
    private const int _pointerId = -1; // PC
#elif UNITY_IOS || UNITY_ANDROID || UNITY_IPHONEA
    private const int _pointerId = 0; // Mobile(touch)
#endif

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.touchCount < 2)
        {
            if (EventSystem.current.IsPointerOverGameObject(_pointerId)) // true:UI오브젝트 위, false:게임오브젝트 위
            {
                Debug.Log("click on UI");
                _isMouseDown = false;
            }
            else
            {
                _isMouseDown = true;
                _mouseDownPosition = Input.mousePosition;
                DispatchEvent(Types.InputEventType.Down, _mouseDownPosition);
            }
            _isSwiping = false;
        }

        if (_isMouseDown && Input.GetMouseButton(0) && Input.touchCount < 2)
        {
            Vector3 mousePosition = Input.mousePosition;
            float distance = Vector2.Distance(mousePosition, _mouseDownPosition);
            if (distance >= SwipeStartDistanceOver)
            {
                _isSwiping = true;
                // Screen 좌표를 PixelPerUnit으로 나눈 값과 ScreenToWorldPoint()를 통해 얻은 값에 차이가 있다.
                // 100px / 100 = 1, ScreenToWorldPoint(100pixel) = 0.7
                // 따라서 ScreenToWorldPoint를 통해 얻은 값으로 Swipe 해야 한다.
                Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                Vector3 worldMouseDownPosition = Camera.main.ScreenToWorldPoint(_mouseDownPosition);
                DispatchEvent(Types.InputEventType.Swipe, (worldMousePosition - worldMouseDownPosition));
            }
        }

        if (_isMouseDown && Input.GetMouseButtonUp(0) && Input.touchCount < 2)
        {
            if (_isSwiping == false)
            {
                DispatchEvent(Types.InputEventType.Up, Input.mousePosition);
            }
            _isSwiping = false;
            _isMouseDown = false;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            DispatchEvent(Types.InputEventType.Zoom, new Vector3(scrollWheel, 0f, 0f));
        }

        if ((Input.touchCount == 2 && EventSystem.current.IsPointerOverGameObject(_pointerId) == false))
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                _prevTouchDistance = ((touch1.position + touch0.deltaPosition) - (touch0.position + touch0.deltaPosition)).magnitude;

                if (_isMouseDown)
                {
                    // 멀티 터치 시 이전 마우스 다운 상태 해제
                    DispatchEvent(Types.InputEventType.DownCanceled, Input.mousePosition);
                    _isSwiping = false;
                    _isMouseDown = false;
                }
            }
            else if ((Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
            {
                float touchDistance = ((touch1.position + touch0.deltaPosition) - (touch0.position + touch0.deltaPosition)).magnitude;
                DispatchEvent(Types.InputEventType.ZoomByTouch, new Vector3(_prevTouchDistance - touchDistance, 0f, 0f));
                _prevTouchDistance = touchDistance;
            }
        }
    }

    public void DispatchEvent(Types.InputEventType inputEventType, Vector3 value)
    {
        if (InputEvent != null)
        {
            InputEvent(inputEventType, value);
        }
    }
}
