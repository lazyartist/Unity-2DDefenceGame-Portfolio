using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDebug : SingletonBase<UIDebug> {
    public Text Text;

    ScrollRect _scrollRect;

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    public void AddText(string text)
    {
        Text.text += "\n" + text;
        _scrollRect.verticalNormalizedPosition = 0f;
        Debug.Log(text);
        if(Text.text.Length > 1000)
        {
            Text.text = "";
        }
    }
}
