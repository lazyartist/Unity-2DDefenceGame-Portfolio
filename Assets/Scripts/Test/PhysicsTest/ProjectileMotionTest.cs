using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMotionTest : MonoBehaviour
{
    [Range(0, 360)]
    public float Angle;
    public float Velocity;
    public float Gravity = 9.8f;

    public float XRange = 10f;
    public float DrawLineResolution = 50;
    
    private void OnDrawGizmos()
    {
        // 각도와 속도 그리기
        float radian = Angle * Mathf.Deg2Rad;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(Velocity * Mathf.Cos(radian), Velocity * Mathf.Sin(radian), 0));

        // 포물선 그리기
        Gizmos.color = Color.red;
        float xUnit = XRange / DrawLineResolution;
        Vector3 prevPosition = transform.position;
        for (int i = 0; i < DrawLineResolution; i++)
        {
            float x = xUnit * i;
            float y = GetProjectileMotionY(x);
            Vector3 curPosition = transform.position + new Vector3(x, y, 0);

            Gizmos.DrawLine(prevPosition, curPosition);

            prevPosition = curPosition;
        }
    }

    private float GetProjectileMotionY(float x)
    {
        float radian = Angle * Mathf.Deg2Rad;
        float y = Mathf.Tan(radian) * x;
        y -= Gravity / (2 * Velocity * Velocity * Mathf.Cos(radian) * Mathf.Cos(radian)) * (x * x);
        return y;
    }
}
