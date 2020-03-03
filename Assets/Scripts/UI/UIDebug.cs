using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDebug : SingletonBase<UIDebug> {
    public Text Text;

    public void AddText(string text)
    {
        Text.text += "\n" + text;
        Debug.Log(text);
    }
}
