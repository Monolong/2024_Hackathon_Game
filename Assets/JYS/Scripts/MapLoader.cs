using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLoader : MonoBehaviour
{
    private Tilemap tilemap;
    private Vector3Int mapOffset;
    private int[,] map;

    private void Awake()
    {
        tilemap = GameObject.Find("Road").GetComponent<Tilemap>();
    }

    public int[,] GetMap()
    {
        if (map == null)
        {
            InitMap();
        }
        return map;
    }

    public Vector3Int GetMapOffset()
    {
        if (mapOffset == null)
        {
            InitMap();
        }
        return mapOffset;
    }

    private void InitMap()
    {
        BoundsInt bounds = tilemap.cellBounds;
        int maxX = 0;
        int maxY = 0;
        int minX = 0;
        int minY = 0;
        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            maxX = Mathf.Max(position.x, maxX);
            maxY = Mathf.Max(position.y, maxY);
            minX = Mathf.Min(position.x, minX);
            minY = Mathf.Min(position.y, minY);
        }

        maxX += 10;
        maxY += 10;
        minX -= 10;
        minY -= 10;

        map = new int[maxX - minX + 1, maxY - minY + 1];
        mapOffset = new Vector3Int(minX, minY);

        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            TileBase tileBase = tilemap.GetTile(position);
            if (tileBase == null)
            {
                map[position.x - mapOffset.x, position.y - mapOffset.y] = 0;
            }
            else
            {
                map[position.x - mapOffset.x, position.y - mapOffset.y] = 1;
            }
        }
    }
}
