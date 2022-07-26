using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン切り替え用エフェクト
/// </summary>
public class FadeInOut : MonoBehaviour
{
    public static bool _OnSceneChange = false; //Scene移行中(誤操作防止のため)

    public string _SceneToLoad; //シーン名
    public float _TransitionSpeed; //エフェクト速度
    public float _ResetTime; //リセットの所要時間
    public bool Reset{ get; set; }
    public Image _ResetImage; //リセット表示画像

    Image _theImage;
    bool _shouldReveal; //フェードイン、アウト制御
    float _resetTimer; //リセット時間計算

    void Start()
    {
        try{
            StageManager._Instance.RegisterFadeInOut(this);
        }catch{}
        
        _theImage = GetComponent<Image>();
        _theImage.material.SetFloat("_Cutoff", -0.3f);
        _shouldReveal = true;
    }

    void Update()
    {
        if (Reset) { ResetScene(); }
        else { _resetTimer = 0; _ResetImage.fillAmount = 0; }

        //黒幕表現処理
        if (_shouldReveal)
        {
            //FadeIn
            _theImage.material.SetFloat("_Cutoff", Mathf.MoveTowards(_theImage.material.GetFloat("_Cutoff"), 1.1f, _TransitionSpeed * Time.deltaTime));
        }
        else
        {
            //FadeOut
            _theImage.material.SetFloat("_Cutoff", Mathf.MoveTowards(_theImage.material.GetFloat("_Cutoff"), -1f, _TransitionSpeed * Time.deltaTime));

            //新しいシーンに遷移する前の音声処理
            if (_theImage.material.GetFloat("_Cutoff") <= -0.1f - _theImage.material.GetFloat("_Smoothing"))
            {
                AudioManager.StopMusicAudio();
                AudioManager.StopPlayerAudio();
                AudioManager.StopFXAudio();
                AudioManager.StopAmbientAudio();
                SceneManager.LoadScene(_SceneToLoad);
                _OnSceneChange = false;
            }
        }
    }

    //----------------------------------------------------
    public void StartFadeOut()
    {
        _OnSceneChange = true;
        _shouldReveal = false;
    }

    //リセット処理
    public void ResetScene()
    {
        _resetTimer += Time.deltaTime;
        _ResetImage.fillAmount = _resetTimer / _ResetTime;

        if (_resetTimer > _ResetTime)
        {
            _SceneToLoad = SceneManager.GetActiveScene().name;
            StartFadeOut();
        }
    }
}
