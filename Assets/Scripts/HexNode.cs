using UnityEngine;

public class HexNode : Node
{
    public override float CalculateDistanceTo(Node otherNode) 
    {
        float xValue = Mathf.Abs(this.Coordinates.x - otherNode.Coordinates.x);
        float yValue = Mathf.Abs(this.Coordinates.y - otherNode.Coordinates.y);
        float zValue = Mathf.Abs(- xValue - yValue);

        return xValue + yValue + zValue; 
    } 

    public override void Initialize(int x, int y, Vector2 offset)
    {
        base.Initialize(x - y / 2, y, offset);

        transform.position = new Vector2(x + y * 0.5f - y / 2, y) - offset;
    }

    public override void Initialize(Vector2Int newCoordinates, Vector2 offset)
    {
        int xCoord = newCoordinates.x;
        int yCoord = newCoordinates.y;
        base.Initialize(new Vector2Int(xCoord - yCoord / 2, yCoord), offset);
        
        transform.position = new Vector2(xCoord + yCoord * 0.5f - yCoord / 2, yCoord) - offset;
    }

    protected override void InitializeDirections() 
    {
        _directions = new Vector2Int[6] 
        {
            new Vector2Int( 1,  0),
            new Vector2Int(-1,  0),
            new Vector2Int( 0,  1),
            new Vector2Int( 0, -1),
            new Vector2Int(-1,  1),
            new Vector2Int( 1, -1)
        };
    }
}
