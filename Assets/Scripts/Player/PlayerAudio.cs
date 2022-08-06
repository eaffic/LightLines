using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip _walkClip;
    public AudioClip _runClip;
    public AudioClip _pushClip;
    public AudioClip _jumpClip;
    public AudioClip _fallClip;

    private AudioSource _source;

    private void Awake()
    {
        _source = gameObject.AddComponent<AudioSource>();
    }

    public void SetPlayerWalkAudio()
    {
        _source.clip = _walkClip;
        _source.loop = true;
        _source.Play();
    }

    public void SetPlayerRunAudio()
    {
        _source.clip = _runClip;
        _source.loop = true;
        _source.Play();
    }

    public void SetPlayerPushAudio()
    {
        _source.clip = _pushClip;
        _source.loop = false;
        _source.Play();
    }

    public void SetPlayerJumpAudio()
    {
        _source.clip = _jumpClip;
        _source.loop = false;
        _source.Play();
    }

    public void StopAudio(){
        _source.Stop();
    }
}