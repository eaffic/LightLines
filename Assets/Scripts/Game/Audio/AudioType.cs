using UnityEngine;
using UnityEngine.Audio;
using GameEnumList;

[System.Serializable] 
public class AudioType {
    [HideInInspector]
    public AudioSource Source;
    public AudioClip[] Clips;
    public AudioMixerGroup Group;

    public string Name;

    [Range(0f, 1f)]
    public float MaxVolume;
    [Range(0.1f, 5f)]
    public float Pitch;
    public bool Loop;
}