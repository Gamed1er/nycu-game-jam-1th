using UnityEngine;
using UnityEngine.EventSystems;

public class Setting : MonoBehaviour, IPointerClickHandler
{
    public GameObject child;
    bool open = false;

    public void AdjustSFX(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }

    public void AdjustBGM(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX("button");
        if (open)
        {
            child.SetActive(false);
            open = false;
        }
        else
        {
            child.SetActive(true);
            open = true;
        }
    }
}