using UnityEngine;

public class Node : IHeapItem<Node>
{
    public Vector3 worldPoint;
    public bool walkable;
    public Node parent;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public int movementPenalty;

    private int heapIndex;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(bool walkable, Vector3 worldPoint, int gridX, int gridY, int movementPenalty)
    {
        this.worldPoint = worldPoint;
        this.walkable = walkable;
        this.gridX = gridX;
        this.gridY = gridY;
        this.movementPenalty = movementPenalty;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
