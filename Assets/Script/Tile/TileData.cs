using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileData : Tile
{
    [Header("Movement")]
    public bool ableToMove = true;
    public int cost = 0;

    [Header("Special")]
    
    public SurfaceType surfaceType;

    public virtual void OnEntityEnter(TileGameObject tileGameObject)
    {
        
    }

    public virtual void OnEntityExit(TileGameObject tileGameObject)
    {
        
    }
    
    public virtual void OnPowerChanged(TileGameObject tileGameObject, bool powered)
    {
        
    }

    public virtual void OnPlayerUse(TileGameObject tileGameObject)
    {
        
    }
}

public enum SurfaceType
{
    Normal,
    Ice,
    Power_In,
    Power_Node,
    Power_Out
}
