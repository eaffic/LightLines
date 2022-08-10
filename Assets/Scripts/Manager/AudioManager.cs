using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 音声管理
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    public float VolumeDuration = 1.5f;
    public AudioType[] AudioTypes;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        foreach (var item in AudioTypes)
        {
            item.Source = gameObject.AddComponent<AudioSource>();

            item.Source.clip = item.Clips[0];
            item.Source.volume = 0f;
            item.Source.pitch = item.Pitch;
            item.Source.loop = item.Loop;
            item.Source.Stop();

            if (item.Group != null)
            {
                item.Source.outputAudioMixerGroup = item.Group;
            }
        }
    }

    private void Start()
    {
        StartNewScene();
    }

    public void Play(string sourceName, bool loop)
    {
        foreach (var item in AudioTypes)
        {
            if (item.Name == sourceName)
            {
                item.Source.loop = loop;
                item.Source.Play();
                return;
            }
        }

        Debug.LogWarning("not found source");
    }

    /// <summary>
    /// 指定clipをplay
    /// </summary>
    /// <param name="sourceName">音源名</param>
    /// <param name="clipName">clip名</param>
    /// <param name="loop">ループ</param>
    public void Play(string sourceName, string clipName, bool loop)
    {
        AudioType type = null;
        AudioClip clip = null;
        foreach (var item in AudioTypes)
        {
            if (item.Name == sourceName)
            {
                type = item;
                break;
            }
        }

        if (type == null)
        {
            Debug.LogWarning("not found source");
            return;
        }

        foreach (var item in type.Clips)
        {
            if (item.name == clipName)
            {
                clip = item;
            }
        }

        if (clip == null)
        {
            Debug.LogWarning("not found clip");
            return;
        }

        type.Source.loop = loop;
        type.Source.clip = clip;
        type.Source.Play();
    }

    /// <summary>
    /// 指定source停止
    /// </summary>
    /// <param name="sourceName"></param>
    public void Pause(string sourceName)
    {
        foreach (var item in AudioTypes)
        {
            if (item.Name == sourceName)
            {
                item.Source.Pause();
                return;
            }
        }

        Debug.LogWarning("not found source");
    }

    /// <summary>
    /// 指定source終了
    /// </summary>
    /// <param name="sourceName"></param>
    public void Stop(string sourceName)
    {
        foreach (var item in AudioTypes)
        {
            if (item.Name == sourceName)
            {
                item.Source.Stop();
                return;
            }
        }

        Debug.LogWarning("not found source");
    }

    /// <summary>
    /// すべてのsourceを終了する
    /// </summary>
    public void StopAllSource()
    {
        foreach (var item in AudioTypes)
        {
            item.Source.Stop();
        }
    }

    /// <summary>
    /// 音量調整
    /// </summary>
    /// <param name="sourceName"></param>
    /// <param name="volume"></param>
    public void SetVolume(string sourceName, float volume)
    {
        foreach (var item in AudioTypes)
        {
            if (item.Name == sourceName)
            {
                //最大音量チェック
                if (item.MaxVolume < volume)
                {
                    item.Source.volume = volume;
                }
                else
                {
                    item.Source.volume = volume;
                }
                return;
            }
        }

        Debug.LogWarning("not found source");
    }

    public void SetAllVolume(float volume)
    {
        foreach (var item in AudioTypes)
        {
            //最大音量チェック
            if (item.Source.volume > item.MaxVolume)
            {
                item.Source.volume = item.MaxVolume;
                continue;
            }
            item.Source.volume = volume;
        }
    }

    public void SetPitch(string sourceName, float pitch)
    {
        foreach (var item in AudioTypes)
        {
            if (item.Name == sourceName)
            {
                item.Source.pitch = pitch;
                return;
            }
        }

        Debug.LogWarning("not found source");
    }

    /// <summary>
    /// 滑らかな音量変化
    /// </summary>
    /// <param name="sourceName">音源名</param>
    /// <param name="volume">指定音量</param>
    /// <param name="duration">時間</param>
    public void ChangeSourceVolume(string sourceName, float volume, float duration){
        foreach (var item in AudioTypes)
        {
            if (item.Name == sourceName)
            {
                StartCoroutine(ChangeSourceVolume(item, volume, duration));
                return;
            }
        }

        Debug.LogWarning("not found source");
    }

    public void StartNewScene()
    {
        StartCoroutine(NewSceneSetting());
    }

    IEnumerator ChangeSourceVolume(AudioType type, float volume, float duration){
        float delta = Time.deltaTime / duration;
        if(type.Source.volume > volume){
            //Decrease
            while(type.Source.volume >= volume){
                type.Source.volume = Mathf.Max(type.Source.volume - delta, volume);
                yield return null;
            }
        }   
        else{
            //Increase
            while(type.Source.volume <= volume){
                type.Source.volume = Mathf.Min(type.Source.volume + delta, volume);
                yield return null;
            }
        }
        type.Source.volume = volume;
        yield return null;
    }

    /// <summary>
    /// 新しーンをロードする時
    /// </summary>
    /// <returns></returns>
    IEnumerator NewSceneSetting()
    {
        float volume = 0f;
        float delta = Time.deltaTime / VolumeDuration;
        while (volume < 0.95f)
        {
            volume = Mathf.Min(volume + delta, 1f);
            SetAllVolume(volume);
            yield return null;
        }
        SetAllVolume(1f);
        yield return null;
    }
}