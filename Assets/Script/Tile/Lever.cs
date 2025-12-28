using UnityEngine;

[CreateAssetMenu(fileName = "Lever", menuName = "Tile/Lever")]
public class Lever : TileData
{
    public Sprite leverOn, leverOff;
    public override void OnPlayerUse(TileGameObject tileGameObject)
    {
        tileGameObject.IsPowered = !tileGameObject.IsPowered;
        PowerSystem.Instance.Recalculate();
        Player.Instance.energy -= 1;
        if (Player.Instance.energy <= 0)
        {
            Player.Instance.StartCoroutine(Player.Instance.PlayerDiedIEnum(Player.Instance.transform.position));
        }

        if (tileGameObject.IsPowered)
        {
            tileGameObject.GetComponent<SpriteRenderer>().sprite = leverOn;
        }
        else
        {
            tileGameObject.GetComponent<SpriteRenderer>().sprite = leverOff;
        }
    }

    public override object SerializeState(TileGameObject tileGameObject)
    {
        return tileGameObject.IsPowered;
    }

    public override void DeserializeState(TileGameObject tileGameObject, object state)
    {
        tileGameObject.IsPowered = (bool)state;
    }
}