using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GraphView : MonoBehaviour
{

    public Tilemap tilemapGraphView;

    public Tile colorStart;
    public Tile colorGoal;
    public Tile colorEasy;
    public Tile colorMedium;
    public Tile colorHard;
    public Tile colorPath;
    //public Tile colorExplored;
    public Tile colorBorder;
    public Tile colorDefault;

    static Dictionary<Tile, Node.NodeType> nodeColorLookupTable = new Dictionary<Tile, Node.NodeType>();

    private void Awake()
    {
        SetupLookupTable();
    }

    public void PaintNodes(List<Node> nodesToPaint, Tile color)
    {
        if (nodesToPaint == null)
        {
            return;
        }

        foreach (Node node in nodesToPaint)
        {
            PaintNode(node, color);
        }
    }

    public void PaintNode(Node node, Tile color)
    {
        Vector3Int positionInTilemap = new Vector3Int(node.PositionX, node.PositionY, 0);

        tilemapGraphView.SetTile(positionInTilemap, color);
    }

    public void PaintNodesDependentOnNodeType(List<Node> nodesToPaint)
    {
        if (nodesToPaint == null)
        {
            return;
        }

        foreach (Node node in nodesToPaint)
        {
            PaintNodeDependentOnNodeType(node);
        }
    }

    public void PaintNodeDependentOnNodeType(Node node)
    {
        Vector3Int positionInTilemap = new Vector3Int(node.PositionX, node.PositionY, 0);

        Tile color = GetColorFromNodeType(node.Type);

        tilemapGraphView.SetTile(positionInTilemap, color);
    }

    void SetupLookupTable()
    {
        nodeColorLookupTable.Add(colorEasy, Node.NodeType.easyTerrain);
        nodeColorLookupTable.Add(colorMedium, Node.NodeType.mediumTerrain);
        nodeColorLookupTable.Add(colorHard, Node.NodeType.hardTerrain);
    }

    public Tile GetColorFromNodeType (Node.NodeType nodeType)
    {
        if (nodeColorLookupTable.ContainsValue(nodeType))
        {
            Tile color = nodeColorLookupTable.FirstOrDefault(x => x.Value == nodeType).Key;
            return color;
        }

        return colorDefault;
    }
}
