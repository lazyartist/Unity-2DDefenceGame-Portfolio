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

    public List<int> BlockNodePositionIndices;

    public AStarNode StartNode;
    public AStarNode EndNode;

    public int StartNodePositionIndex;
    public int EndNodePositionIndex;

    public List<AStarNode> Nodes = new List<AStarNode>();

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
        Vector2 mapArea = new Vector2(CameraController.ValidMapAreaRect.xMax - CameraController.ValidMapAreaRect.xMin, CameraController.ValidMapAreaRect.yMax - CameraController.ValidMapAreaRect.yMin);
        _nodeSpace = new Vector2(mapArea.x / NodeGridSize.x, mapArea.y / NodeGridSize.y);
        _nodeSearchAreaSize = _nodeSpace * 2f;
        Vector3 startPosition = new Vector3(mapArea.x * -0.5f + _nodeSpace.x * 0.5f, mapArea.y * 0.5f + _nodeSpace.y * -0.5f, 0f);

        // remove all children
        for (int i = this.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(this.transform.GetChild(i).gameObject);
        }

        Nodes.Clear();

        // Create nodes
        for (int i = 0; i < NodeGridSize.x * NodeGridSize.y; i++)
        {
            int col = (i % NodeGridSize.x);
            int row = Mathf.FloorToInt(i / NodeGridSize.x);

            AStarNode node = Instantiate<AStarNode>(AStarNodePrefab);
            node.transform.SetParent(AStarNodeContainer.transform);
            node.transform.position = startPosition + new Vector3(col * _nodeSpace.x, (row * -_nodeSpace.y), 0f);

            node.name = i.ToString();
            Nodes.Add(node);

            // 블록 노드 찾기
            for (int j = 0; j < BlockNodePositionIndices.Count; j++)
            {
                int BlockNodePositionIndex = BlockNodePositionIndices[j];
                if (i == BlockNodePositionIndex)
                {
                    node.IsBlock = true;
                }
            }
        }

        StartNode = Nodes[StartNodePositionIndex];
        EndNode = Nodes[EndNodePositionIndex];
    }

    public List<Vector3> GetPathOrNull(Vector3 startPosition, Vector3 endPosition)
    {
        StartNode = FindNode(startPosition);
        EndNode = FindNode(endPosition);

        if (StartNode == null || EndNode == null)
        {
            return null;
        }

        _aStarAlgorithm.Init(StartNode, EndNode, _nodeSearchAreaSize, AStarNodeLayerMask);
        bool isSuccess = _aStarAlgorithm.SearchPath();
        if (isSuccess == false || _aStarAlgorithm.NodePath.Count == 0)
        {
            return null;
        }

        List<Vector3> positionPath = new List<Vector3>();
        for (int i = 0; i < _aStarAlgorithm.NodePath.Count; i++)
        {
            AStarNode node = _aStarAlgorithm.NodePath[i];
            positionPath.Add(node.transform.position);
        }
        return positionPath;
    }

    AStarNode FindNode(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, _nodeSearchAreaSize, 0f, AStarNodeLayerMask);
        AStarNode node = null;
        float minDistance = 999;
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];
            float distance = Vector3.Distance(collider.transform.position, position);
            if (minDistance > distance)
            {
                minDistance = distance;
                node = collider.GetComponent<AStarNode>();
            }
        }

        return node;
    }

    private void OnDrawGizmos()
    {
        if (StartNode == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawCube(StartNode.transform.position, new Vector3(0.3f, 0.3f, 0.3f));
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(EndNode.transform.position, new Vector3(0.3f, 0.3f, 0.3f));
    }
}
