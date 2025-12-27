using UnityEngine;

[CreateAssetMenu(fileName = "DoorButton", menuName = "Tile/DoorButton")]
public class DoorButton : TileData
{
    public bool IsPowered { get; private set; }

    public override void OnEntityEnter(TileGameObject tileGameObject)
    {
        Debug.Log("ButtonPressed");
        tileGameObject.IsPowered = true; 
        PowerSystem.Instance.Recalculate();
    }

    public override void OnEntityExit(TileGameObject tileGameObject)
    {
        tileGameObject.IsPowered = false;
        PowerSystem.Instance.Recalculate();
    }
}
