using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Wire", menuName = "Tile/Wire")]
public class Wire : Tile 
{
    // Tile 類別內不需要存狀態，保持純淨
}

// 讓 Source 和 Receiver 也能透過 Tilemap 取得
public interface IPowerNode { }

public interface IPowerReceiver
{
    public void OnPowerChanged(bool powered);
}

public interface IPowerSource
{
    bool IsPowered { get; }
}
