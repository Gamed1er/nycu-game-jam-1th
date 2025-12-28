using System.Collections;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    public bool conductsPower = true;
    public bool conductsPowerIn = false;
    public Vector3Int Cell =>
        TileManager.Instance.tilemap.WorldToCell(transform.position);

    public Sprite ele;

    public bool TryPush(Vector2Int dir)
    {
        Vector3Int target = Cell + (Vector3Int)dir;

        // 目標格不能進 → 推不了
        TileGameObject currentTile = TileManager.Instance.GetTileObject(Vector3Int.FloorToInt(transform.position));
        TileGameObject targetTile = TileManager.Instance.GetTileObject(target);
        if (targetTile == null || !targetTile.ableToMove)
            return false;

        foreach (GameObject c in TileManager.Instance.Corpses)
        {
            if (c == null) continue;
            if (TileManager.Instance.tilemap.WorldToCell(c.transform.position) == target)
            {
                return false;
            }
        }

        currentTile.tileData.OnEntityExit(currentTile);
        targetTile.tileData.OnEntityEnter(targetTile);

        // 玩家能量 -1
        Player.Instance.energy -= 1;

        // 動畫
        StartCoroutine(PlayMoveAnimIEnum(target));

        PowerSystem.Instance.Recalculate();
        return true;
    }

    IEnumerator PlayMoveAnimIEnum(Vector3 target_pos)
    {
        Player.Instance.player_can_control = false;
        Vector3 ori_pos = transform.position;
        Animator anim = Player.Instance.GetComponent<Animator>();
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0));
        anim.SetBool("isPushing", true);
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0));
        float distance = Vector3.Distance(ori_pos, target_pos);
        float duration = 0.1f * distance;
        float elapsed = 0f;
        Debug.Log("wait");
        yield return new WaitForSeconds(0.2f);

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
        anim.SetBool("isPushing", false);
        transform.position = target_pos;
        StartCoroutine(Player.Instance.IsPlayerDieIEnum(Player.Instance.transform.position));
    }
}
