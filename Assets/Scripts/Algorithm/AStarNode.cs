using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : MonoBehaviour
{
    public AStarNode ParentNode;

    public bool IsBlock = false;

    public float fitness; // 적절성 : goal + heuristic
    public float goal; // 목표 : 시작에서 현재까지 거리
    public float heuristic; // 추정치 : 종료에서 현재까지  거리

    Color _color = Color.white;
    float _angleToParent;
    Vector3 _directionToParent = Vector3.right;

    public void SetColor(Color color)
    {
        _color = color;
    }

    public void SetParent(AStarNode parentNode)
    {
        ParentNode = parentNode;

        Vector3 direction = ParentNode.transform.position - this.transform.position;
        float radian = Mathf.Atan2(direction.y, direction.x);
        float degree = radian * Mathf.Rad2Deg;
        _directionToParent = Quaternion.Euler(0f, 0f, degree) * Vector3.right * 0.5f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsBlock ? Color.black : _color;
        Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _directionToParent);
    }
}
