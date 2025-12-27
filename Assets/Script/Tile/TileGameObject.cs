using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGameObject : MonoBehaviour
{
    public TileData tileData;
    public TileData wire;

    [Header("Movement")]
    public bool ableToMove = true;
    public int cost = 0;

    
    [Header("SurfaceType")]
    public SurfaceType surfaceType;

    [Header("Power")]
    public bool IsPowered;

    void Start()
    {
        
        if(transform.parent.name != "TileMap")
        {
            tileData = wire;
        }
        else
        {
            Vector3Int cell;
            cell = TileManager.Instance.tilemap.WorldToCell(transform.position);
            TileManager.Instance.RegisterTileObject(cell, this);
            tileData = TileManager.Instance.tilemap.GetTile<TileData>(cell);
        }
        
        ableToMove = tileData.ableToMove;
        cost = tileData.cost;
        surfaceType = tileData.surfaceType;

        GetComponent<SpriteRenderer>().sprite = tileData.sprite;

        name = tileData.name;

        if(tileData.surfaceType == SurfaceType.Power_In || tileData.surfaceType == SurfaceType.Power_Out)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
    }

}