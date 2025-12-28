using UnityEngine;

[CreateAssetMenu(fileName = "Electric", menuName = "Tile/Electric")]
public class Electric : TileData
{
    public Sprite electricOn, electricOff;
    public override void OnEntityEnter(TileGameObject tileGameObject)
    {
        // 沒通電就什麼都不做
        if (!tileGameObject.IsPowered)
            return;

        // 1. 殺玩家
        Player.Instance.energy = -999;
    }

    public override void OnPowerChanged(TileGameObject tileGameObject, bool powered)
    {
        tileGameObject.IsPowered = powered;
        if (powered)
        {
            tileGameObject.GetComponent<SpriteRenderer>().sprite = electricOn;
        }
        else
        {
            tileGameObject.GetComponent<SpriteRenderer>().sprite = electricOff;
        }
    }
}
