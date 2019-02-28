using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    Node[,] nodes;

    int m_width;
    int m_height;

    int[,] mapData;

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

        for (int x=0; x <= m_width; x++)
        {
            for (int y = 0; y <= m_height; y++)
            {
                Node.NodeType newNodeType = (Node.NodeType) mapData[x, y];
                nodes[x, y] = new Node(x, y, newNodeType);
            }
        }
    }

    public Node GetNodeAtPosition(int xPosition, int yPosition)
    {
        return nodes[xPosition, yPosition];
    }
}
