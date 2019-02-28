using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    int m_xPosition;
    int m_yPosition;
    NodeType m_nodeType;

    List<Node> adjacentNodes;

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
