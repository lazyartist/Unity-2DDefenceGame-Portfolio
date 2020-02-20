using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorManager : SingletonBase<SelectorManager>
{
    public Selector CurSelector;

    public GameObject CursorContainer;
    public Animator CursorAnimator_Fail;
    public Animator CursorAnimator_Success;
    public Animator RallyPointAnimator;
    public SpriteRenderer MapSpriteRenderer;
    public SpriteRenderer WaymapSpriteRenderer;

    private Vector3 _waymapSpritePivot;

    protected override void Awake()
    {
        base.Awake();

        _waymapSpritePivot = new Vector3(WaymapSpriteRenderer.sprite.pivot.x, WaymapSpriteRenderer.sprite.pivot.y, 0f);
    }

    void Start()
    {
        InputManager.Inst.InputEvent += _OnInputEvent_InputManager;
    }

    //void Update()
    //{
    //}

    public void RegisterSelector(Selector selector)
    {
        if (CurSelector != null)
        {
            CurSelector.SelectExit();
        }
        CurSelector = selector;
    }

    public void UnregisterSelector()
    {
        if (CurSelector != null)
        {
            CurSelector.SelectExit();
        }
        CurSelector = null;
    }
    private void _OnInputEvent_InputManager(Types.InputEventType inputEventType, Vector3 value)
    {
        switch (inputEventType)
        {
            case Types.InputEventType.None:
                break;
            case Types.InputEventType.Down:
                break;
            case Types.InputEventType.Up:
                {
                    // 현재 Selector가 있으면 처리의 우선권을 줌
                    Selector selector = null;
                    Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    clickPosition.z = 0;
                    Vector3 positionOnWaymap = (_waymapSpritePivot + ((clickPosition - MapSpriteRenderer.transform.position) * WaymapSpriteRenderer.sprite.pixelsPerUnit / MapSpriteRenderer.gameObject.transform.localScale.x));
                    Color pixelOnWaymap = WaymapSpriteRenderer.sprite.texture.GetPixel((int)positionOnWaymap.x, (int)positionOnWaymap.y);
                    bool isClickPositionOnWay = pixelOnWaymap == Color.black;
                    Types.SelectResult selectResult = Types.SelectResult.Create();
                    if (CurSelector != null)
                    {
                        selectResult = CurSelector.SelectUpdate(selector, clickPosition, isClickPositionOnWay);
                        switch (selectResult.SelectResultType)
                        {
                            case Types.SelectResultType.None:
                                break;
                            case Types.SelectResultType.Select:
                                break;
                            case Types.SelectResultType.Deselect:
                                UnregisterSelector();
                                break;
                        }
                    }

                    //if (selectResult.SelectResultType == Types.SelectResultType.None || selectResult.SelectResultType == Types.SelectResultType.Deselect)
                    if (selectResult.IsSpread)
                    {
                        // 현재 Selector가 다음 Selector에게 이벤트 전파를 해도 된다고 함
                        RaycastHit2D raycastHit = Physics2D.Raycast(clickPosition, Vector3.forward, Camera.main.transform.position.z);
                        if (raycastHit)
                        {
                            //Selector selector = null;
                            selector = raycastHit.collider.GetComponent<Selector>();
                            if (selector != null)
                            {
                                selectResult = selector.SelectEnter();
                            }
                        }
                    }

                    // cursor
                    CursorAnimator_Success.gameObject.SetActive(false);
                    CursorAnimator_Fail.gameObject.SetActive(false);
                    switch (selectResult.CursorType)
                    {
                        case Types.CursorType.None:
                            break;
                        case Types.CursorType.Success:
                            CursorAnimator_Success.gameObject.SetActive(true);
                            CursorAnimator_Success.SetTrigger("Click");
                            break;
                        case Types.CursorType.Fail:
                            CursorAnimator_Fail.gameObject.SetActive(true);
                            CursorAnimator_Fail.SetTrigger("Click");
                            break;
                        default:
                            break;
                    }

                    // flag
                    if (selectResult.IsFlag)
                    {
                        RallyPointAnimator.gameObject.SetActive(true);
                        RallyPointAnimator.SetTrigger("Click");
                    }
                    else
                    {
                        RallyPointAnimator.gameObject.SetActive(false);
                    }

                    switch (selectResult.SelectResultType)
                    {
                        case Types.SelectResultType.None:
                            break;
                        case Types.SelectResultType.Select:
                            RegisterSelector(selector);
                            break;
                        case Types.SelectResultType.Deselect:
                            UnregisterSelector();
                            break;
                    }

                    CursorContainer.transform.position = clickPosition;
                }
                break;
            case Types.InputEventType.Swipe:
                break;
            default:
                break;
        }
    }
}
