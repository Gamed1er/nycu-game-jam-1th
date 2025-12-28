using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // 等待 1tick 之後重設所有電線
        StartCoroutine(StartLevelIEnum());
    }

    IEnumerator StartLevelIEnum()
    {
        yield return null;
        foreach (var pair in TileManager.Instance.tileObjects)
        {
            pair.Value.tileData.OnStart(pair.Value);
        } 
        PowerSystem.Instance.Recalculate();
    }
}