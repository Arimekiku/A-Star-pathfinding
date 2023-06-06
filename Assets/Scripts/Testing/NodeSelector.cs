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
        _currentStartNode.MarkAsStart();

        foreach (Node node in _currentBoard.GetNodes()) 
        {
            if (node == _currentStartNode)
                continue;
            
            node.OnNodeClick += ClickedOn;
        }
    }

    private void ClickedOn(Node newTargetNode) 
    {
        _currentTargetNode?.ClearMark();
        _currentTargetNode = newTargetNode;

        _currentPath.ForEach(n => n.ClearMark());
        _currentPath = new List<Node>(_pathFinder.Path(_currentStartNode, _currentTargetNode));
        _currentPath.ForEach(n => n.MarkAsPath());

        _currentTargetNode.MarkAsTarget();
    }
}
