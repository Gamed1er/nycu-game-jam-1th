using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    public GameObject TextScoreParticle;

    [Header("這個留空，他會自己抓")]
    public Transform ParticleParent;

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

    public void SpawnTextScoreParticle(Transform location ,int value = int.MinValue, string value_s = null, Color? color = null, TextParticleType textParticleType = TextParticleType.DropDown)
    {
        ParticleParent ??= FindFirstObjectByType<Canvas>().transform;
        GameObject game = Instantiate(TextScoreParticle, location.position, location.rotation, ParticleParent);
        game.GetComponent<TextScoreParticle>().Init(value, value_s, color, textParticleType);
    }
}
