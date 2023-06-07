using System.Collections.Generic;
using System.Linq;

public class PathFinder
{
    private List<Node> _nodesToExplore = new List<Node>();
    private List<Node> _exploredNodes = new List<Node>();

    public List<Node> Path(Node startNode, Node targetNode) 
    {
        _nodesToExplore.Clear();
        _nodesToExplore.Add(startNode);

        _exploredNodes.Clear();

        int i = 0;

        while (_nodesToExplore.Count != 0) 
        {
            i++;
            Node currentNode = _nodesToExplore[0];
            foreach (Node node in _nodesToExplore) 
                if (node.FCost <= currentNode.FCost && node.HCost < currentNode.HCost)   
                    currentNode = node;
            
            _nodesToExplore.Remove(currentNode);
            _exploredNodes.Add(currentNode);

            if (currentNode == targetNode) 
                return ReclaimPath(startNode, targetNode);
                

            foreach (Node neighbour in currentNode.Neighbours.Where(n => _exploredNodes.Contains(n) is false)) 
            {
                bool nodeExploring = _nodesToExplore.Contains(neighbour);
                float potentialGCost = currentNode.GCost + currentNode.CalculateDistanceTo(neighbour);

                if (nodeExploring is false || potentialGCost < neighbour.GCost) 
                {
                    neighbour.SetGCost(potentialGCost);
                    neighbour.SetRoot(currentNode);

                    if (nodeExploring is false) 
                    {
                        neighbour.SetHCost(neighbour.CalculateDistanceTo(targetNode));
                        _nodesToExplore.Add(neighbour);
                    }
                }
            }  
        }

        return null;
    }

    private List<Node> ReclaimPath(Node startNode, Node targetNode) 
    {
        List<Node> resultPath = new List<Node>();
        Node currentNode = targetNode;

        while (currentNode != startNode)
        {
            resultPath.Add(currentNode);
            currentNode = currentNode.Root;
        }

        return resultPath;
    }
}