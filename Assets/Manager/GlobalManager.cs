using System.Collections.Generic;
using UnityEngine;


public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
