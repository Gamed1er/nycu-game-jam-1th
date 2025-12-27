using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileData : Tile
{
    [Header("Movement")]
    public bool ableToMove = true;

    [Header("Special")]
    public bool chargeStation;
    
    public SurfaceType surfaceType;

    public virtual void OnPlayerEnter()
    {
        
    }
}

public enum SurfaceType
{
    Normal,
    Ice,
    Mud
}
