using UnityEngine;

[CreateAssetMenu(fileName = "Door", menuName = "Tile/Door")]
public class Door : TileData
{
    public Sprite DoorClose, DoorOpen;
    public SpriteRenderer spriteRenderer;
    public void ControlDoor(bool is_open)
    {
        if (is_open)
        {
            spriteRenderer.sprite = DoorOpen;
            ableToMove = true;
        }
        else
        {
            spriteRenderer.sprite = DoorClose;
            ableToMove = true;
        }
    }
}