using UnityEngine;

[CreateAssetMenu(fileName = "Door", menuName = "Tile/Door")]
public class Door : TileData, IPowerReceiver
{
    public Sprite doorClose, doorOpen;

    public void OnPowerChanged(bool powered)
    {
        if (powered)
        {
            sprite = doorOpen;
            ableToMove = true;
        }
        else
        {
            sprite = doorClose;
            ableToMove = false;
        }
    }
}