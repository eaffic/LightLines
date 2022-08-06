using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameEnumList;

/// <summary>
/// シーン切り替え用エフェクト
/// </summary>
public class FadeInOut : MonoBehaviour
{
    public static bool OnSceneChange = false; //Scene移行中(誤操作防止のため)

    public float TransitionSpeed; //エフェクト速度
    public float ResetTime; //リセットの所要時間
    public bool Reset { get; set; }
    public Image ResetImage; //リセット表示画像

    private SceneType _sceneToLoad; //シーン名
    private Image _theImage;
    private bool _shouldReveal; //フェードイン、アウト制御
    private float _resetTimer; //リセット時間計算

    private void OnEnable() {
        EventCenter.AddFadeListener(Notify);
    }

    private void OnDisable() {
        EventCenter.RemoveFadeListener(Notify);
    }

    void Start()
    {
        _theImage = GetComponent<Image>();
        _theImage.material.SetFloat("_Cutoff", -1f);
        _shouldReveal = true;
        OnSceneChange = false;
    }

    void Update()
    {
        // if (Reset) { ResetScene(); }
        // else { _resetTimer = 0; ResetImage.fillAmount = 0; }

        //黒幕表現処理
        if (_shouldReveal)
        {
            //FadeIn
            _theImage.material.SetFloat("_Cutoff", Mathf.MoveTowards(_theImage.material.GetFloat("_Cutoff"), 1.1f, TransitionSpeed * Time.deltaTime));
        }
        else
        {
            //FadeOut
            _theImage.material.SetFloat("_Cutoff", Mathf.MoveTowards(_theImage.material.GetFloat("_Cutoff"), -1f, TransitionSpeed * Time.deltaTime));

            //新しいシーンに遷移する前の音声処理
            if (_theImage.material.GetFloat("_Cutoff") <= -0.1f - _theImage.material.GetFloat("_Smoothing"))
            {
                SceneManager.LoadScene((int)_sceneToLoad);
                GameManager.OpenMenu = false;
                GameManager.StageClear = false;
                GameManager.CurrentScene = _sceneToLoad;
            }
        }
    }

    //----------------------------------------------------
    //リセット処理
    public void ResetScene()
    {
        _resetTimer += Time.deltaTime;
        ResetImage.fillAmount = _resetTimer / ResetTime;

        if (_resetTimer > ResetTime)
        {
            _sceneToLoad = (SceneType)SceneManager.GetActiveScene().buildIndex;
            Notify(_sceneToLoad);
        }
    }

    public void Notify(SceneType type){
        Debug.Log("a");
        _sceneToLoad = type;
        OnSceneChange = true;
        _shouldReveal = false;
    }
}
