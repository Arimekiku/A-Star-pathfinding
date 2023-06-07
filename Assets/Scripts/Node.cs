using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour
{
    public event Action<Node> OnNodeClick;
    public NodeModel Model { get; private set; }
    public List<Node> Neighbours { get; private set; }
    public Node Root { get; private set; }
    public Vector2Int Coordinates { get; private set; }
    public float HCost { get; private set; }
    public float GCost { get; private set; }
    public float FCost => HCost + GCost;

    private void Awake() 
    { 
        Model = GetComponent<NodeModel>();

        Neighbours = new List<Node>();
    }

    private void OnMouseDown() => OnNodeClick?.Invoke(this);

    public abstract float CalculateDistanceTo(Node otherNode);
    public abstract void FindNeighbours(Board currentMap);

    public virtual void Initialize(int x, int y, Vector2 offset) => Coordinates = new Vector2Int(x, y);
    public virtual void Initialize(Vector2Int newCoordinates, Vector2 offset) => Coordinates = newCoordinates;

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

    public void SetRoot(Node newRoot) => Root = newRoot;
}
