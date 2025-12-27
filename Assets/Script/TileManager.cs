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

        MoveResult result = new MoveResult(true, tilemap.GetCellCenterWorld(currentCell), 0, currentTile.surfaceType);

        // 無法通過
        if (targetTile == null || !targetTile.ableToMove)
        {
            result.canMove = false;
            return result;
        }

        // 玩家踩上去的效果
        currentTile.OnEntityExit();
        targetTile.OnEntityEnter();
        
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
            result.total_cost += targetTile.cost;
            result.surfaceType = targetTile.surfaceType;
        }
        
        // 執行被踩過的效果
        return result;
    }
}

public struct MoveResult
{
    public bool canMove;
    public Vector3 targetWorldPos;

    public int total_cost;
    public SurfaceType surfaceType;
    
    public MoveResult(bool a, Vector3 b, int c, SurfaceType d)
    {
        canMove = a;
        targetWorldPos = b;
        total_cost = c;
        surfaceType = d;
    }
}
