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

    public void SpawnTextScoreParticle(
    Transform location,
    int value = int.MinValue,
    string value_s = null,
    Color? color = null,
    TextParticleType textParticleType = TextParticleType.DropDown)
    {
        ParticleParent = FindFirstObjectByType<Canvas>().transform;

        Canvas canvas = ParticleParent.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.transform as RectTransform;

        // World → Screen
        Vector2 screenPos = Camera.main.WorldToScreenPoint(location.position);

        // Screen → Canvas(Local)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out Vector2 localPos
        );

        GameObject game = Instantiate(TextScoreParticle, ParticleParent);
        RectTransform rect = game.GetComponent<RectTransform>();
        rect.anchoredPosition = localPos;

        game.GetComponent<TextScoreParticle>()
            .Init(value, value_s, color, textParticleType);
    }

}
