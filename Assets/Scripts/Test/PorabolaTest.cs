using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorabolaTest : MonoBehaviour {
    public float HeightLimit = 10f;
    public float TimeToTopmostHeight = 2f;
    public int frameCount = 100;

    public GameObject Target;

    void OnDrawGizmos()
    {
        return;
        ParabolaAlgorithm porabola = new ParabolaAlgorithm();
        porabola.Init(HeightLimit, TimeToTopmostHeight, transform.position, Target.transform.position);

        Gizmos.color = Color.blue;
        float frameTime = porabola.TimeToEndPosition / frameCount;
        for (int i = 1; i <= frameCount; i++)
        {
            float t = frameTime * (float)i;
            Gizmos.DrawWireSphere(porabola.GetPosition(t), 0.5f);
        }
    }
}
