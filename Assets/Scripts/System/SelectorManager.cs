using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorManager : SingletonBase<SelectorManager>
{
    public Types.SelectionEvent SelectionEvent;
    public Selector CurSelector;

    public GameObject CursorContainer;
    public Animator CursorAnimator_Fail;
    public Animator CursorAnimator_Success;
    public Animator RallyPointAnimator;
    public SpriteRenderer MapSpriteRenderer;
    public SpriteRenderer WaymapSpriteRenderer;
    public Vector3 _waymapSpritePivot;

    protected override void Awake()
    {
        base.Awake();

        _waymapSpritePivot = new Vector3(WaymapSpriteRenderer.sprite.pivot.x, WaymapSpriteRenderer.sprite.pivot.y, 0f);
    }

    void Start()
    {
        InputManager.Inst.MouseEvent += _OnMouseEvent_InputManager;
    }

    //void Update()
    //{
    //}

    public void SetCurSelector(Selector selector)
    {
        if (CurSelector != null)
        {
            CurSelector.Deselect();
        }
        CurSelector = selector;
    }

    public void Deselect()
    {
        if (CurSelector != null)
        {
            CurSelector.Deselect();
        }
        CurSelector = null;
    }
    Vector3 _lastCameraPosition;
    private void _OnMouseEvent_InputManager(Types.MouseEventType mouseEventType, Vector3 value)
    {
        Debug.Log("MouseEvent " + mouseEventType + ", " + value);
        switch (mouseEventType)
        {
            case Types.MouseEventType.None:
                break;
            case Types.MouseEventType.Down:
                _lastCameraPosition = Camera.main.transform.position;
                break;
            case Types.MouseEventType.Up:
                {
                    // 현재 Selector 있으면 클릭 처리의 우선권을 줌
                    Selector selector = null;
                    Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    clickPosition.z = 0;
                    Vector3 positionOnWaymap = (_waymapSpritePivot + ((clickPosition - MapSpriteRenderer.transform.position) * WaymapSpriteRenderer.sprite.pixelsPerUnit / MapSpriteRenderer.gameObject.transform.localScale.x));
                    Color pixelOnWaymap = WaymapSpriteRenderer.sprite.texture.GetPixel((int)positionOnWaymap.x, (int)positionOnWaymap.y);
                    bool isClickPositionOnWay = pixelOnWaymap == Color.black;
                    Types.SelectResult selectResult = new Types.SelectResult();
                    if (CurSelector != null)
                    {
                        selectResult = CurSelector.SelectNext(selector, clickPosition, isClickPositionOnWay);
                        switch (selectResult.SelectResultType)
                        {
                            case Types.SelectResultType.None:
                                break;
                            case Types.SelectResultType.Select:
                                break;
                            case Types.SelectResultType.Deselect:
                                Deselect();
                                break;
                        }
                    }

                    if (selectResult.SelectResultType == Types.SelectResultType.None || selectResult.SelectResultType == Types.SelectResultType.Deselect)
                    {
                        // 현재 Selector가 아무런 처리를 하지 않았으므로 클릭된 Selector에게 처리 기회를 줌
                        RaycastHit2D raycastHit = Physics2D.Raycast(clickPosition, Vector3.forward, Camera.main.transform.position.z);
                        if (raycastHit)
                        {
                            //Selector selector = null;
                            selector = raycastHit.collider.GetComponent<Selector>();
                            if (selector != null)
                            {
                                selectResult = selector.Select();
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
                            SetCurSelector(selector);
                            break;
                        case Types.SelectResultType.Deselect:
                            Deselect();
                            break;
                    }

                    CursorContainer.transform.position = clickPosition;
                }
                break;
            case Types.MouseEventType.Swipe:
                {
                    Vector3 swipeDistance = value / 100f * -1;
                    Camera.main.transform.position = _lastCameraPosition + swipeDistance;
                }
                break;
            default:
                break;
        }
    }
}
