using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Volume")]
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("Audio Library")]
    public List<AudioEntry> bgmList;
    public List<AudioEntry> sfxList;

    Dictionary<string, AudioClip> bgmDict;
    Dictionary<string, AudioClip> sfxDict;

    private readonly Dictionary<string, float> _nextTime = new();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource.loop = true;

        BuildDictionary();
        ApplyVolume();
    }

    /* ================= 建立索引 ================= */

    void BuildDictionary()
    {
        bgmDict = new Dictionary<string, AudioClip>();
        sfxDict = new Dictionary<string, AudioClip>();

        foreach (var e in bgmList)
        {
            if (!bgmDict.ContainsKey(e.key))
                bgmDict.Add(e.key, e.clip);
        }

        foreach (var e in sfxList)
        {
            if (!sfxDict.ContainsKey(e.key))
                sfxDict.Add(e.key, e.clip);
        }
    }

    /* ================= 播放（字串版） ================= */

    public void PlayBGM(string key, bool restart = false)
    {
        if (!bgmDict.ContainsKey(key))
        {
            Debug.LogWarning($"BGM not found: {key}");
            return;
        }

        AudioClip clip = bgmDict[key];

        if (bgmSource.clip == clip && bgmSource.isPlaying && !restart)
            return;

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(string key)
    {
        if (!sfxDict.ContainsKey(key))
        {
            Debug.LogWarning($"SFX not found: {key}");
            return;
        }

        sfxSource.PlayOneShot(sfxDict[key], sfxVolume);
    }

    public void PlaySFXCooldown(string key, float cooldownSec)
    {
        float now = Time.time;
        if (_nextTime.TryGetValue(key, out float t) && now < t) return;

        PlaySFX(key);
        _nextTime[key] = now + cooldownSec;
    }

    /* ================= 音量控制 ================= */

    public void SetBGMVolume(float value)
    {
        bgmVolume = Mathf.Clamp01(value);
        bgmSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        sfxSource.volume = sfxVolume;
    }

    void ApplyVolume()
    {
        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
    }
}

[System.Serializable]
public class AudioEntry
{
    public string key;
    public AudioClip clip;
}
