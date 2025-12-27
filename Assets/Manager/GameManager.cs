using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float total_gameplay_ticks = 0;
    public static GameManager Instance;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Application.targetFrameRate = 30;
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        total_gameplay_ticks += Time.deltaTime;
    }

    public void SwitchScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
