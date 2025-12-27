using UnityEngine;

[CreateAssetMenu(fileName = "Lever", menuName = "Tile/Lever")]
public class Lever : TileData
{

    public override void OnPlayerUse(TileGameObject tileGameObject)
    {
        tileGameObject.IsPowered = !tileGameObject.IsPowered;
        PowerSystem.Instance.Recalculate();
        Player.Instance.energy -= 1;
        if(Player.Instance.energy <= 0) Player.Instance.StartCoroutine(Player.Instance.PlayerDiedIEnum(Player.Instance.transform.position));
    }
}