using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    Node[,] nodes;

    int m_width;
    int m_height;

    private float m_distanceUnitsStraight = 1f;
    private float m_distanceUnitsDiagonal = 1.4f; //~pythagoras of root(1^2+1^2)

    //int[,] mapData;

    public static readonly Vector2[] allDirections =
    {
        new Vector2(0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0f),
        new Vector2(1f, -1f),
        new Vector2(0f, -1f),
        new Vector2(-1f, -1f),
        new Vector2(-1f, 0f),
        new Vector2(-1f, 1f)
    };

    private Graph()
    {
    }

    public Graph(int[,] mapData)
    {
        Init(mapData);
    }

    void Init(int[,] mapData)
    {
        //if (mapManager != null)
        //{
        //    maximum_xPosition = mapManager.MapWidth;
        //    maximum_yPosition = mapManager.MapHeight;
        //} else
        //{
        //    Debug.LogWarning("MapManager not found. Initializing nodearray with default values:" +
        //        " maximum_xPosition " + maximum_xPosition + " maximum_yPosition " + maximum_yPosition);
        //}

        m_width = mapData.GetLength(0);
        m_height = mapData.GetLength(1);

        nodes = new Node[m_width, m_height];

        for (int x=0; x < m_width; x++)
        {
            for (int y = 0; y < m_height; y++)
            {
                Node.NodeType newNodeType = (Node.NodeType) mapData[x, y];
                nodes[x, y] = new Node(x, y, newNodeType);
            }
        }

        for (int x = 0; x < m_width; x++)
        {
            for (int y = 0; y < m_height; y++)
            {
                nodes[x, y].adjacentNodes = GetAdjacentNodesAtPosition(x, y);
            }
        }
    }

    public Node GetNodeAtPosition(int xPosition, int yPosition)
    {
        return nodes[xPosition, yPosition];
    }

    public bool IsWithinBounds(int xPosition, int yPosition)
    {
        return (xPosition >= 0 && xPosition < m_width && yPosition >= 0 && yPosition < m_height);
    }

    List<Node> GetAdjacentNodesAtPosition(int xPosition, int yPosition)
    {
        List<Node> adjacentNodes = new List<Node>();

        foreach (Vector2 dir in allDirections)
        {
            int newX = xPosition + (int)dir.x;
            int newY = yPosition + (int)dir.y;

            if (IsWithinBounds(newX, newY) && nodes[newX,newY] != null && nodes[newX, newY].Type != Node.NodeType.blockedTerrain)
            {
                adjacentNodes.Add(nodes[newX, newY]);
            }
        }

        return adjacentNodes;
    }

    /*
    * to find the shortest distance, you first go diagonal steps which are equal to the smaller absolute delta (x xor y),
    * then you go the remaining steps in a straight line.
    */
    public float DetermineDistanceBetweenNodes(Node nodeFrom, Node nodeTo)
    {        
        int absDeltaX = Mathf.Abs(nodeFrom.PositionX - nodeTo.PositionX);
        int absDeltaY = Mathf.Abs(nodeFrom.PositionY - nodeTo.PositionY);

        int stepsDiagonal;
        int stepsStraight;
               
        if (absDeltaX >= absDeltaY)
        {
            stepsDiagonal = absDeltaY;
            stepsStraight = absDeltaX - absDeltaY;            
        }
        else
        {
            stepsDiagonal = absDeltaX;
            stepsStraight = absDeltaY - absDeltaX;
        }

        return m_distanceUnitsDiagonal * stepsDiagonal + m_distanceUnitsStraight * stepsStraight;
    }

    /*
     * Rougher, slightly more performant estimation of distance
     * see https://en.wikipedia.org/wiki/Taxicab_geometry
     */
    public float DetermineManhattanDistanceBetweenNodes(Node nodeFrom, Node nodeTo)
    {
        int absDeltaX = Mathf.Abs(nodeFrom.PositionX - nodeTo.PositionX);
        int absDeltaY = Mathf.Abs(nodeFrom.PositionY - nodeTo.PositionY);

        return (absDeltaX + absDeltaY);
    }

}
