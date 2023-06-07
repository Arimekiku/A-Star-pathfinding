using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Node _nodePrefab;

    private Vector2 Offset => new Vector2((_boardSize.x - 1) * 0.5f, (_boardSize.y - 1) * 0.5f); 

    private Vector2Int _boardSize;
    private Node[,] _nodes;

    public IEnumerator Initialize(Vector2Int newBoardSize) 
    {
        _boardSize = newBoardSize;
        _nodes = new Node[_boardSize.x, _boardSize.y];

        for (int y = 0; y < _boardSize.y; y++) 
        {
            for (int x = 0; x < _boardSize.x; x++) 
            {
                Node newNode = _nodes[x, y] = Instantiate(_nodePrefab, transform);
                newNode.Initialize(x, y);
                newNode.transform.position = new Vector2Int(x, y) - Offset;
                newNode.name = "Node " + x + " " + y;
            }

            yield return new WaitForSeconds(0.05f);            
        }

        foreach (Node node in _nodes) 
            node.FindNeighbours(this);
    }

    public Node TryGetNode(int x, int y) 
    {
        if (x >= _boardSize.x || x < 0)
            return null;

        if (y >= _boardSize.y || y < 0)
            return null;

        return _nodes[x, y];
    }

    public Node TryGetNode(Vector2Int nodeCoordinates) 
    {
        if (nodeCoordinates.x >= _boardSize.x || nodeCoordinates.x < 0)
            return null;

        if (nodeCoordinates.y >= _boardSize.y || nodeCoordinates.y < 0)
            return null;

        return _nodes[nodeCoordinates.x, nodeCoordinates.y];
    }

    public List<Node> GetNodes() 
    {
        List<Node> nodes = new List<Node>();

        for (int y = 0; y < _boardSize.y; y++)
            for (int x = 0; x < _boardSize.x; x++)
                nodes.Add(_nodes[x, y]);
        
        return nodes;
    }
}
