using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectSystem : SingletonBase<SelectSystem> {
    public Types.SelectionEvent SelectionEvent;
    public GameObject SelectedGameObject;
    public Selector CurSelector;

    public GameObject ClickContainer;
    public Animator ClickCursorAnimator_Fail;
    public Animator ClickCursorAnimator_Success;
    public Animator RallyPointAnimator;
    public SpriteRenderer WaymapSpriteRenderer;

    private int _pointerId;

    protected override void Awake()
    {
        base.Awake();

        //PC에서는 포인터 아이디가 -1이고 모바일에서는 0이므로 다르게 값을 넣어준다
#if UNITY_EDITOR || UNITY_STANDALONE || UNITYPLAYER
        _pointerId = -1;
#elif UNITY_IOS || UNITY_ANDROID || UNITY_IPHONEA
        _pointerId = 0;
#endif
    }

    public void Notify(GameObject selectGameObject)
    {
        // todo selectable
        if (selectGameObject)
        {
            SelectedGameObject = selectGameObject;
            SelectionEvent(Types.SelectionEventType.Selected, SelectedGameObject);
        }
    }
    
	void Start () {
		
	}
	
	void Update () {
        if (Input.GetMouseButtonUp(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(_pointerId)) // true:UI오브젝트 위, false:게임오브젝트 위
            {
                Debug.Log("click on UI");
            }
            else
            {
                Selector selector = null;
                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D raycastHit = Physics2D.Raycast(clickPosition, Vector3.forward, 10f);
                if(raycastHit)
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

                // 길을 클릭했는지 확인
                Color pixel = WaymapSpriteRenderer.sprite.texture.GetPixel((int)Input.mousePosition.x, (int)Input.mousePosition.y);
                clickPosition.z = 0;
                ClickContainer.transform.position = clickPosition;
                Debug.Log("Click color " + pixel);

                if(pixel == Color.black) // 길 클릭
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

                if (true)
                {
                    RallyPointAnimator.gameObject.SetActive(true);
                    RallyPointAnimator.SetTrigger("Click");
                }else
                {
                    RallyPointAnimator.gameObject.SetActive(false);
                }

                // todo 클릭 결과에 따라 클릭 아이콘 애니 재생 출력
                //Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //ClickCursorAnimator_Success.SetTrigger("Click");
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
