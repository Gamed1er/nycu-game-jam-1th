using UnityEngine;

[CreateAssetMenu(fileName = "DoorButton", menuName = "Tile/DoorButton")]
public class DoorButton : TileData, IPowerSource
{
    public bool IsPowered { get; private set; }

    public override void OnEntityEnter()
    {
        IsPowered = true;
        PowerSystem.Instance.Recalculate();
    }

    public override void OnEntityExit()
    {
        IsPowered = false;
        PowerSystem.Instance.Recalculate();
    }
}
