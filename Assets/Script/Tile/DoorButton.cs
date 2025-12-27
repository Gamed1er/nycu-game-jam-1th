using UnityEngine;

[CreateAssetMenu(fileName = "DoorButton", menuName = "Tile/DoorButton")]
public class DoorButton : TileData
{
    public Vector2Int doorPos;

    public override void OnEntityEnter()
    {
        TileManager.Instance.tilemap.GetTile<Door>((Vector3Int)doorPos);
        base.OnEntityEnter();
    }
}