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
    public bool CanBePath { get; private set; }
    public float FCost => HCost + GCost;

    protected Vector2Int[] _directions;

    private void Awake() 
    { 
        CanBePath = true;
        Model = GetComponent<NodeModel>();

        Neighbours = new List<Node>();
    }

    private void OnMouseDown() => OnNodeClick?.Invoke(this);

    public abstract float CalculateDistanceTo(Node otherNode);
    protected abstract void InitializeDirections();

    public virtual void Initialize(int x, int y, Vector2 offset) 
    {
        Coordinates = new Vector2Int(x, y);
        InitializeDirections();
    }

    public virtual void Initialize(Vector2Int newCoordinates, Vector2 offset) 
    {
        Coordinates = newCoordinates;
        InitializeDirections();
    } 

    public void FindNeighbours(Board currentMap) 
    {
        Neighbours.Clear();

        foreach (Vector2Int direction in _directions)
        {
            Node neighbourNode = currentMap.TryGetNodeByNodeCoord(direction + Coordinates);

            if (neighbourNode != null) 
                Neighbours.Add(neighbourNode);
        }
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

    public void BecomeWall() 
    {
        CanBePath = false;

        Model.MarkAsWall();
    }

    public void DestroyNode() => Model.ClearNode();
    public void SetRoot(Node newRoot) => Root = newRoot;
}
