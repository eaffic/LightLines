using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 音声管理
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] float _volumeChangeSpeed = 0.02f;

    [Header("BGM")]
    public AudioClip _TitleBGMClip;
    public AudioClip _SelectBGMClip;
    public AudioClip _StageBGMClip;
    public AudioClip _ResultBGMClip;

    [Header("プレイヤー効果音")]
    public AudioClip _WalkStepClips;
    public AudioClip _RunStepClips;
    public AudioClip _JumpClip;
    public AudioClip _PunchClip;

    [Header("環境音/エフェクト音")]
    public AudioClip _LaserClip;
    public AudioClip _MagicCircleClip;

    [Header("効果音/動作音")]
    public AudioClip _OpenTargetClip;
    public AudioClip _BoxMoveClip;
    public AudioClip _BoxDownClip;
    public AudioClip _SelectClickClip;
    public AudioClip _SubmitClickClip;
    public AudioClip _GetItemClip;

    //各音声用のオーディオソース
    AudioSource _musicSource;
    AudioSource _playerSource;
    AudioSource _ambientSource;
    AudioSource _fxSource;

    public static AudioManager _Instance;

    void Awake()
    {
        if (_Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _Instance = this;
        DontDestroyOnLoad(this.gameObject);

        //オーディオソースコンポーネント追加
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.playOnAwake = false;
        _playerSource = gameObject.AddComponent<AudioSource>();
        _ambientSource = gameObject.AddComponent<AudioSource>();
        _fxSource = gameObject.AddComponent<AudioSource>();
    }

    #region BGM関係
    public static void PlayTitleBGMAudio()
    {
        StopMusicAudio();
        _Instance._musicSource.clip = _Instance._TitleBGMClip;
        _Instance._musicSource.loop = true;
        _Instance._musicSource.volume = 0.8f;
        _Instance._musicSource.Play();
    }
    public static void PlaySelectBGMAudio()
    {
        StopMusicAudio();
        _Instance._musicSource.clip = _Instance._SelectBGMClip;
        _Instance._musicSource.loop = true;
        _Instance._musicSource.volume = 0.8f;
        _Instance._musicSource.Play();
    }
    public static void PlayStageBGMAudio()
    {
        StopMusicAudio();
        _Instance._musicSource.clip = _Instance._StageBGMClip;
        _Instance._musicSource.loop = true;
        _Instance._musicSource.volume = 0.8f;
        _Instance._musicSource.Play();
    }
    public static void PlayResultBGMAudio()
    {
        StopMusicAudio();
        _Instance._musicSource.clip = _Instance._ResultBGMClip;
        _Instance._musicSource.volume = 0.8f;
        _Instance._musicSource.Play();
    }
    #endregion

    #region プレイヤー関係
    public static void PlayWalkStepAudio()
    {
        StopPlayerAudio();
        _Instance._playerSource.clip = _Instance._WalkStepClips;
        _Instance._playerSource.loop = true;
        _Instance._playerSource.Play();
    }
    public static void PlayRunStepAudio()
    {
        StopPlayerAudio();
        _Instance._playerSource.clip = _Instance._RunStepClips;
        _Instance._playerSource.loop = true;
        _Instance._playerSource.Play();
    }
    public static void PlayJumpAudio()
    {
        StopPlayerAudio();
        _Instance._playerSource.PlayOneShot(_Instance._JumpClip);
    }
    public static void PlayPunchAudio()
    {
        StopPlayerAudio();
        _Instance._playerSource.PlayOneShot(_Instance._PunchClip);
    }
    #endregion

    #region 環境音、エフェクト
    public static void PlayMagicCircleAudio()
    {
        StopAmbientAudio();
        _Instance._ambientSource.PlayOneShot(_Instance._MagicCircleClip);
    }
    public static void PlayLaserAudio()
    {
        StopAmbientAudio();
        _Instance._ambientSource.PlayOneShot(_Instance._LaserClip);
    }
    #endregion

    #region SE関係
    public static void PlayBoxMoveAudio()
    {
        StopFXAudio();
        _Instance._fxSource.PlayOneShot(_Instance._BoxMoveClip);
    }
    public static void PlayBoxDownAudio()
    {
        StopFXAudio();
        _Instance._fxSource.PlayOneShot(_Instance._BoxDownClip);
    }
    public static void PlayOpenTargetAudio()
    {
        StopFXAudio();
        _Instance._fxSource.PlayOneShot(_Instance._OpenTargetClip);
    }
    public static void PlayClickAudio()
    {
        StopFXAudio();
        _Instance._fxSource.PlayOneShot(_Instance._SelectClickClip);
    }
    public static void PlaySubmitAudio()
    {
        StopFXAudio();
        _Instance._fxSource.PlayOneShot(_Instance._SubmitClickClip);
    }
    public static void PlayGetItemAudio(){
        StopFXAudio();
        _Instance._fxSource.PlayOneShot(_Instance._GetItemClip);
    }
    #endregion

    public static void StopMusicAudio()
    {
        _Instance._musicSource.loop = false;
        _Instance._musicSource.Stop();
        //_Instance.StartCoroutine(_Instance.DecreaseVolumetoClose(_Instance._musicSource));
    }
    public static void StopPlayerAudio()
    {
        _Instance._playerSource.loop = false;
        _Instance._playerSource.Stop();
    }
    public static void StopAmbientAudio()
    {
        _Instance._ambientSource.loop = false;
        _Instance._ambientSource.Stop();
    }
    public static void StopFXAudio()
    {
        _Instance._fxSource.loop = false;
        _Instance._fxSource.Stop();
        
    }

    IEnumerator DecreaseVolumetoClose(AudioSource audio_)
    {
        if (audio_.volume > 0.01f)
        {
            audio_.volume = Mathf.Max(0, audio_.volume - _volumeChangeSpeed);
            yield return null;
        }

        audio_.Stop();
        // audio_.clip = nextClip;

        // if(audio_.volume < 0.99f)
        // {
        //     audio_.volume = Mathf.Max(0, audio_.volume + _volumeChangeSpeed);
        //     yield return new WaitForSeconds(0.01f);
        // }
        yield return null;
    }
}
