using UnityEngine;

[CreateAssetMenu(fileName = "Door", menuName = "Tile/Door")]
public class Door : TileData
{
    public Sprite doorClose, doorOpen;

    public override void OnPowerChanged(TileGameObject tileGameObject, bool powered)
    {
        tileGameObject.IsPowered = powered;
        if (powered)
        {
            tileGameObject.GetComponent<SpriteRenderer>().sprite = doorOpen;
            tileGameObject.ableToMove = true;
        }
        else
        {
            tileGameObject.GetComponent<SpriteRenderer>().sprite = doorClose;
            tileGameObject.ableToMove = false;

            foreach (GameObject c in TileManager.Instance.Corpses.ToArray())
            {
                if (c == null) continue;
                if (TileManager.Instance.tilemap.WorldToCell(c.transform.position) == tileGameObject.transform.position)
                {
                    TileManager.Instance.KillCorpse(c);
                }
            }
        }
    }
}