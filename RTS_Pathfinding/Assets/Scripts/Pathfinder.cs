using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public MapData mapData;

    Graph m_graph;
    Node m_startNode;
    Node m_goalNode;
    
    void Start()
    {
        if (mapData == null)
        {
            Debug.LogWarning("Could not initialize pathfinder!");
            return;
        }
        m_graph = new Graph();
        m_startNode = m_graph.GetNodeAtPosition(mapData.startX, mapData.startY);
        m_goalNode = m_graph.GetNodeAtPosition(mapData.startX, mapData.startY);
    }
}
