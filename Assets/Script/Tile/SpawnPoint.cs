using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPoint", menuName = "Tile/SpawnPoint")]
public class SpawnPoint : TileData
{
    public override void OnEntityEnter(TileGameObject tileGameObject)
    {
        Player.Instance.spawnPoint = transform.GetPosition();
        ParticleManager.Instance.SpawnTextScoreParticle(Player.Instance.transform, value_s:"已設置重生點");
        base.OnEntityEnter(tileGameObject);
    }
}