using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    int m_mapWidth;
    public int MapWidth { get { return m_mapWidth; } }

    int m_mapHeight;
    public int MapHeight { get { return m_mapHeight; } }

    void DetermineMapBoundaries()
    {
        //TODO set map height and width
    }

    void Awake()
    {
        DetermineMapBoundaries();
    }

    public Node.NodeType GetNodeTypeFromTileMap(int xPosition, int yPosition)
    {
        //TODO get the right nodeType

        return Node.NodeType.blockedTerrain;
    }
}
