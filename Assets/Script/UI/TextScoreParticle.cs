using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextScoreParticle : MonoBehaviour
{
    public Text text;
    float exist_time = 1, now_time = 0;
    public TextParticleType textParticleType;

    void Start()
    {
        transform.localScale = Vector3.one * AdjustScreenScale.Instance.scaleFactor;
        Init(value_s:"666");
    }
    public void Init(int value = int.MinValue, string value_s = null, Color? color = null, TextParticleType textParticleType = TextParticleType.DropDown)
    {
        Color finalColor = color ?? Color.black;

        if (value != int.MinValue) text.text = value.ToString();
        else if (value_s != null) text.text = value_s;
        //else Destroy(this.gameObject);

        text.color = finalColor;

        if(textParticleType == TextParticleType.DropDown) StartCoroutine(DropDown());
        if(textParticleType == TextParticleType.FloatUp) StartCoroutine(FloatUp());
        if(textParticleType == TextParticleType.Idle) StartCoroutine(Idle());
    }

    IEnumerator DropDown()
    {
        RectTransform rect = GetComponent<RectTransform>();
        now_time = 0;

        // 初速度（依 UI 單位調整）
        Vector2 velocity = new Vector2(
            Random.Range(-80f, 80f), // 左右隨機
            Random.Range(180f, 260f) // 初始向上速度
        );

        float gravity = -1000f;   // UI 重力
        Color baseColor = text.color;

        while (now_time < exist_time)
        {
            float dt = Time.deltaTime;
            now_time += dt;

            // 套用重力
            velocity.y += gravity * dt;

            // 更新位置
            rect.anchoredPosition += velocity * dt;

            // 淡出
            float alpha = 1 - (now_time / exist_time);
            text.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

            yield return null;
        }

        Destroy(gameObject);
    }

    IEnumerator FloatUp()
    {
        RectTransform rect = GetComponent<RectTransform>();
        now_time = 0;

        // 初速度（依 UI 單位調整）
        Vector2 velocity = new Vector2(
            0, 100
        );

        Color baseColor = text.color;

        while (now_time < exist_time)
        {
            float dt = Time.deltaTime;
            now_time += dt;

            // 更新位置
            rect.anchoredPosition += velocity * dt;

            // 淡出
            float alpha = 1 - (now_time / exist_time);
            text.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

            yield return null;
        }

        Destroy(gameObject);
    }

    IEnumerator Idle()
    {
        RectTransform rect = GetComponent<RectTransform>();
        now_time = 0;
        Color baseColor = text.color;

        while (now_time < exist_time)
        {
            // 淡出
            float alpha = 1 - (now_time / exist_time);
            text.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

            yield return null;
        }

        Destroy(gameObject);
    }
}

public enum TextParticleType
{
    DropDown, FloatUp, Idle
}