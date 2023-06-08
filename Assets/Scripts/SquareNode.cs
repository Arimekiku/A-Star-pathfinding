using UnityEngine;

public class SquareNode : Node
{
    public override void Initialize(int x, int y, Vector2 offset)
    {
        base.Initialize(x, y, offset);

        transform.position = new Vector2(x, y) - offset;
    }

    public override void Initialize(Vector2Int newCoordinates, Vector2 offset)
    {
        base.Initialize(newCoordinates, offset);

        transform.position = newCoordinates - offset;
    }

    public override float CalculateDistanceTo(Node otherNode) 
    {
        float xValue = Mathf.Abs(this.Coordinates.x - otherNode.Coordinates.x);
        float yValue = Mathf.Abs(this.Coordinates.y - otherNode.Coordinates.y);

        return xValue + yValue; 
    }

    protected override void InitializeDirections() 
    {
        _directions = new Vector2Int[4] 
        {
            new Vector2Int( 1,  0),
            new Vector2Int(-1,  0),
            new Vector2Int( 0,  1),
            new Vector2Int( 0, -1)
        };
    }
}
