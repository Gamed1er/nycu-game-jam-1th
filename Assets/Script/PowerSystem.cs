using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerSystem : MonoBehaviour
{
    public static PowerSystem Instance;

    [SerializeField] private Tilemap tilemap;

    private void Awake()
    {
        Instance = this;
    }

    private static readonly Vector3Int[] dirs =
    {
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right
    };

    public void Recalculate()
    {
        HashSet<Vector3Int> poweredCells = new HashSet<Vector3Int>();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();

        // 1️⃣ 找所有電源
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            TileData tile = tilemap.GetTile<TileData>(pos);
            if (tile is IPowerSource source && source.IsPowered)
            {
                queue.Enqueue(pos);
                poweredCells.Add(pos);
            }
        }

        // 2️⃣ BFS 沿 wire 擴散
        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();

            foreach (var d in dirs)
            {
                Vector3Int next = current + d;
                if (poweredCells.Contains(next)) continue;

                Wire nextTile = tilemap.GetTile<Wire>(next);
                if (nextTile is Wire)
                {
                    poweredCells.Add(next);
                    queue.Enqueue(next);
                }
            }
        }

        // 3️⃣ 更新所有 Receiver
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Wire tile = tilemap.GetTile<Wire>(pos);

            if (tile is Wire wire)
                wire.powered = poweredCells.Contains(pos);

            if (tile is IPowerReceiver receiver)
                receiver.OnPowerChanged(poweredCells.Contains(pos));
        }
    }
}
