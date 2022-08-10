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
    public float TransitionSpeed; //エフェクト速度

    private SceneType _sceneToLoad; //シーン名
    private Image _theImage;

    private void OnEnable() {
        EventCenter.AddFadeListener(Notify);
    }

    private void OnDisable() {
        EventCenter.RemoveFadeListener(Notify);
    }

    void Start()
    {
        _theImage = GetComponent<Image>();
        _theImage.material.SetFloat("_Cutoff", -1.1f);
        GameManager.OnSceneChange = true;
        StartFadeIn();
    }

    public void StartFadeIn(){
        StartCoroutine(FadeIn());
    }

    public void StartFadeOut(){
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn(){
        float cutoff = _theImage.material.GetFloat("_Cutoff");
        while(cutoff < 1f){
            cutoff = Mathf.MoveTowards(_theImage.material.GetFloat("_Cutoff"), 1.1f, TransitionSpeed * Time.deltaTime);
            _theImage.material.SetFloat("_Cutoff", cutoff);
            yield return null;
        }
        _theImage.material.SetFloat("_Cutoff", 1.1f);
        //フェード効果完成
        GameManager.OnSceneChange = false;
        GameManager.Pause = false;
        yield return null;
    }

    IEnumerator FadeOut(){
        float cutoff = _theImage.material.GetFloat("_Cutoff");
        while (cutoff > -1f)
        {
            cutoff = Mathf.MoveTowards(_theImage.material.GetFloat("_Cutoff"), -1.1f, TransitionSpeed * Time.deltaTime);
            _theImage.material.SetFloat("_Cutoff", cutoff);
            yield return null;
        }
        _theImage.material.SetFloat("_Cutoff", -1.1f);
        //新しいシーンに遷移する
        GameManager.Instance.StartToLoadNewScene(_sceneToLoad);
        
        yield return null;
    }

    //----------------------------------------------------
    public void Notify(SceneType type){
        GameManager.OnSceneChange = true;
        _sceneToLoad = type;
        StartCoroutine(FadeOut());
    }
}
