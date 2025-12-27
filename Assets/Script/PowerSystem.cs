using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerSystem : MonoBehaviour
{
    public static PowerSystem Instance;

    [Header("Tilemaps")]
    public Tilemap tileMap;   // 放 Power_In / Power_Out
    public Tilemap wireMap;   // 放 Power_Node

    // 哪些電線格目前是有電的（純系統狀態）
    private HashSet<Vector3Int> poweredWires = new HashSet<Vector3Int>();

    private static readonly Vector3Int[] dirs =
    {
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right
    };

    void Awake()
    {
        Instance = this;
    }

    // ======= 對外唯一入口 =======
    public void Recalculate()
    {
        poweredWires.Clear();

        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        HashSet<Vector3Int> poweredOutputs = new HashSet<Vector3Int>();

        // ---------- Step 1：找所有 Power_In ----------
        foreach (var pair in TileManager.Instance.tileObjects)
        {
            Vector3Int pos = pair.Key;
            TileGameObject go = pair.Value;

            if (go.surfaceType != SurfaceType.Power_In) continue;
            if (!go.IsPowered) continue;

            // 將相鄰電線加入 BFS 起點
            foreach (var dir in dirs)
            {
                Vector3Int next = pos + dir;
                if (IsWire(next))
                    queue.Enqueue(next);
            }
        }

        // ---------- Step 2：BFS 傳電 ----------
        while (queue.Count > 0)
        {
            Vector3Int curr = queue.Dequeue();
            if (poweredWires.Contains(curr)) continue;

            poweredWires.Add(curr);

            foreach (var dir in dirs)
            {
                Vector3Int next = curr + dir;

                // 繼續沿著電線傳
                if (IsWire(next) && !poweredWires.Contains(next))
                {
                    queue.Enqueue(next);
                }

                // 碰到輸出（門）
                if (TryGetPowerOut(next, out TileGameObject outGO))
                {
                    poweredOutputs.Add(next);
                }
            }
        }

        // ---------- Step 3：通知所有 Power_Out ----------
        foreach (var pair in TileManager.Instance.tileObjects)
        {
            Vector3Int pos = pair.Key;
            TileGameObject go = pair.Value;

            if (go.surfaceType != SurfaceType.Power_Out) continue;

            bool powered = poweredOutputs.Contains(pos);
            (go.tileData as Door)?.OnPowerChanged(go, powered);
        }

        // （可選）更新電線顯示
        UpdateWireVisual();
    }

    // ======= 小工具 =======

    bool IsWire(Vector3Int pos)
    {
        if (!wireMap.HasTile(pos)) return false;
        TileData t = wireMap.GetTile<TileData>(pos);
        return t != null && t.surfaceType == SurfaceType.Power_Node;
    }

    bool TryGetPowerOut(Vector3Int pos, out TileGameObject go)
    {
        go = TileManager.Instance.GetTileObject(pos);
        return go != null && go.surfaceType == SurfaceType.Power_Out;
    }

    void UpdateWireVisual()
    {
        foreach (var pos in wireMap.cellBounds.allPositionsWithin)
        {
            if (!IsWire(pos)) continue;

            wireMap.SetTileFlags(pos, TileFlags.None);
            wireMap.SetColor(
                pos,
                poweredWires.Contains(pos) ? Color.yellow : Color.gray
            );
        }
    }
}
