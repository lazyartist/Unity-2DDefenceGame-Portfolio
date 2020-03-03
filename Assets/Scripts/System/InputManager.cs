using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : SingletonBase<InputManager>
{
    public Types.InputEvent InputEvent;
    public float SwipeStartDistanceOver = 2f;
    private bool _isMouseDown = false;
    private bool _isSwiping = false;
    private Vector3 _mousePositionDown;

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
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(_pointerId)) // true:UI오브젝트 위, false:게임오브젝트 위
            {
                Debug.Log("click on UI");
                _isMouseDown = false;
            }
            else
            {
                _isMouseDown = true;
                _mousePositionDown = Input.mousePosition;
                DispatchEvent(Types.InputEventType.Down, _mousePositionDown);
            }
            _isSwiping = false;
        }

        if (_isMouseDown && Input.GetMouseButton(0))
        {
            Vector3 mousePositionLast = Input.mousePosition;
            float distance = Vector2.Distance(mousePositionLast, _mousePositionDown);
            if (distance >= SwipeStartDistanceOver)
            {
                _isSwiping = true;
                DispatchEvent(Types.InputEventType.Swipe, mousePositionLast - _mousePositionDown);
            }
        }

        if (_isMouseDown && Input.GetMouseButtonUp(0))
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

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(_pointerId)) // true:UI오브젝트 위, false:게임오브젝트 위
            {
                //Debug.Log("click on UI");
                //_isMouseDown = false;
            }
            else
            {
                Input.simulateMouseWithTouches = true;
                UIDebug.Inst.AddText("touchCount " + Input.touchCount);
                //_isMouseDown = true;
                //_mousePositionDown = Input.mousePosition;
                //DispatchEvent(Types.InputEventType.Down, _mousePositionDown);
            }
            //_isSwiping = false;
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
