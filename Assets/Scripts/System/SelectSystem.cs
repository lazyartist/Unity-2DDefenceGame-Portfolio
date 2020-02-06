using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectSystem : SingletonBase<SelectSystem> {
    public Types.SelectionEvent SelectionEvent;
    public GameObject SelectedGameObject;
    public Selector CurSelector;

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
                Debug.Log("click on ui");
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
