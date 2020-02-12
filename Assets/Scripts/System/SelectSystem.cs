using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectSystem : SingletonBase<SelectSystem>
{
    public Types.SelectionEvent SelectionEvent;
    public Selector CurSelector;

    public GameObject ClickContainer;
    public Animator ClickCursorAnimator_Fail;
    public Animator ClickCursorAnimator_Success;
    public Animator RallyPointAnimator;
    public SpriteRenderer MapSpriteRenderer;
    public SpriteRenderer WaymapSpriteRenderer;
    public Vector3 _waymapSpritePivot;

    //PC에서는 포인터 아이디가 -1이고 모바일에서는 0이므로 다르게 값을 넣어준다
#if UNITY_EDITOR || UNITY_STANDALONE || UNITYPLAYER
    private const int _pointerId = -1; // PC
#elif UNITY_IOS || UNITY_ANDROID || UNITY_IPHONEA
    private const int _pointerId = 0; // Mobile(touch)
#endif

    protected override void Awake()
    {
        base.Awake();

        _waymapSpritePivot = new Vector3(WaymapSpriteRenderer.sprite.pivot.x, WaymapSpriteRenderer.sprite.pivot.y, 0f);
    }

    //public void Notify(GameObject selectGameObject)
    //{
    //    // todo selectable
    //    if (selectGameObject)
    //    {
    //        SelectedGameObject = selectGameObject;
    //        SelectionEvent(Types.SelectionEventType.Selected, SelectedGameObject);
    //    }
    //}

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(_pointerId)) // true:UI오브젝트 위, false:게임오브젝트 위
            {
                Debug.Log("click on UI");
            }
            else
            {
                // 현재 Selector 있으면 클릭 처리의 우선권을 줌
                Selector selector = null;
                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 positionOnWaymap = (_waymapSpritePivot + ((clickPosition - MapSpriteRenderer.transform.position) * WaymapSpriteRenderer.sprite.pixelsPerUnit / MapSpriteRenderer.gameObject.transform.localScale.x));
                Color pixelOnWaymap = WaymapSpriteRenderer.sprite.texture.GetPixel((int)positionOnWaymap.x, (int)positionOnWaymap.y);
                bool isClickPositionOnWay = pixelOnWaymap == Color.black;
                Types.SelectResult selectResult = new Types.SelectResult();
                if (CurSelector != null)
                {
                    selectResult = CurSelector.SelectNext(selector, clickPosition, isClickPositionOnWay);
                }

                if(selectResult.SelectResultType == Types.SelectResultType.None)
                {
                    // 현재 Selector가 아무런 처리를 하지 않았으므로 클릭된 Selector에게 처리 기회를 줌
                    RaycastHit2D raycastHit = Physics2D.Raycast(clickPosition, Vector3.forward, Camera.main.transform.position.z);
                    if (raycastHit)
                    {
                        //Selector selector = null;
                        selector = raycastHit.collider.GetComponent<Selector>();
                        if(selector != null)
                        {
                            selectResult = selector.Select();
                        }
                    }
                }

                // cursor
                ClickCursorAnimator_Success.gameObject.SetActive(false);
                ClickCursorAnimator_Fail.gameObject.SetActive(false);
                switch (selectResult.CursorType)
                {
                    case Types.CursorType.None:
                        break;
                    case Types.CursorType.Success:
                        ClickCursorAnimator_Success.gameObject.SetActive(true);
                        ClickCursorAnimator_Success.SetTrigger("Click");
                        break;
                    case Types.CursorType.Fail:
                        ClickCursorAnimator_Fail.gameObject.SetActive(true);
                        ClickCursorAnimator_Fail.SetTrigger("Click");
                        break;
                    default:
                        break;
                }

                // flag
                if (selectResult.IsFlag)
                {
                    RallyPointAnimator.gameObject.SetActive(true);
                    RallyPointAnimator.SetTrigger("Click");
                } else
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

                clickPosition.z = 0;
                ClickContainer.transform.position = clickPosition;
            }
        }
    }

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
}
