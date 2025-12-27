using UnityEngine;

[CreateAssetMenu(fileName = "Door", menuName = "Tile/Door")]
public class Door : TileData
{
    public Sprite doorClose, doorOpen;

    public override void OnPowerChanged(TileGameObject tileGameObject, bool powered)
    {
        if (powered)
        {
            tileGameObject.GetComponent<SpriteRenderer>().sprite = doorOpen;
            tileGameObject.ableToMove = true;
        }
        else
        {
            tileGameObject.GetComponent<SpriteRenderer>().sprite = doorClose;
            tileGameObject.ableToMove = false;
        }
    }
}