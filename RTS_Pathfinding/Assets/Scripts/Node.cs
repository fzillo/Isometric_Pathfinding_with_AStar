using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    int m_xPosition;
    public int PositionX { get { return m_xPosition; } }
    int m_yPosition;
    public int PositionY { get { return m_yPosition; } }

    NodeType m_nodeType;
    public NodeType Type { get { return m_nodeType; } }

    public List<Node> adjacentNodes;

    public Node previousNode;

    //public float distanceTraveled;

    public enum NodeType
    {
        blockedTerrain=0,
        easyTerrain=1,
        mediumTerrain=2,
        hardTerrain=3
    }

    public Node(int xPosition, int yPosition, NodeType nodeType)
    {
        this.m_xPosition = xPosition;
        this.m_yPosition = yPosition;
        this.m_nodeType = nodeType;
    }
}
