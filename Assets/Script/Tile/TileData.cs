using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileData : Tile
{
    [Header("Movement")]
    public bool ableToMove = true;
    public int cost = 0;

    [Header("Special")]
    public bool chargeStation;
    
    public SurfaceType surfaceType;

    public virtual void OnEntityEnter()
    {
        
    }

    public virtual void OnEntityExit()
    {
        
    }

    public virtual void OnElectricOpen()
    {
        
    }

    public virtual void OnElectricClose()
    {
        
    }
}

public enum SurfaceType
{
    Normal,
    Ice,
    Mud
}
