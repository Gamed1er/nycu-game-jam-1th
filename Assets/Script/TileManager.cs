using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    public Tilemap tilemap;

    void Awake()
    {
        Instance = this;
    }

    public MoveResult TryMove(Vector3 worldPos, Vector2Int dir)
    {
        Vector3Int currentCell = tilemap.WorldToCell(worldPos);
        Vector3Int targetCell = currentCell + new Vector3Int(dir.x, dir.y, 0);

        TileData currentTile = tilemap.GetTile<TileData>(currentCell);
        TileData targetTile = tilemap.GetTile<TileData>(targetCell);

        MoveResult result = new MoveResult(true, tilemap.GetCellCenterWorld(currentCell), currentTile.chargeStation, currentTile.surfaceType);

        // 無法通過
        if (targetTile == null || !targetTile.ableToMove)
        {
            result.canMove = false;
            return result;
        }

        // 可以通過
        if (targetTile.surfaceType == SurfaceType.Ice)
        {
            result = TryMove(targetCell, dir);
            result.canMove = true;
        }
        else
        {
            result.canMove = true;
            result.targetWorldPos = tilemap.GetCellCenterWorld(targetCell);
            result.charge = targetTile.chargeStation;
            result.surfaceType = targetTile.surfaceType;
        }

        
        return result;
    }
}

public struct MoveResult
{
    public bool canMove;
    public Vector3 targetWorldPos;

    public bool charge;
    public SurfaceType surfaceType;
    
    public MoveResult(bool a, Vector3 b, bool c, SurfaceType d)
    {
        canMove = a;
        targetWorldPos = b;
        charge = c;
        surfaceType = d;
    }
}
