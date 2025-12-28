using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;
    StageSnapshot snapshot;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // 等待 1tick 之後重設所有電線
        StartCoroutine(StartLevelIEnum());
    }

    IEnumerator StartLevelIEnum()
    {
        yield return null;
        foreach (var pair in TileManager.Instance.tileObjects)
        {
            pair.Value.tileData.OnStart(pair.Value);
        } 
        PowerSystem.Instance.Recalculate();
        SaveStageState(Vector3.zero);
    }

    public void SaveStageState(Vector3 platerResetPos)
    {
        snapshot = new StageSnapshot();
        snapshot.playerResetPos = platerResetPos; // (0,0)

        foreach (var pair in TileManager.Instance.tileObjects)
        {
            TileGameObject tileGO = pair.Value;

            TileSnapshot ts = new TileSnapshot
            {
                position = pair.Key,
                tileData = tileGO.tileData,
                state = tileGO.tileData.SerializeState(tileGO)
            };

            snapshot.tiles.Add(ts);
        }
    }

    public void ResetStage()
    {
        // 1. 還原 Tile
        foreach (var ts in snapshot.tiles)
        {
            TileGameObject tileGO = TileManager.Instance.GetTileObject(ts.position);
            if (tileGO == null) continue;

            tileGO.tileData = ts.tileData;
            tileGO.tileData.DeserializeState(tileGO, ts.state);
        }

        // 2. 清空屍體
        foreach (var corpse in TileManager.Instance.Corpses)
        {
            if (corpse != null)
                Destroy(corpse);
        }
        TileManager.Instance.Corpses.Clear();

        // 3. 玩家重置
        Player.Instance.transform.position = snapshot.playerResetPos;
        Player.Instance.energy = 3;

        // 4. 重算電力
        PowerSystem.Instance.Recalculate();
    }


}

[System.Serializable]
public class TileSnapshot
{
    public Vector3Int position;
    public TileData tileData;
    public object state; // TileData 自己定義
}

[System.Serializable]
public class StageSnapshot
{
    public List<TileSnapshot> tiles = new();
    public Vector3 playerResetPos;
}
