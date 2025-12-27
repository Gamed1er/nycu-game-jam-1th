using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Wire", menuName = "Tile/Wire")]
public class Wire : Tile
{
    public bool powered;
}

public interface IPowerNode
{
    void SetPower(bool powered);
}

public interface IPowerReceiver
{
    void OnPowerChanged(bool powered);
}

public interface IPowerSource
{
    bool IsPowered { get; }
}
