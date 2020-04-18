using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTest : MonoBehaviour , IPointerClickHandler, IPointerUpHandler{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData);
        Debug.Log("OnPointerClick");
        //throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        Debug.Log("OnClick");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(eventData);
        Debug.Log("OnPointerUp");
    }
}
