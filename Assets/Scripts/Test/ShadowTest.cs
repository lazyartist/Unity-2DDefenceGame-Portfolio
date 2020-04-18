using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowTest : MonoBehaviour {
    public Image Image;
    public Shadow Shadow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float maxDistance = 100;
        var mouse = Input.mousePosition;

        if (true)
        {
            float distance = Mathf.Min(maxDistance, Vector2.Distance(Image.rectTransform.position, mouse));
            float rad = Mathf.Atan2(Image.rectTransform.position.y - mouse.y, Image.rectTransform.position.x - mouse.x);
            float x = Mathf.Cos(rad) * distance;
            float y = Mathf.Sin(rad) * distance;
            Shadow.effectDistance = new Vector2(x, y);
        } 

        if(false)
        {
            float x = Mathf.Min(Image.rectTransform.position.x - mouse.x, maxDistance);
            float y = Mathf.Min(Image.rectTransform.position.y - mouse.y, maxDistance);
            Shadow.effectDistance = new Vector2(x, y);
        }
    }
}
