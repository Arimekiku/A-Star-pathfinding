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

    private void Start()
    {
        _currentBoard.Initialize(_boardSize);

        int xCoord = Random.Range(0, _boardSize.x);
        int yCoord = Random.Range(0, _boardSize.y);
        _currentStartNode = _currentBoard.TryGetNode(xCoord, yCoord);
        _currentStartNode.Model.MarkAsStart();

        foreach (Node node in _currentBoard.GetNodes()) 
        {
            if (node == _currentStartNode)
                continue;
            
            node.OnNodeClick += ClickedOn;
        }
    }

    private void ClickedOn(Node newTargetNode) 
    {
        _currentTargetNode?.Model.ClearMark();
        _currentTargetNode = newTargetNode;

        StartCoroutine(ClearPath());
        _currentPath = new List<Node>(_pathFinder.Path(_currentStartNode, _currentTargetNode));
        StartCoroutine(AnimatePath());

        _currentTargetNode.Model.MarkAsTarget();
    }

    private IEnumerator AnimatePath() 
    {
        _currentPath.Reverse();
        int i = 0;
        Node currentNode = _currentPath[i++];

        while (currentNode != _currentPath[_currentPath.Count - 1]) 
        {
            currentNode.Model.MarkAsPath();

            yield return new WaitForSeconds(0.05f);

            currentNode = _currentPath[i++];
        }
    }

    private IEnumerator ClearPath() 
    {
        if (_currentPath.Count == 0)
            yield break;

        List<Node> pathCopy = _currentPath;
        int i = 0;
        Node currentNode = pathCopy[i++];

        while (currentNode != pathCopy[pathCopy.Count - 1]) 
        {
            currentNode.Model.ClearMark();

            yield return new WaitForSeconds(0.05f);

            currentNode = pathCopy[i++];
        }
    }
}
