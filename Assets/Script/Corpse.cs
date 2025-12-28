using System.Collections;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    public bool conductsPower = true;
    public bool conductsPowerIn = false;
    public Vector3Int Cell =>
        TileManager.Instance.tilemap.WorldToCell(transform.position);

    public Sprite ele;

    public void TryPush(Vector2Int dir)
    {
        Vector3Int target = Cell + (Vector3Int)dir;

        // 目標格不能進 → 推不了
        TileGameObject currentTile = TileManager.Instance.GetTileObject(Vector3Int.FloorToInt(transform.position));
        TileGameObject targetTile = TileManager.Instance.GetTileObject(target);
        if (targetTile == null || !targetTile.ableToMove)
            return;

        foreach (GameObject c in TileManager.Instance.Corpses)
        {
            if (c == null) continue;
            if (TileManager.Instance.tilemap.WorldToCell(c.transform.position) == target)
            {
                return;
            }
        }

        currentTile.tileData.OnEntityExit(currentTile);
        targetTile.tileData.OnEntityEnter(targetTile);

        // 動畫
        StartCoroutine(PlayMoveAnimIEnum(target));

        PowerSystem.Instance.Recalculate();
        return;
    }

    IEnumerator PlayMoveAnimIEnum(Vector3 target_pos)
    {
        Vector3 ori_pos = transform.position;

        float distance = Vector3.Distance(ori_pos, target_pos);
        float duration = 0.1f * distance;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(
                ori_pos,
                target_pos,
                elapsed / duration
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target_pos;
    }
}
