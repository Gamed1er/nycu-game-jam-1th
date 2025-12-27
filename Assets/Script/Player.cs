using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    bool player_can_control = true;
    public int energy = 3;

    public Vector3 spawnPoint = new(0, 0, 0);
    Animator anim;
    void Awake()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player_can_control)
        {
            if (Input.GetKey(KeyCode.A)){
                transform.localScale = new Vector3(-1, 1, 1);
                RequestMove(Vector2Int.left);
            }
            else if (Input.GetKey(KeyCode.D)){
                transform.localScale = new Vector3(1, 1, 1);
                RequestMove(Vector2Int.right);
            }
            else if (Input.GetKey(KeyCode.S)) RequestMove(Vector2Int.down);
            else if (Input.GetKey(KeyCode.W)) RequestMove(Vector2Int.up);
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                anim.SetTrigger("Suicide");
                ParticleManager.Instance.SpawnTextScoreParticle(transform, value_s:"指令錯誤 ! 機體電量流失中 !", color:Color.red);
                energy--;
                if(energy <= 0) StartCoroutine(PlayerDiedIEnum(new MoveResult(false, transform.position, 0, SurfaceType.Normal)));
            }
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
        if (anim != null) {
            anim.SetTrigger("Walk"); // 這裡填入你在 Animator 參數面板設定的名字
        }

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

        // 4. Player Dead
        if(energy <= 0) yield return PlayerDiedIEnum(moveResult);
        else player_can_control = true;
    }

    IEnumerator PlayerDiedIEnum(MoveResult moveResult)
    {
        TileManager.Instance.SpawnCorpse(moveResult.targetWorldPos);
        yield return null;
        transform.position = spawnPoint;
        energy = 3;
        player_can_control = true;
    }
}
