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
        InputManager.Inst.InputEvent += OnInputEvent_InputManager;
    }

    public void RegisterSelector(Selector newSelector)
    {
        if (CurSelector != null && CurSelector != newSelector)
        {
            CurSelector.SelectExit();
        }

        if (CurSelector != newSelector)
        {
            Types.SelectResult selectResult = newSelector.SelectEnter();
            UpdateCursor(selectResult);
        }
        CurSelector = newSelector;
    }

    public void UnregisterSelector()
    {
        if (CurSelector != null)
        {
            CurSelector.SelectExit();
        }
        CurSelector = null;
    }

    void UpdateCursor(Types.SelectResult selectResult)
    {
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
    }

    void OnInputEvent_InputManager(Types.InputEventType inputEventType, Vector3 value)
    {
        switch (inputEventType)
        {
            case Types.InputEventType.None:
                break;
            case Types.InputEventType.Down:
                break;
            case Types.InputEventType.Up:
                {
                    Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    clickPosition.z = 0;
                    Vector3 positionOnWaymap = (_waymapSpritePivot + ((clickPosition - MapSpriteRenderer.transform.position) * WaymapSpriteRenderer.sprite.pixelsPerUnit / MapSpriteRenderer.gameObject.transform.localScale.x));
                    Color pixelOnWaymap = WaymapSpriteRenderer.sprite.texture.GetPixel((int)positionOnWaymap.x, (int)positionOnWaymap.y);
                    bool isClickPositionOnWay = pixelOnWaymap == Color.black;
                    Types.SelectResult selectResult = Types.SelectResult.Create();
                    bool isDirtyCursorPosition = false;

                    // 현재 Selector가 있으면 처리의 우선권을 줌
                    if (CurSelector != null)
                    {
                        selectResult = CurSelector.SelectUpdate(clickPosition, isClickPositionOnWay);
                        switch (selectResult.SelectResultType)
                        {
                            case Types.SelectResultType.None:
                                break;
                            case Types.SelectResultType.Register:
                                break;
                            case Types.SelectResultType.Unregister:
                                UnregisterSelector();
                                break;
                        }
                        UpdateCursor(selectResult);
                        isDirtyCursorPosition = true;
                    }

                    // 현재 Selector가 새로운 Selector에게 이벤트 전파를 해도 된다고 함
                    if (CurSelector == null || selectResult.IsSpread)
                    {
                        RaycastHit2D raycastHit = Physics2D.Raycast(clickPosition, Vector3.forward, Camera.main.transform.position.z);
                        if (raycastHit)
                        {
                            Selector newSelector = raycastHit.collider.GetComponent<Selector>();
                            if (newSelector != null)
                            {
                                RegisterSelector(newSelector);
                                isDirtyCursorPosition = true;
                            }
                        }
                    }

                    if (isDirtyCursorPosition)
                    {
                        CursorContainer.transform.position = clickPosition;
                    }
                }
                break;
            case Types.InputEventType.Swipe:
                break;
            default:
                break;
        }
    }
}
