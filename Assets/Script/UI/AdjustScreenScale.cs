using System.Collections.Generic;
using UnityEngine;

public class AdjustScreenScale : MonoBehaviour
{
    public static AdjustScreenScale Instance;
    public List<Transform> ScaleChange;
    public List<Transform> ScaleChangeSizeBig;
    public float scaleFactor = 0, scaleFactorBig;
    private const float baseWidth = 1920, baseHeight = 1080;

    void Awake()
    {
        Instance = this;
        scaleFactor = GetCurrentScaleMin();
    }

    void Update()
    {
        AdjustUIScale();
    }

    void AdjustUIScale()
    {
        scaleFactor = GetCurrentScaleMin();
        foreach (Transform t in ScaleChange)
        {
            if (t != null)
                t.localScale = Vector3.one * scaleFactor;
        }

        scaleFactorBig = GetCurrentScaleMax();
        foreach (Transform t in ScaleChangeSizeBig)
        {
            if (t != null)
                t.localScale = Vector3.one * scaleFactorBig;
        }
    }

    public float GetCurrentScaleMin()
    {
        float nowX = Screen.width;
        float nowY = Screen.height;
        return Mathf.Min(nowX / baseWidth, nowY / baseHeight);
    }

    public float GetCurrentScaleMax()
    {
        float nowX = Screen.width;
        float nowY = Screen.height;
        return Mathf.Max(nowX / baseWidth, nowY / baseHeight);
    }
}