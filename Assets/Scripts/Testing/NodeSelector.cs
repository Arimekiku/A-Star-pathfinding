using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeSelector : MonoBehaviour
{
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private SquareNode _squareNodePrefab;
    [SerializeField] private HexNode _hexNodePrefab;

    private Board _currentBoard;
    private Node _currentTargetNode;
    private Node _currentStartNode;
    private List<Node> _currentPath = new List<Node>();
    private PathFinder _pathFinder = new PathFinder();

    private void Awake() 
    {
        _currentBoard = FindObjectOfType<Board>();
        _currentBoard.OnInitializeFinished += () => {
            List<Node> nodesOnField = _currentBoard.GetNodes();

            foreach (Node node in nodesOnField) 
            {
                node.Model.EnableCollider();

                node.OnNodeClick += ClickedOnNode;
            } 

            PickRandomStartNode();
        };
    } 

    private void Start() => StartCoroutine(_currentBoard.Initialize(_boardSize, _squareNodePrefab));
    
    private void ClickedOnNode(Node newTargetNode) 
    {
        if (newTargetNode.CanBePath == false)
            return;
        
        _currentTargetNode?.Model.ClearMark();
        _currentTargetNode = newTargetNode;

        StartCoroutine(ClearAndCreatePath());
    }

    public void ClickedOnSwitch() 
    {
        Node lastNode = _currentBoard.TryGetNodeByNodeCoord(0, 0);

        if (lastNode == null)
            return;
        
        //Clear all what needs to be clear
        _currentStartNode = null;
        _currentTargetNode = null;
        _currentPath.Clear();
        _currentBoard.ClearNodes();
        
        //Initialize new grid
        if (lastNode is HexNode)
            StartCoroutine(_currentBoard.Initialize(_boardSize, _squareNodePrefab));
        else 
            StartCoroutine(_currentBoard.Initialize(_boardSize, _hexNodePrefab));
    }

    public void ClickedOnStart() 
    {   
        StartCoroutine(ClearPath());

        _currentPath.Clear();
        
        _currentStartNode?.Model.ClearMark();
        _currentStartNode.OnNodeClick += ClickedOnNode;

        _currentTargetNode?.Model.ClearMark();
        _currentTargetNode = null;

        PickRandomStartNode();
    }

    private void PickRandomStartNode() 
    {
        _currentStartNode = _currentBoard.SelectRandomPoint();
        while (_currentStartNode.CanBePath == false) 
            _currentStartNode = _currentBoard.SelectRandomPoint();

        _currentStartNode.Model.MarkAsStart(); 
        _currentStartNode.OnNodeClick -= ClickedOnNode;
    }

    private IEnumerator AnimatePath() 
    {
        _currentPath.Reverse();

        foreach (Node currentNode in _currentPath) 
        {
            if (currentNode == _currentTargetNode) 
            {
                currentNode.Model.MarkAsTarget();
                yield break;
            }

            yield return new WaitForSeconds(0.02f);

            currentNode.Model.MarkAsPath();
        }
    }

    private IEnumerator ClearPath() 
    {
        List<Node> pathCopy = new List<Node>(_currentPath);

        foreach (Node currentNode in pathCopy) 
        {
            yield return new WaitForSeconds(0.02f);

            currentNode.Model.ClearMark();
        }
    }

    private IEnumerator ClearAndCreatePath() 
    {
        StartCoroutine(ClearPath());

        if (_currentPath.Count is not 0)
            yield return new WaitForSeconds(0.5f);

        _currentPath = new List<Node>(_pathFinder.Path(_currentStartNode, _currentTargetNode));

        StartCoroutine(AnimatePath());
    }

    private IEnumerator DisableButton(Button buttonToDisable) 
    {
        buttonToDisable.interactable = false;

        yield return new WaitForSeconds(1.5f);

        buttonToDisable.interactable = true;
    }

    public void DisableButtonForSeconds(Button button) => StartCoroutine(DisableButton(button));
}