using UnityEngine;
using UnityEngine.Audio;
using GameEnumList;

/// <summary>
/// 音源単位クラス
/// </summary>
[System.Serializable] 
public class AudioType {
    [HideInInspector]
    public AudioSource Source;
    public AudioClip[] Clips;
    public AudioMixerGroup Group;

    public string Name; //音源名

    [Range(0f, 1f)]
    public float MaxVolume; //最大音量
    [Range(0.1f, 5f)]
    public float Pitch; //音高
    public bool Loop; //ループ
}