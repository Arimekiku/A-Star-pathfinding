using System;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public event Action<Node> OnNodeClick;
    public List<Node> Neighbours { get; private set; }
    public Node Root { get; private set; }
    public Vector2Int Coordinates { get; private set; }
    public float HCost { get; private set; }
    public float GCost { get; private set; }
    public float FCost => HCost + GCost;

    private SpriteRenderer _renderer;

    private static readonly Vector2Int[] _directions = new Vector2Int[4] 
    {
        new Vector2Int( 1,  0),
        new Vector2Int(-1,  0),
        new Vector2Int( 0,  1),
        new Vector2Int( 0, -1)
    };

    private void Awake() => _renderer = GetComponent<SpriteRenderer>();   

    private void OnMouseDown() => OnNodeClick?.Invoke(this);

    public void FindNeighbours(Board currentMap) 
    {
        Neighbours = new List<Node>();
    
        foreach (Vector2Int direction in _directions)
        {
            Node neighbourNode = currentMap.TryGetNode(direction + Coordinates);

            if (neighbourNode != null) 
                Neighbours.Add(neighbourNode);
        }
    }

    public float CalculateDistanceTo(Node otherNode) 
    {
        float xValue = Mathf.Abs(this.Coordinates.x - otherNode.Coordinates.x);
        float yValue = Mathf.Abs(this.Coordinates.y - otherNode.Coordinates.y);

        return xValue + yValue; 
    } 

    public void SetHCost(float newCost) 
    {
        if (newCost < 0 )
            throw new ArgumentException("Invalid cost value");
        
        HCost = newCost;
    }

    public void SetGCost(float newCost) 
    {
        if (newCost < 0 )
            throw new ArgumentException("Invalid cost value");
        
        GCost = newCost;
    }

    public void Initialize(int x, int y) => Coordinates = new Vector2Int(x, y);
    public void Initialize(Vector2Int newCoordinates) => Coordinates = newCoordinates;

    public void SetRoot(Node newRoot) => Root = newRoot;

    public void MarkAsTarget() => _renderer.material.color = Color.black;
    public void MarkAsStart() => _renderer.material.color = Color.cyan;
    public void MarkAsPath() => _renderer.material.color = Color.blue;
    public void ClearMark() => _renderer.material.color = Color.white;
}
