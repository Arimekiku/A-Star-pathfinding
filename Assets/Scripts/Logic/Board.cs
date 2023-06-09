using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Vector2 Offset => new Vector2((_boardSize.x - 1) * 0.5f, (_boardSize.y - 1) * 0.5f);

    private Vector2Int _boardSize;
    private Node[,] _nodes;
    private int _wallSpawnChance = 3;

    public event Action OnInitializeFinished;

    public IEnumerator Initialize(Vector2Int newBoardSize, Node newNodePrefab) 
    {
        _boardSize = newBoardSize;
        _nodes = new Node[_boardSize.x, _boardSize.y];

        for (int x = 0; x < _boardSize.x; x++) 
        {
            for (int y = 0; y < _boardSize.y; y++) 
            {
                //Instantiate nodes
                Node newNode = _nodes[x, y] = Instantiate(newNodePrefab, transform);
                newNode.Initialize(x, y, Offset);
                newNode.name = "Node " + x + " " + y;

                //Spawn walls
                bool shouldBecomeWall = UnityEngine.Random.Range(0, 11) > _wallSpawnChance ? false : true;
                if (shouldBecomeWall is true) 
                {
                    newNode.BlockPath();
                    newNode.Model.MarkAsWall();
                }   
            }

            yield return new WaitForSeconds(0.05f);            
        }

        //Initialize neighbours in nodes
        foreach (Node node in _nodes) 
        { 
            node.Neighbours.Clear();

            foreach (Vector2Int direction in node.Directions)
            {
                Node neighbourNode = TryGetNodeByNodeCoord(direction + node.Coordinates);

                if (neighbourNode is not null) 
                    node.Neighbours.Add(neighbourNode);
            }
        }
        
        OnInitializeFinished.Invoke();
    }

    public Node TryGetNodeByBoardCoord(int x, int y) 
    {
        if (x >= _boardSize.x || x < 0)
            return null;
        
        if (y >= _boardSize.y || y < 0)
            return null;

        return _nodes[x, y];
    }

    public Node TryGetNodeByNodeCoord(int x, int y) 
    {
        foreach (Node node in _nodes) 
        {
            if (node.Coordinates == new Vector2Int(x, y))
                return node;
        }
        
        return null;
    }

    public Node TryGetNodeByNodeCoord(Vector2Int nodeCoordinates) 
    {
        foreach (Node node in _nodes) 
        {
            if (node?.Coordinates == nodeCoordinates)
                return node;
        }
        
        return null;
    }

    public List<Node> GetNodes() 
    {
        List<Node> nodes = new List<Node>();

        for (int y = 0; y < _boardSize.y; y++)
            for (int x = 0; x < _boardSize.x; x++)
                nodes.Add(_nodes[x, y]);
        
        return nodes;
    }

    public void ClearNodes() 
    {
        StopAllCoroutines();

        foreach (Node node in _nodes)
            node?.Model.ClearNode();
    }

    public Node SelectRandomPoint()
    {
        int xCoord = UnityEngine.Random.Range(0, _boardSize.x);
        int yCoord = UnityEngine.Random.Range(0, _boardSize.y);

        return _nodes[xCoord, yCoord];
    }
}
