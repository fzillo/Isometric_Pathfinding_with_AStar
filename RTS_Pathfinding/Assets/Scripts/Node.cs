using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : IComparable<Node>
{
    int m_xPosition;
    public int PositionX { get { return m_xPosition; } }
    int m_yPosition;
    public int PositionY { get { return m_yPosition; } }

    NodeType m_nodeType;
    public NodeType Type { get { return m_nodeType; } }

    public List<Node> adjacentNodes;

    public Node previousNode;

    public int priority;

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

    // used for sorting order priorityqueue
    public int CompareTo(Node other)
    {
        if(this.priority < other.priority)
        {
            return -1;
        } else if (this.priority > other.priority)
        {
            return 1;
        } else
        {
            return 0;
        }
    }
}
