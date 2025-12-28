using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    bool player_can_control = true;
    public int energy = 3;

    public Vector3 spawnPoint = new(0, 0, 0);
    public Vector3 playerNowDir = new(0, 0, 0);
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
            if (Input.GetKey(KeyCode.A))
            {
                transform.localScale = new Vector3(-1, 1, 1);
                playerNowDir = new(-1, 0, 0);
                RequestMove(Vector2Int.left);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.localScale = new Vector3(1, 1, 1);
                playerNowDir = new(1, 0, 0);
                RequestMove(Vector2Int.right);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                playerNowDir = new(1, 0, 0);
                RequestMove(Vector2Int.down);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                playerNowDir = new(-1, 0, 0);
                RequestMove(Vector2Int.up);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Vector3Int currentCell = TileManager.Instance.tilemap.WorldToCell(transform.position);
                Vector3Int targetCell  = currentCell + Vector3Int.FloorToInt(playerNowDir);

                TileGameObject currentTile = TileManager.Instance.GetTileObject(targetCell);
                currentTile.tileData.OnPlayerUse(currentTile);
            }
            else if (Input.GetKeyDown(KeyCode.Z) && energy > 0)
            {
                anim.SetTrigger("Suicide");
                //ParticleManager.Instance.SpawnTextScoreParticle(transform, value_s:"指令錯誤 ! 機體電量流失中 !", color:Color.red);
                energy--;
                if (energy <= 0) 
                {
                    anim.SetBool("isDead", true);
                    print("sui");
                    StartCoroutine(PlayerDiedIEnum(transform.position, true));
                };
            }
            else
            {
                anim.SetBool("isWalking",false);
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
        anim.SetBool("isWalking",false );
    }

    void OnCharge()
    {
        Debug.Log("充電");
    }

    IEnumerator PlayMoveAnimIEnum(MoveResult moveResult)
    {
        player_can_control = false;

        if (anim != null && anim.GetBool("isWalking") == false)
        {
            anim.SetBool("isWalking", true);
            print("iswalking");
        }

        Vector3 ori_pos = transform.position;
        Vector3 target_pos = moveResult.targetWorldPos;

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

        if (energy <= 0)
        {
            print("dead");
            anim.SetBool("isDead", true);
            yield return PlayerDiedIEnum(moveResult.targetWorldPos, false);
        }
        else
        {
            player_can_control = true;
        }
    }


    public IEnumerator PlayerDiedIEnum(Vector3 targetWorldPos, bool longerAnimation = false)
    {
        player_can_control = false;
        if (longerAnimation)
        {
            print("longer");
            yield return new WaitForSeconds(0.5f);
        }
        print("wait");
        print(anim.GetBool("isDead"));
        yield return new WaitForSeconds(1.1f);
        anim.SetBool("isDead", false);
        print("endwait");
        TileManager.Instance.SpawnCorpse(targetWorldPos);


        transform.position = spawnPoint;
        energy = 3;
        player_can_control = true;
    }
}
