using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public MapData mapData;

    Graph m_graph;
    public GraphView graphView;

    Node m_startNode;
    Node m_goalNode;

    Queue<Node> m_frontierNodes;
    List<Node> m_exploredNodes;
    List<Node> m_pathNodes;

    int m_iterations;
    public bool showIterations = false;
    public float timeStepIterations = .1f;

    bool isComplete = false;

    public void Init(int startPosX, int startPosY, int goalPosX, int goalPosY)
    {
        if (mapData == null)
        {
            Debug.LogWarning("Could not initialize pathfinder!");
            return;
        }

        int[,] map = mapData.CreateMap();
        m_graph = new Graph(map);
        //graphView = GetComponentInChildren<GraphView>();

        //m_startNode = m_graph.GetNodeAtPosition(mapData.startX, mapData.startY);
        //m_goalNode = m_graph.GetNodeAtPosition(mapData.startX, mapData.startY);
        m_startNode = m_graph.GetNodeAtPosition(startPosX, startPosY);
        m_goalNode = m_graph.GetNodeAtPosition(goalPosX, goalPosY);
        
        m_frontierNodes = new Queue<Node>();
        m_frontierNodes.Enqueue(m_startNode);
        m_exploredNodes = new List<Node>();
        m_pathNodes = new List<Node>();
                
        isComplete = false;
        m_iterations = 0;

        VisualizePathfinding();
        Debug.Log("Pathfinder is initialized");
    }

    public IEnumerator GeneratePath()
    {
        Debug.Log("Starting path generating...");
        float timeStart = Time.realtimeSinceStartup;
        yield return null;

        while (!isComplete)
        {
            if (m_frontierNodes.Count > 0) {

                Node currentNode = (Node) m_frontierNodes.Dequeue();
                m_iterations++;

                if (!m_exploredNodes.Contains(currentNode))
                {
                    m_exploredNodes.Add(currentNode);
                }

                ExpandFrontierBreadthFirst(currentNode);
                //ExpandFrontierAStar(currentNode);

                if (m_frontierNodes.Contains(m_goalNode))
                {
                    m_pathNodes = GetPathNodes(m_goalNode);
                    isComplete = true;
                    Debug.Log("Path has been found in " + m_iterations + " iterations!");
                }


                if (showIterations)
                {
                    VisualizePathfinding();
                    yield return new WaitForSeconds(timeStepIterations);
                }

            }
            else
            {
                isComplete = true;
                Debug.Log("No Path has been found. " + m_iterations + " iterations.");
            }
        }


        VisualizePathfinding();
        Debug.Log("Pathfinder elapsed time: " + (Time.realtimeSinceStartup - timeStart).ToString());
    }

    private void ExpandFrontierBreadthFirst(Node currentNode)
    {
        if (currentNode == null)
        {
            return;
        }

        for (int i = 0; i < currentNode.adjacentNodes.Count; i++)
        {
            if (!m_exploredNodes.Contains(currentNode.adjacentNodes[i]) && !m_frontierNodes.Contains(currentNode.adjacentNodes[i]))
            {
                currentNode.adjacentNodes[i].previousNode = currentNode;
                m_frontierNodes.Enqueue(currentNode.adjacentNodes[i]);
            }
        }
    }

    void ExpandFrontierAStar(Node currentNode)
    {
        throw new NotImplementedException();
    }

    List<Node> GetPathNodes(Node endNode)
    {
        List<Node> path = new List<Node>();

        if (endNode == null) {
            return path;
        }

        Node currentNode = endNode;

        while (currentNode.previousNode != null)
        {
            path.Insert(0, currentNode);
            currentNode = currentNode.previousNode;
        }

        return path;
    }

    void VisualizePathfinding()
    {
        if (graphView != null)
        {
            graphView.PaintNodes(new List<Node>(m_frontierNodes), graphView.colorBorder);
            graphView.PaintNodes(m_exploredNodes, graphView.colorExplored);
            graphView.PaintNodes(m_pathNodes, graphView.colorPath);
        }
    }
}
