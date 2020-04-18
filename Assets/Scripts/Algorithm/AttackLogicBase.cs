using UnityEngine;
using UnityEditor;

public class AttackLogicBase : MonoBehaviour
{
    //[MenuItem("Tools/MyTool/Do It in C#")]
    //static void DoIt()
    //{
    //    EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    //}

    public bool TryFindTarget(AttackData attackData)
    {
        Debug.Log("TryFindTarget in AttackLogicBase");
        return true;
    }
}
