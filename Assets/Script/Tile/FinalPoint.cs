using UnityEngine;

[CreateAssetMenu(fileName = "FinalPoint", menuName = "Tile/FinalPoint")]
public class FinalPoint : TileData
{
    public override void OnEntityEnter(TileGameObject tileGameObject)
    {
        GameManager.Instance.GameEnd();
    }
}