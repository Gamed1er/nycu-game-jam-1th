using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    public Image[] images;          // 7 張圖片（Inspector 依序放）
    public Button backButton;       // 回首頁按鈕

    public float fadeInTime = 1f;   // a
    public float showTime = 1f;     // b
    public float fadeOutTime = 1f;  // c

    void Start()
    {
        backButton.gameObject.SetActive(false);

        foreach (var img in images)
        {
            img.gameObject.SetActive(false);
            SetAlpha(img, 0);
        }

        StartCoroutine(PlayEndSequence());
    }

    IEnumerator PlayEndSequence()
    {
        for (int i = 0; i < images.Length; i++)
        {
            Image img = images[i];
            img.gameObject.SetActive(true);

            // 淡入
            yield return Fade(img, 0, 1, fadeInTime);

            // 顯示
            yield return new WaitForSeconds(showTime);

            // 最後一張不淡出
            if (i != images.Length - 1)
            {
                yield return Fade(img, 1, 0, fadeOutTime);
                img.gameObject.SetActive(false);
            }
        }

        // 顯示回首頁按鈕
        backButton.gameObject.SetActive(true);
    }

    IEnumerator Fade(Image img, float from, float to, float time)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / time);
            SetAlpha(img, a);
            yield return null;
        }
        SetAlpha(img, to);
    }

    void SetAlpha(Image img, float a)
    {
        Color c = img.color;
        c.a = a;
        img.color = c;
    }

    // 給 Button 用
    public void BackToTitle()
    {
        SceneManager.LoadScene("Title"); // 換成你的首頁場景名
    }
}
