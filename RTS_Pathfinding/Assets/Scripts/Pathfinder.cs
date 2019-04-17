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

    PriorityQueue<Node> m_frontierNodes;
    List<Node> m_exploredNodes;
    List<Node> m_pathNodes;

    int m_iterations;
    public bool showIterations = false;
    public float timeStepIterations = .1f;
    
    public SearchAlgorithm configuredAlgorithm = SearchAlgorithm.BreadthFirst;

    bool isComplete = false;

    public enum SearchAlgorithm
    {
        BreadthFirst,
        Dijkstra,
        GreedyBestFirst,
        AStar
    }

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
        m_startNode.distanceFromStart = 0;
        m_startNode.costsFromStart = 0;
        m_goalNode = m_graph.GetNodeAtPosition(goalPosX, goalPosY);

        m_frontierNodes = new PriorityQueue<Node>();
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

                if (SearchAlgorithm.BreadthFirst.Equals(configuredAlgorithm))
                {
                    ExpandFrontierBreadthFirst(currentNode);
                }
                else if (SearchAlgorithm.Dijkstra.Equals(configuredAlgorithm))
                {
                    ExpandFrontierDijkstra(currentNode);
                }
                else if (SearchAlgorithm.GreedyBestFirst.Equals(configuredAlgorithm))
                {
                    ExpandFrontierGreedyBestFirst(currentNode);
                }
                else 
                {
                    ExpandFrontierAStar(currentNode);
                }

                if (m_frontierNodes.Contains(m_goalNode))
                {
                    m_pathNodes = GetPathNodes(m_goalNode);
                    isComplete = true;
                    Debug.Log("Path has been found in " + m_iterations + " iterations!");
                    Debug.Log("Path distance " + m_goalNode.distanceFromStart + "!");
                    Debug.Log("Path costs " + m_goalNode.costsFromStart + "!");
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

    void ExpandFrontierBreadthFirst(Node node)
    {
        if (node == null)
        {
            return;
        }

        for (int i = 0; i < node.adjacentNodes.Count; i++)
        {
            if (!m_exploredNodes.Contains(node.adjacentNodes[i]) && !m_frontierNodes.Contains(node.adjacentNodes[i]))
            {
                //START BLOCK the values are only included for debugging purposes, they are not used for priority - comment out for more performance
                float distanceToAdjacent = m_graph.DetermineDistanceBetweenNodes(node, node.adjacentNodes[i]);
                float newDistanceFromStart = node.distanceFromStart + distanceToAdjacent;
                float newCostsFromStart = node.costsFromStart + distanceToAdjacent + node.hazardValue;
                node.adjacentNodes[i].distanceFromStart = newDistanceFromStart;
                node.adjacentNodes[i].costsFromStart = newCostsFromStart;
                //END BLOCK

                node.adjacentNodes[i].previousNode = node;

                //this way it still works with priorityqueue - because it just counts up
                node.adjacentNodes[i].priority = m_exploredNodes.Count;
                //Debug.Log("Explored nodes count: "+ m_exploredNodes.Count);

                m_frontierNodes.Enqueue(node.adjacentNodes[i]);
            }
        }
    }

    void ExpandFrontierDijkstra(Node node)
    {
        if (node == null)
        {
            return;
        }

        for (int i = 0; i < node.adjacentNodes.Count; i++)
        {
            if (!m_exploredNodes.Contains(node.adjacentNodes[i]))
            {
                float distanceToAdjacent = m_graph.DetermineDistanceBetweenNodes(node, node.adjacentNodes[i]);
                float newDistanceFromStart = node.distanceFromStart + distanceToAdjacent;
                float newCostsFromStart = node.costsFromStart + distanceToAdjacent + node.hazardValue; //this way the terraincosts get included

                //if (float.IsPositiveInfinity(node.adjacentNodes[i].distanceFromStart)
                //        || distanceFromStart < node.adjacentNodes[i].distanceFromStart)
                if (float.IsPositiveInfinity(node.adjacentNodes[i].costsFromStart)
                        || newCostsFromStart < node.adjacentNodes[i].costsFromStart)

                {
                    node.adjacentNodes[i].previousNode = node;
                    node.adjacentNodes[i].distanceFromStart = newDistanceFromStart;
                    node.adjacentNodes[i].costsFromStart = newCostsFromStart;
                }

                if (!m_frontierNodes.Contains(node.adjacentNodes[i]))
                {
                    //node.adjacentNodes[i].priority = node.adjacentNodes[i].distanceFromStart;
                    node.adjacentNodes[i].priority = node.adjacentNodes[i].costsFromStart;
                    m_frontierNodes.Enqueue(node.adjacentNodes[i]);
                }

               
            }   
        }
    }

    void ExpandFrontierGreedyBestFirst(Node node)
    {
        if (node == null)
        {
            return;
        }

        for (int i = 0; i < node.adjacentNodes.Count; i++)
        {
            if (!m_exploredNodes.Contains(node.adjacentNodes[i]) && !m_frontierNodes.Contains(node.adjacentNodes[i]))
            {
                //START BLOCK the values are only included for debugging purposes, they are not used for priority - comment out for more performance
                float distanceToAdjacent = m_graph.DetermineDistanceBetweenNodes(node, node.adjacentNodes[i]);
                float newDistanceFromStart = node.distanceFromStart + distanceToAdjacent;
                float newCostsFromStart = node.costsFromStart + distanceToAdjacent + node.hazardValue;
                node.adjacentNodes[i].distanceFromStart = newDistanceFromStart;
                node.adjacentNodes[i].costsFromStart = newCostsFromStart;
                //END BLOCK

                node.adjacentNodes[i].previousNode = node;

                if (m_graph != null)
                {
                    //node.adjacentNodes[i].priority = m_graph.DetermineDistanceBetweenNodes(node.adjacentNodes[i], m_goalNode);
                    node.adjacentNodes[i].priority = m_graph.DetermineManhattanDistanceBetweenNodes(node.adjacentNodes[i], m_goalNode);
                }

                m_frontierNodes.Enqueue(node.adjacentNodes[i]);
            }
        }
    }


    //see https://en.wikipedia.org/wiki/A*_search_algorithm
    void ExpandFrontierAStar(Node node)
    {
        if (node == null)
        {
            return;
        }

        for (int i = 0; i < node.adjacentNodes.Count; i++)
        {
            if (!m_exploredNodes.Contains(node.adjacentNodes[i]))
            {
                float distanceToAdjacent = m_graph.DetermineDistanceBetweenNodes(node, node.adjacentNodes[i]);
                float newDistanceFromStart = node.distanceFromStart + distanceToAdjacent;
                float newCostsFromStart = node.costsFromStart + distanceToAdjacent + node.hazardValue; //this way the terraincosts get included
                
                if (float.IsPositiveInfinity(node.adjacentNodes[i].costsFromStart)
                        || newCostsFromStart < node.adjacentNodes[i].costsFromStart)

                {
                    node.adjacentNodes[i].previousNode = node;
                    node.adjacentNodes[i].distanceFromStart = newDistanceFromStart;
                    node.adjacentNodes[i].costsFromStart = newCostsFromStart;
                }

                if (!m_frontierNodes.Contains(node.adjacentNodes[i]) && m_graph != null)
                {
                    float distanceToGoal = m_graph.DetermineDistanceBetweenNodes(node.adjacentNodes[i],m_goalNode);

                    //AStar: f = g + h
                    node.adjacentNodes[i].priority = node.adjacentNodes[i].costsFromStart + distanceToGoal;
                    m_frontierNodes.Enqueue(node.adjacentNodes[i]);
                }


            }
        }
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
            graphView.PaintNodes(new List<Node>(m_frontierNodes.ToList()), graphView.colorBorder);
            graphView.PaintNodesDependentOnNodeType(m_exploredNodes);
            graphView.PaintNodes(m_pathNodes, graphView.colorPath);
            graphView.PaintNode(m_startNode, graphView.colorStart);
            graphView.PaintNode(m_goalNode, graphView.colorGoal);
        }
    }
}
