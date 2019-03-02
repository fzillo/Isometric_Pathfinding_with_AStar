using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GraphView : MonoBehaviour
{

    public Tilemap tilemapGraphView;

    public Tile colorPath;
    public Tile colorExplored;
    public Tile colorBorder;
        
    public void PaintNodes(List<Node> nodesToPaint, Tile color)
    {
        if (nodesToPaint == null)
        {
            return;
        }

        foreach (Node node in nodesToPaint)
        {
            Vector3Int positionInTilemap = new Vector3Int(node.PositionX, node.PositionY, 0);
            
            tilemapGraphView.SetTile(positionInTilemap, color);
        }
    }
}
