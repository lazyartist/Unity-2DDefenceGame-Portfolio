﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectSystem : SingletonBase<SelectSystem>
{
    public Types.SelectionEvent SelectionEvent;
    //public GameObject SelectedGameObject;
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
                Selector selector = null;
                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D raycastHit = Physics2D.Raycast(clickPosition, Vector3.forward, 99f);
                if (raycastHit)
                {
                    selector = raycastHit.collider.GetComponent<Selector>();
                }

                if (selector == null)
                {
                    // 선택 가능한 오브젝트를 선택하지 않았다. 현재 선택된 오브젝트 해제
                    Deselect();
                }
                else
                {
                    // 선택 가능한 새로운 오브젝트를 클릭했다. 
                    Debug.Log("click on gameobject " + selector);
                    SelectSelector(selector);
                }

                // 클릭한 위치의 Waymap의 색상정보로 길인지 아닌지 판단
                // Map의 월드에서의 Map의 위치와 스케일까지 고려
                Vector3 positionOnWaymap = (_waymapSpritePivot + ((clickPosition - MapSpriteRenderer.transform.position) * WaymapSpriteRenderer.sprite.pixelsPerUnit / MapSpriteRenderer.gameObject.transform.localScale.x));
                Color pixelOnWaymap = WaymapSpriteRenderer.sprite.texture.GetPixel((int)positionOnWaymap.x, (int)positionOnWaymap.y);

                clickPosition.z = 0;
                ClickContainer.transform.position = clickPosition;
                if (pixelOnWaymap == Color.black) // 길 클릭
                {
                    ClickCursorAnimator_Success.gameObject.SetActive(true);
                    ClickCursorAnimator_Fail.gameObject.SetActive(false);
                    ClickCursorAnimator_Success.SetTrigger("Click");
                }
                else // 길아닌 곳 클릭
                {
                    ClickCursorAnimator_Success.gameObject.SetActive(false);
                    ClickCursorAnimator_Fail.gameObject.SetActive(true);
                    ClickCursorAnimator_Fail.SetTrigger("Click");
                }

                // 현재 Barraks의 랠리포인트 지정하고 있으면 RallyPointFlag 출력
                // todo rally Point mode
                if (true)
                {
                    RallyPointAnimator.gameObject.SetActive(true);
                    RallyPointAnimator.SetTrigger("Click");
                }
                else
                {
                    RallyPointAnimator.gameObject.SetActive(false);
                }
            }
        }
    }

    public void Deselect()
    {
        if (CurSelector != null)
        {
            CurSelector.Deselect();
        }
        CurSelector = null;
    }

    public void SelectSelector(Selector selector)
    {
        if (selector.Select())
        {
            // 선택 오브젝트 교체
            if (CurSelector != null && CurSelector != selector)
            {
                CurSelector.Deselect();
            }
            CurSelector = selector;
            //SelectSelector(selector);
        }
        else
        {
            // 선택 오브젝트가 현재 선택될 수 없다. 교체하지 않음
        }
    }
}
