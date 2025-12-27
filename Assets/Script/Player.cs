using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    bool player_can_control = true;
    public int energy = 3;

    public Vector3 spawnPoint = new(0, 0, 0);

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (player_can_control)
        {
            if (Input.GetKeyDown(KeyCode.A)) RequestMove(Vector2Int.left);
            else if (Input.GetKeyDown(KeyCode.D)) RequestMove(Vector2Int.right);
            else if (Input.GetKeyDown(KeyCode.S)) RequestMove(Vector2Int.down);
            else if (Input.GetKeyDown(KeyCode.W)) RequestMove(Vector2Int.up);
        }
    }

    void RequestMove(Vector2Int dir)
    {
        MoveResult result = TileManager.Instance.TryMove(transform.position, dir);

        if (!result.canMove)
        {
            PlayBlockedAnim();
            return;
        }

        PlayMoveAnim(result);

        energy -= result.total_cost;
    }

    void PlayMoveAnim(MoveResult moveResult)
    {
        StartCoroutine(PlayMoveAnimIEnum(moveResult));
    }

    void PlayBlockedAnim()
    {
        Debug.Log("撞牆動畫");
    }

    void OnCharge()
    {
        Debug.Log("充電");
    }

    IEnumerator PlayMoveAnimIEnum(MoveResult moveResult) 
    {
        // 1. Prevent input at the start
        player_can_control = false;

        Vector3 ori_pos = transform.position;
        Vector3 target_pos = moveResult.targetWorldPos; // Example property
        float elapsed = 0;
        float duration = 0.1f * Vector3.Distance(ori_pos, target_pos); // Seconds the move takes

        // 2. The Animation Loop (Linear Interpolation)
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(ori_pos, target_pos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // 3. Ensure the final position is exact
        transform.position = target_pos;

        // 4. Re-enable control only after the animation is finished
        player_can_control = true;
    }

}
