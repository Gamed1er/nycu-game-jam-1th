using UnityEngine;

[CreateAssetMenu(fileName = "Quack", menuName = "Tile/Quack")]
public class Quack : TileData
{

    public override void OnPlayerUse(TileGameObject tileGameObject)
    {
        ParticleManager.Instance.SpawnTextScoreParticle(tileGameObject.transform, value_s:"嘎嘎 !", color:Color.yellow, textParticleType:TextParticleType.FloatUp);
    }
}