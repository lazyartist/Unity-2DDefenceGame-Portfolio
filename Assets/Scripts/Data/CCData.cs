using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CCData_", menuName = "SO/Create CCData")]
// todo CC -> Burf
public class CCData : ScriptableObject {
    public Types.CCType CCType = Types.CCType.None;
    public float CCTime = 0f;
    public float CCValue = 0f;

    public void Copy(CCData ccData)
    {
        ccData.CCType = this.CCType;
        ccData.CCTime = this.CCTime;
        ccData.CCValue = this.CCValue;
    }
}
