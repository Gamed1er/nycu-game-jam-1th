using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    public Sprite[] energySprites; // size = 3
    public Image targetImage;

    void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();
    }

    void Update()
    {
        if (Player.Instance == null) return;

        UpdateEnergy(Player.Instance.energy);
    }

    void UpdateEnergy(int energy)
    {
        if (energySprites == null || energySprites.Length < 3) return;

        // energy: 3,2,1 ¡÷ index: 2,1,0
        int index = Mathf.Clamp(energy, 0, energySprites.Length);

        targetImage.sprite = energySprites[index];
    }
}
