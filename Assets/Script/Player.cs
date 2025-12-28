using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public bool player_can_control = true;
    public int energy = 3;

    public Vector3 spawnPoint = new(0, 0, 0);
    public Vector3 playerNowDir = new(0, 0, 0);
    Animator anim;
    public bool eletric = false;

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
                playerNowDir = new(0, 1, 0);
                RequestMove(Vector2Int.down);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                playerNowDir = new(0, -1, 0);
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
                StartCoroutine(IsPlayerDieIEnum(transform.position));
            }
            else
            {
                anim.SetBool("isWalking",false);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StageManager.Instance.ResetStage();
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

        AudioManager.Instance.PlaySFXCooldown("walk", 0.22f);
        PlayMoveAnim(result);

        // 周圍有充電裝就不會受傷害
        bool player_immune_damage = false;
        foreach(var c in TileManager.Instance.Corpses)
        {
            if(c == null) continue;
            if(!c.GetComponent<Corpse>().conductsPowerIn) continue;
            if(Mathf.Abs(c.transform.position.x - result.targetWorldPos.x) <= 1.5 && Mathf.Abs(c.transform.position.y - result.targetWorldPos.y) <= 1.5)
            {
                player_immune_damage = true;
                break;
            }
        }
        if(!player_immune_damage) energy -= result.total_cost;
        
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

    IEnumerator PlayMoveAnimIEnum(MoveResult moveResult)
    {
        player_can_control = false;

        if (anim != null && anim.GetBool("isWalking") == false)
        {
            anim.SetBool("isWalking", true);
            print("isWalking");
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
        yield return new WaitForSeconds(0.21f);
        anim.SetBool("isWalking", false);

        yield return IsPlayerDieIEnum(target_pos);
    }

    public IEnumerator IsPlayerDieIEnum(Vector3 targetWorldPos)
    {
        player_can_control = false;
        // 玩家死亡
        // -999 代表被電死
        if (eletric && energy <=0)
        {
            print("dead");
            anim.SetBool("die_ele", true);
            yield return PlayerEleDiedIEnum(targetWorldPos, false);
        }
        else if (energy <= 0)
        {
            print("dead noraml");
            anim.SetBool("isDead", true);
            yield return PlayerDiedIEnum(targetWorldPos, false);
        }
        else
        {
            player_can_control = true;
        }
    }


    public IEnumerator PlayerDiedIEnum(Vector3 targetWorldPos, bool longerAnimation = false)
    {
        Debug.Log("die");
        anim.SetBool("isDead", true);
        player_can_control = false;
        if (longerAnimation)
        {
            print("longer");
            yield return new WaitForSeconds(0.5f);
        }

        AudioManager.Instance.PlaySFX("die_normal");
        yield return new WaitForSeconds(1.1f);
        anim.SetBool("isDead", false);
        TileManager.Instance.SpawnCorpse(targetWorldPos, false);


        transform.position = spawnPoint;
        energy = 3;
        player_can_control = true;

        foreach (var pair in TileManager.Instance.tileObjects)
        {
            if(pair.Value.tileData is SpawnPoint)
            {
                pair.Value.SpawnPoint = true;
            }
        } 
    }

    public IEnumerator PlayerEleDiedIEnum(Vector3 targetWorldPos, bool longerAnimation = false)
    {
        Debug.Log(energy);
        player_can_control = false;
        if (longerAnimation)
        {
            print("longer");
            yield return new WaitForSeconds(0.5f);
        }
        AudioManager.Instance.PlaySFX("die_ele");
        yield return new WaitForSeconds(1.1f);
        anim.SetBool("die_ele", false);
        eletric = false;
        Debug.Log(anim.GetBool("die_ele"));
        TileManager.Instance.SpawnCorpse(targetWorldPos, true);


        transform.position = spawnPoint;
        energy = 3;
        player_can_control = true;

        foreach (var pair in TileManager.Instance.tileObjects)
        {
            if(pair.Value.tileData is SpawnPoint)
            {
                pair.Value.SpawnPoint = true;
            }
        } 
    }
}
