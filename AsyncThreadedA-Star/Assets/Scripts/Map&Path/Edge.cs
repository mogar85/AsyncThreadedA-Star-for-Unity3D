using System;

[Serializable]
public class Edge
{
    public Node Node1, Node2;

    public Edge(Node node1, Node node2)
    {
        Node1 = node1;
        Node2 = node2;
    }
}
