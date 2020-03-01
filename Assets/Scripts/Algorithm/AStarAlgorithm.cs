using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm
{
    public List<AStarNode> NodePath = new List<AStarNode>();
    public bool IsSuccess = false;

    List<AStarNode> _openNodes = new List<AStarNode>();
    List<AStarNode> _closedNodes = new List<AStarNode>();
    AStarNode _startNode;
    AStarNode _endNode;
    Vector2 _searchAreaSize;
    LayerMask _aStarNodeLayerMask;

    public void Init(AStarNode startNode, AStarNode endNode, Vector2 searchAreaSize, LayerMask aStarNodeLayerMask)
    {
        _startNode = startNode;
        _endNode = endNode;
        _searchAreaSize = searchAreaSize;
        _aStarNodeLayerMask = aStarNodeLayerMask;

        IsSuccess = false;

        _openNodes.Clear();
        _closedNodes.Clear();
    }

    public bool SearchPath()
    {
        int i = 0;
        AStarNode curNode = _startNode;
        curNode.ParentNode = null;
        while (curNode != null && i++ < 100)
        {
            curNode = SearchNode(curNode);
        }
        return IsSuccess;
    }

    AStarNode SearchNode(AStarNode curNode)
    {
        _openNodes.Remove(curNode);
        _closedNodes.Add(curNode);

        // 주변 노드를 찾아온다.
        Collider2D[] colliders = Physics2D.OverlapBoxAll(curNode.transform.position, _searchAreaSize, 0f, _aStarNodeLayerMask);
        Debug.Log("colliders " + colliders.Length);
        if (colliders.Length == 0) return null;

        for (int i = 0; i < colliders.Length; i++)
        {
            AStarNode node = colliders[i].gameObject.GetComponent<AStarNode>();
            if (node != null)
            {
                // 현재 노드, 블록이라면 무시
                if (curNode == node || node.IsBlock)
                {
                    continue;
                }

                // 이미 탐색 완료된 노드라면 무시
                bool isClosedNode = _closedNodes.Any<AStarNode>((eleNode) => node == eleNode);
                if (isClosedNode)
                {
                    continue;
                }

                // 이미 오픈리스트에 있는 노드라면
                bool isOpenNode = _openNodes.Any<AStarNode>((eleNode) => node == eleNode);
                if (isOpenNode)
                {
                    // 누적 경로 비용을 현재 노드를 부모라 가정하고 다시 계산해서
                    // 비용이 더 낮다면 부모를 현재 노드로 변경하고 비용을 전부 갱신
                    var disFromCur = Vector3.Distance(curNode.transform.position, node.transform.position);
                    var disToEnd = Vector3.Distance(_endNode.transform.position, node.transform.position);

                    if (node.goal > curNode.goal + disFromCur)
                    {
                        node.goal = curNode.goal + disFromCur;
                        node.heuristic = disToEnd;
                        node.fitness = node.goal + node.heuristic;

                        node.SetParent(curNode);
                    }

                    continue;
                }

                // 처음 발견한 노드라면 
                {
                    // 비용 계산
                    var disFromCur = Vector3.Distance(curNode.transform.position, node.transform.position);
                    var disToEnd = Vector3.Distance(_endNode.transform.position, node.transform.position);

                    node.goal = curNode.goal + disFromCur;
                    node.heuristic = disToEnd;
                    node.fitness = node.goal + node.heuristic;

                    // 부모 설정
                    node.SetParent(curNode);
                    // 오픈 리스트에 추가
                    _openNodes.Add(node);

                    node.SetColor(Color.gray);
                }
            }
        }

        if (_openNodes.Count == 0)
        {
            // 오픈 노드가 없다. 종료
            return null;
        }

        // 오픈 리스트에서 최적화 노드 찾기
        AStarNode fitnessNode = null;
        foreach (AStarNode node in _openNodes)
        {
            if (fitnessNode == null)
            {
                fitnessNode = node;
            }
            else if (fitnessNode.fitness > node.fitness)
            {
                fitnessNode = node;
            }
        }

        fitnessNode.SetColor(Color.blue);
        if (fitnessNode == _endNode)
        {
            // 목표 지점에 도착했다.
            fitnessNode.SetColor(Color.green);
            IsSuccess = true;
            SavePath();
            return null;
        }

        return fitnessNode;
    }

    // 목표지점에서 역으로 부모노드를 찾아가며 최종 경로 저장
    void SavePath()
    {
        NodePath.Clear();

        AStarNode node = _endNode;
        while (node != null)
        {
            NodePath.Insert(0, node);

            if (node != _endNode && node != _startNode)
            {
                node.SetColor(Color.yellow);
            }

            node = node.ParentNode;
        }
    }
}
