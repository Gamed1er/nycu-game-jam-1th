using UnityEngine;

[CreateAssetMenu(fileName = "DoorButton", menuName = "Tile/DoorButton")]
public class DoorButton : TileData
{
    public Sprite doorClose, doorOpen;
    public override void OnEntityEnter(TileGameObject tileGameObject)
    {
        tileGameObject.IsPowered = true; 
        tileGameObject.GetComponent<SpriteRenderer>().sprite = doorOpen;
        PowerSystem.Instance.Recalculate();
    }

    public override void OnEntityExit(TileGameObject tileGameObject)
    {
        tileGameObject.IsPowered = false;
        tileGameObject.GetComponent<SpriteRenderer>().sprite = doorClose;
        PowerSystem.Instance.Recalculate();
    }

    public override void DeserializeState(TileGameObject tileGameObject, object state)
    {
        tileGameObject.IsPowered = false;
    }
}
