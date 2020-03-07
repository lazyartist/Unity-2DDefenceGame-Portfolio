using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingManager : SingletonBase<PathFindingManager>
{
    public AStarNode AStarNodePrefab;
    public Types.IntTuple2 NodeGridSize; // x, y
    public GameObject AStarNodeContainer;
    public LayerMask AStarNodeLayerMask;

    List<AStarNode> nodes = new List<AStarNode>();
    AStarNode _startNode;
    AStarNode _endNode;
    Vector2 _nodeSpace;
    Vector2 _nodeSearchAreaSize;
    AStarAlgorithm _aStarAlgorithm;

    private void Start()
    {
        Init();
        _aStarAlgorithm = new AStarAlgorithm();
    }

    void Init()
    {
        Vector2 mapArea = new Vector2(CameraManager.ValidMapAreaRect.xMax - CameraManager.ValidMapAreaRect.xMin, CameraManager.ValidMapAreaRect.yMax - CameraManager.ValidMapAreaRect.yMin);
        _nodeSpace = new Vector2(mapArea.x / NodeGridSize.x, mapArea.y / NodeGridSize.y);
        _nodeSearchAreaSize = _nodeSpace * 2f;
        Vector3 startPosition = new Vector3(mapArea.x * -0.5f + _nodeSpace.x * 0.5f, mapArea.y * 0.5f + _nodeSpace.y * -0.5f, 0f);

        // remove all children
        for (int i = this.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(this.transform.GetChild(i).gameObject);
        }

        nodes.Clear();

        // Create nodes
        MapManager mapManager = MapManager.Inst;
        for (int i = 0; i < NodeGridSize.x * NodeGridSize.y; i++)
        {
            int col = (i % NodeGridSize.x);
            int row = Mathf.FloorToInt(i / NodeGridSize.x);
            Vector3 position = startPosition + new Vector3(col * _nodeSpace.x, (row * -_nodeSpace.y), 0f); ;

            bool isBlock = mapManager.IsMask(position, Types.MapMaskChannelType.Block, Consts.MapMaskColor_Block);
            if (isBlock)
            {
                // Block 노드는 생성하지 않기
                continue;
            }

            AStarNode node = Instantiate<AStarNode>(AStarNodePrefab, AStarNodeContainer.transform);
            node.transform.position = position;
            node.IsBlock = isBlock;
            node.name = i.ToString();
            nodes.Add(node);
        }
        Debug.Log("Node Count : " + AStarNodeContainer.transform.childCount);

        _startNode = nodes[0];
        _endNode = nodes[nodes.Count - 1];
    }

    public List<Vector3> GetPathOrNull(Vector3 startPosition, Vector3 endPosition, out Types.PathFindResultType pathFindResultType)
    {
        ResetAllNodes();

        _startNode = FindNearestNodeToTargetPosition(startPosition, endPosition);
        _endNode = FindNearestNodeToTargetPosition(endPosition, startPosition);

        if (_startNode == _endNode)
        {
            pathFindResultType = Types.PathFindResultType.EqualStartAndEnd;
            return null;
        }

        if (_startNode == null || _endNode == null)
        {
            pathFindResultType = Types.PathFindResultType.Fail;
            return null;
        }

        _aStarAlgorithm.Init(_startNode, _endNode, _nodeSearchAreaSize, AStarNodeLayerMask);
        bool isSuccess = _aStarAlgorithm.SearchPath();
        if (isSuccess == false || _aStarAlgorithm.NodePath.Count == 0)
        {
            pathFindResultType = Types.PathFindResultType.Fail;
            return null;
        }

        List<Vector3> positionPath = new List<Vector3>();
        for (int i = 0; i < _aStarAlgorithm.NodePath.Count; i++)
        {
            AStarNode node = _aStarAlgorithm.NodePath[i];
            positionPath.Add(node.transform.position);
        }
        pathFindResultType = Types.PathFindResultType.Success;
        return positionPath;
    }

    void ResetAllNodes()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            AStarNode node = nodes[i];
            node.Reset();
        }
    }

    AStarNode FindNearestNodeToTargetPosition(Vector3 position, Vector3 targetPosition)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, _nodeSearchAreaSize, 0f, AStarNodeLayerMask);
        AStarNode nearestNodeToTargetPosition = null;
        float minDistance = 999;
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];
            AStarNode node = collider.GetComponent<AStarNode>();
            float distance = Vector3.Distance(collider.transform.position, targetPosition);
            if (node.IsBlock == false && minDistance > distance)
            {
                minDistance = distance;
                nearestNodeToTargetPosition = node;
            }
        }

        return nearestNodeToTargetPosition;
    }

    private void OnDrawGizmos()
    {
        if (_startNode == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawCube(_startNode.transform.position, new Vector3(0.6f, 0.6f, 0.6f));
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(_endNode.transform.position, new Vector3(0.6f, 0.6f, 0.6f));
    }
}
