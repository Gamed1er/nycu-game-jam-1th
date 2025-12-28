using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    public Tilemap tilemap;

    [Header("Tile Objects")]
    public Dictionary<Vector3Int, TileGameObject> tileObjects = new();

    [Header("Corpses")]
    public List<GameObject> Corpses = new();
    public GameObject CorpsePrefab;
    public int max_corpse_count = 5;
    public Transform CorpseParent;

    private void Awake()
    {
        Instance = this;
    }

    #region TileGameObject 管理

    public void RegisterTileObject(Vector3Int cellPos, TileGameObject obj)
    {
        tileObjects[cellPos] = obj;
    }

    public TileGameObject GetTileObject(Vector3Int cellPos)
    {
        tileObjects.TryGetValue(cellPos, out var obj);
        return obj;
    }

    #endregion

    #region Movement

    public MoveResult TryMove(Vector3 worldPos, Vector2Int dir)
    {
        Vector3Int currentCell = tilemap.WorldToCell(worldPos);
        Vector3Int targetCell  = currentCell + new Vector3Int(dir.x, dir.y, 0);

        TileGameObject currentTile = GetTileObject(currentCell);
        TileGameObject targetTile  = GetTileObject(targetCell);

        // 沒有 TileGameObject = 視為不可走
        if (targetTile == null || !targetTile.ableToMove)
            return new MoveResult(false, worldPos, 0, SurfaceType.Normal);

        // 屍體阻擋
        foreach (GameObject c in Corpses)
        {
            if (c == null) continue;
            if (tilemap.WorldToCell(c.transform.position) == targetCell)
            {
                c.GetComponent<Corpse>().TryPush(dir);
                return new MoveResult(false, worldPos, 0, SurfaceType.Normal);
            }
        }

        // 觸發 Enter / Exit
        currentTile?.tileData.OnEntityExit(currentTile);
        targetTile.tileData.OnEntityEnter(targetTile);

        // Ice 滑行處理
        if (targetTile.surfaceType == SurfaceType.Ice)
        {
            MoveResult slide = TryMove(
                tilemap.GetCellCenterWorld(targetCell),
                dir
            );

            if (!slide.canMove)
            {
                return new MoveResult(
                    true,
                    tilemap.GetCellCenterWorld(targetCell),
                    targetTile.cost,
                    targetTile.surfaceType
                );
            }

            slide.total_cost += targetTile.cost;
            return slide;
        }

        // 正常移動
        return new MoveResult(
            true,
            tilemap.GetCellCenterWorld(targetCell),
            targetTile.cost,
            targetTile.surfaceType
        );
    }

    #endregion

    #region Corpse

    public void SpawnCorpse(Vector3 location, bool ele)
    {
        if (Corpses.Count >= max_corpse_count)
        {
            KillCorpse(Corpses[0]);
            //Corpses.RemoveAt(0);
        }

        GameObject corpse = Instantiate(CorpsePrefab, location, Quaternion.identity, CorpseParent);
        Corpses.Add(corpse);
        if (ele)
        {
            corpse.GetComponent<Corpse>().conductsPower = false;
            corpse.GetComponent<Corpse>().conductsPowerIn = true;
            corpse.GetComponent<SpriteRenderer>().sprite = corpse.GetComponent<Corpse>().ele;
        }

        Vector3Int cell = tilemap.WorldToCell(location);
        GetTileObject(cell)?.tileData.OnEntityEnter(GetTileObject(cell));
    }

    public void KillCorpse(GameObject target)
    {
        if (target == null) return;

        Vector3Int cell = tilemap.WorldToCell(target.transform.position);
        GetTileObject(cell)?.tileData.OnEntityExit(GetTileObject(cell));

        Corpses.Remove(target);
        Destroy(target);
    }

    #endregion
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
