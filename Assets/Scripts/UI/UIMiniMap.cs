using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniMap : MonoBehaviour {
    public Button ZoomInButton;
    public Button ZoomOutButton;

    // Use this for initialization
    void Start () {
        ZoomInButton.onClick.AddListener(() =>
        {
            ZoomMap(true);
        });
        ZoomOutButton.onClick.AddListener(() =>
        {
            ZoomMap(false);
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ZoomMap(bool zoomIn)
    {
        InputManager.Inst.InputEvent(Types.InputEventType.Zoom, new Vector3(zoomIn ? -1f : 1f, 0f, 0f));
    }
}
