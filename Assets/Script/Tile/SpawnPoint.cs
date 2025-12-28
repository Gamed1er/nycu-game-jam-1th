using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPoint", menuName = "Tile/SpawnPoint")]
public class SpawnPoint : TileData
{
    public override void OnStart(TileGameObject tileGameObject)
    {
        tileGameObject.SpawnPoint = true;
        base.OnStart(tileGameObject);
    }

    public override void OnEntityEnter(TileGameObject tileGameObject)
    {
        AudioManager.Instance.PlaySFX("recharge",0.8f);
        Player.Instance.spawnPoint = tileGameObject.transform.position;
        ParticleManager.Instance.SpawnTextScoreParticle(Player.Instance.transform, value_s:"已設置重生點");
        if (tileGameObject.SpawnPoint)
        {
            Player.Instance.energy += 1;
            if(Player.Instance.energy >= 3) Player.Instance.energy = 3;
            tileGameObject.SpawnPoint = false;
        }
        //StageManager.Instance.SaveStageState(tileGameObject.transform.position);
        base.OnEntityEnter(tileGameObject);
    }

    public override void DeserializeState(TileGameObject tileGameObject, object state)
    {
        tileGameObject.SpawnPoint = true;
    }
}