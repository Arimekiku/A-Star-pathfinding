using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSelector : MonoBehaviour
{
    [SerializeField] private Vector2Int _boardSize;

    private Board _currentBoard;
    private Node _currentTargetNode;
    private Node _currentStartNode;
    private List<Node> _currentPath = new List<Node>();
    private PathFinder _pathFinder = new PathFinder();

    private void Awake() => _currentBoard = FindObjectOfType<Board>();

    private void Start() => StartCoroutine(_currentBoard.Initialize(_boardSize));
    
    private void ClickedOnNode(Node newTargetNode) 
    {
        if (newTargetNode.CanBePath == false)
            return;
        
        _currentTargetNode?.Model.ClearMark();
        _currentTargetNode = newTargetNode;

        StartCoroutine(ClearPath());
    }

    public void ClickedOnStart() 
    {
        int xCoord = Random.Range(0, _boardSize.x);
        int yCoord = Random.Range(0, _boardSize.y);
        _currentStartNode = _currentBoard.TryGetNodeByBoardCoord(xCoord, yCoord);
        while (_currentStartNode.CanBePath == false) 
        {
            xCoord = Random.Range(0, _boardSize.x);
            yCoord = Random.Range(0, _boardSize.y);
            _currentStartNode = _currentBoard.TryGetNodeByBoardCoord(xCoord, yCoord);
        }
        _currentStartNode.Model.MarkAsStart();

        List<Node> nodesOnField = _currentBoard.GetNodes();

        foreach (Node node in nodesOnField) 
        {
            if (node == _currentStartNode)
                continue;
            
            node.OnNodeClick += ClickedOnNode;
        }

        foreach (Node node in nodesOnField) 
            node.Model.EnableCollider();
    }

    private IEnumerator AnimatePath() 
    {
        _currentPath.Reverse();
        int i = 0;
        Node currentNode = _currentPath[i++];

        while (currentNode != _currentPath[_currentPath.Count - 1]) 
        {
            currentNode.Model.MarkAsPath();

            yield return new WaitForSeconds(0.02f);

            currentNode = _currentPath[i++];
        }

        _currentTargetNode.Model.MarkAsTarget();
    }

    private IEnumerator ClearPath() 
    {
        if (_currentPath.Count != 0) 
        {
            int i = 0;
            Node currentNode = _currentPath[i++];

            while (currentNode != _currentPath[_currentPath.Count - 1]) 
            {
                currentNode.Model.ClearMark();

                yield return new WaitForSeconds(0.02f);

                currentNode = _currentPath[i++];
            }

            currentNode.Model.ClearMark();
        }

        _currentPath = new List<Node>(_pathFinder.Path(_currentStartNode, _currentTargetNode));

        StartCoroutine(AnimatePath());
    }
}
