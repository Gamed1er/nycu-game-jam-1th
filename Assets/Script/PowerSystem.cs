using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerSystem : MonoBehaviour
{
    public static PowerSystem Instance;
    public Tilemap wireMap; // 專門放電線的 Tilemap
    public Tilemap componentMap; // 放電源和接收器的 Tilemap

    // 紀錄目前哪些位置是有電的
    private HashSet<Vector3Int> poweredPositions = new HashSet<Vector3Int>();

    private void Awake() => Instance = this;

    private static readonly Vector3Int[] dirs = {
        Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
    };

    [ContextMenu("Recalculate")]
    public void Recalculate()
    {
        // 1. 初始化：清空舊電力狀態
        poweredPositions.Clear();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        HashSet<IPowerReceiver> activeReceivers = new HashSet<IPowerReceiver>();

        // 2. 第一步：尋找所有「電源 (IPowerSource)」作為起點
        // 掃描 componentMap 範圍內的所有 Tile
        foreach (var pos in componentMap.cellBounds.allPositionsWithin)
        {
            var tile = componentMap.GetTile(pos);
            if (tile is IPowerSource source && source.IsPowered)
            {
                // 將電源鄰近的電線加入傳導隊列
                foreach (var dir in dirs)
                {
                    Vector3Int neighbor = pos + dir;
                    if (wireMap.HasTile(neighbor)) queue.Enqueue(neighbor);
                }
            }
        }

        // 3. 第二步：擴散電力 (BFS 演算法)
        while (queue.Count > 0)
        {
            Vector3Int curr = queue.Dequeue();
            if (poweredPositions.Contains(curr)) continue;

            // 標記此電線位置有電
            poweredPositions.Add(curr);

            // 檢查四個方向
            foreach (var dir in dirs)
            {
                Vector3Int next = curr + dir;

                // 如果是電線，繼續傳播
                if (wireMap.HasTile(next) && !poweredPositions.Contains(next))
                {
                    queue.Enqueue(next);
                }
                
                // 如果是接收器，標記為「待觸發」
                var compTile = componentMap.GetTile(next);
                if (compTile is IPowerReceiver receiver)
                {
                    activeReceivers.Add(receiver);
                }
            }
        }

        // 4. 第三步：通知所有接收器
        // 這裡你可以視需求優化：通知「所有」接收器目前是有電還是沒電
        // 簡單做法：遍歷 componentMap 內所有接收器
        foreach (var pos in componentMap.cellBounds.allPositionsWithin)
        {
            if (componentMap.GetTile(pos) is IPowerReceiver r)
            {
                r.OnPowerChanged(activeReceivers.Contains(r));
            }
        }

        // (可選) 更新電線的視覺效果，例如切換顏色或動畫
        UpdateWireVisuals();
    }

    void UpdateWireVisuals()
    {
        // 遍歷所有電線，根據 poweredPositions 變更顏色
        foreach (var pos in wireMap.cellBounds.allPositionsWithin)
        {
            if (wireMap.HasTile(pos))
            {
                bool isOn = poweredPositions.Contains(pos);
                wireMap.SetColor(pos, isOn ? Color.yellow : Color.gray);
                // 注意：Tilemap 的 Tile 必須設定 Flags 為 None 才能改顏色
                wireMap.SetTileFlags(pos, TileFlags.None);
            }
        }
    }
}