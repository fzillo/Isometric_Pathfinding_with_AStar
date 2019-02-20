using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    Node[,] nodes;

    int maximum_xPosition = 31;
    int maximum_yPosition = 31;

    public MapManager mapManager;

    void Awake()
    {
        if (mapManager != null)
        {
            maximum_xPosition = mapManager.MapWidth;
            maximum_yPosition = mapManager.MapHeight;
        } else
        {
            Debug.LogWarning("MapManager not found. Initializing nodearray with default values:" +
                " maximum_xPosition " + maximum_xPosition + " maximum_yPosition " + maximum_yPosition);
        }
        nodes = new Node[maximum_xPosition, maximum_yPosition];

        for (int x=0; x <= maximum_xPosition; x++)
        {
            for (int y = 0; y <= maximum_yPosition; y++)
            {
                Node.NodeType newNodeType = mapManager.GetNodeTypeFromTileMap(x, y);
                nodes[x, y] = new Node(x, y, newNodeType);
            }
        }
    }
}
