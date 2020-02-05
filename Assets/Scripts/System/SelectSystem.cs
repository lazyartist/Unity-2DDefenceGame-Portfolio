using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectSystem : SingletonBase<SelectSystem> {
    public Types.SelectionEvent SelectionEvent;
    public GameObject SelectedGameObject;
    public Selector Selector;

    private int _pointerId;

    protected override void Awake()
    {
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
                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D raycastHit = Physics2D.Raycast(clickPosition, Vector3.forward, 10f);
                if(raycastHit)
                {
                    Debug.Log("click on gameobject " + raycastHit.collider);
                    Selector selector = raycastHit.collider.GetComponent<Selector>();
                    if(selector == null)
                    {
                        // 아무것도 클릭하지 않았다. 현재 선택된 오브젝트 해제
                        selector = null;
                        SelectionEvent();
                    }
                }
            }


            //clickPosition.z = 0;
            //ClickAnimator.transform.position = clickPosition;
            //ClickAnimator.SetTrigger("Click");

            //AProjectile projectile = null;
            //switch (MasterSkillType)
            //{
            //    case Types.MasterSkillType.None:
            //        return;
            //    case Types.MasterSkillType.Fire:
            //        projectile = Instantiate<AProjectile>(Projectile_FireDrop, clickPosition, Quaternion.identity, transform);
            //        break;
            //    case Types.MasterSkillType.Rain:
            //        projectile = Instantiate<AProjectile>(Projectile_RainDrop, clickPosition, Quaternion.identity, transform);
            //        break;
            //    default:
            //        break;
            //}

            //if (projectile != null)
            //{
            //    projectile.InitByPosition(clickPosition);
            //}
        }
    }
}
