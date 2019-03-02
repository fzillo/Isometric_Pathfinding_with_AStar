using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapData : MonoBehaviour
{
    public Tilemap tilemapTerrain;
    
    [SerializeField]
    int m_mapWidth;
    public int MapWidth { get { return m_mapWidth; } }

    [SerializeField]
    int m_mapHeight;
    public int MapHeight { get { return m_mapHeight; } }

    public TileBase deadTerrain;
    public TileBase grassTerrain;
    public TileBase sandTerrain;
    public TileBase waterTerrain;


    //public TileBase blockedTerrain;
    //public TileBase easyTerrain;
    //public TileBase mediumTerrain;
    //public TileBase hardTerrain;

    Dictionary<TileBase, Node.NodeType> terrainDifficultyDict = new Dictionary<TileBase, Node.NodeType>();

    void DetermineMapBoundaries()
    {
        if(tilemapTerrain != null) { 
            Vector3 tilemapSize = tilemapTerrain.size;
            m_mapWidth = (int) tilemapSize.x;
            m_mapHeight = (int) tilemapSize.y;
            Debug.Log("The Size of the tilemap is " + m_mapWidth + " by " + m_mapHeight);
        } else
        {
            Debug.LogWarning("Tilemap is not set! Could not determine map height or width!");
        }
    }

    void Awake()
    {
        DetermineMapBoundaries();
        SetupTerrainDifficulty();
        //GetNodeTypeAtPosition(15, 15);
    }

    void SetupTerrainDifficulty()
    {
        if (deadTerrain == null || grassTerrain == null || sandTerrain == null || waterTerrain == null)
        {
            Debug.LogWarning("Could not setup terrain difficulty!");
            return;
        }

        terrainDifficultyDict.Add(deadTerrain, Node.NodeType.blockedTerrain);
        terrainDifficultyDict.Add(grassTerrain, Node.NodeType.easyTerrain);
        terrainDifficultyDict.Add(sandTerrain, Node.NodeType.mediumTerrain);
        terrainDifficultyDict.Add(waterTerrain, Node.NodeType.hardTerrain);       
    }

    public Node.NodeType GetNodeTypeAtPosition(int xPosition, int yPosition)
    {
        TileBase tile = tilemapTerrain.GetTile(new Vector3Int(xPosition, yPosition, 0));

        if(terrainDifficultyDict.ContainsKey(tile))
        {
            Node.NodeType nodeTypeTile = terrainDifficultyDict[tile];
            //Debug.Log("The tile "+ tile.ToString() + " at position " + xPosition + "," + yPosition + " is classified as type " + nodeTypeTile.ToString());
            
            return nodeTypeTile;
        }

        return Node.NodeType.blockedTerrain;
    }

    public int[,] CreateMap()
    {
        int[,] map = new int[m_mapWidth, m_mapHeight];

        for (int x = 0; x < m_mapWidth; x++)
        {
            for (int y = 0; y < m_mapHeight; y++)
            {
                map[x, y] = (int) GetNodeTypeAtPosition(x, y);
            }
        }

        return map;
    }
}
