using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameEnumList;

public class GameManager : Singleton<GameManager>
{
    public static SceneType CurrentScene;

    private static bool _onSceneChange; //Scene移行中
    public static bool OnSceneChange { get => _onSceneChange; set => _onSceneChange = value; }

    private static bool _pause; //一時停止(UIを開くとき、操作を制限したい時)
    public static bool Pause { get => _pause; set => _pause = value; }

    protected override void Awake()
    {
        CurrentScene = (SceneType)SceneManager.GetActiveScene().buildIndex;

        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        SceneBGMSetting();
        AudioManager.Instance.StartNewScene();
    }

    /// <summary>
    /// bgm設定
    /// </summary>
    public void SceneBGMSetting(){
        switch(CurrentScene){
            case SceneType.Title:
                AudioManager.Instance.Play("BackGround", "Title", true);
                break;
            case SceneType.StageSelect:
                AudioManager.Instance.Play("BackGround", "StageSelect", true);
                break;
            case SceneType.Stage1_1:
            case SceneType.Stage1_2:
            case SceneType.Stage1_3:
                AudioManager.Instance.Play("BackGround", "Stage", true);
                break;
        }
    }

    public void StartToLoadNewScene(SceneType targetScene){
        StartCoroutine(LoadNewScene(targetScene));
    }

    /// <summary>
    /// bgmの音量調整
    /// </summary>
    /// <param name="targetScene"></param>
    /// <returns></returns>
    IEnumerator LoadNewScene(SceneType targetScene){
        GameManager.CurrentScene = targetScene;
        GameManager.Pause = false;
        
        float volume = 1f;
        while(volume > 0.1f){
            volume = Mathf.Max(volume - Time.deltaTime, 0f);
            AudioManager.Instance.SetAllVolume(volume);
            yield return null;
        }

        AudioManager.Instance.StopAllSource();
        SceneManager.LoadScene((int)targetScene); //シーン遷移
        SceneBGMSetting();
        AudioManager.Instance.StartNewScene();
        yield return null;
    }
}