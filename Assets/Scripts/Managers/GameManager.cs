using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameEnumList;

public class GameManager : UnitySingleton<GameManager>
{
    private static SceneType _currentScene;
    public static SceneType CurrentScene => _currentScene;

    private static bool _onSceneChange; //Scene移行中
    public static bool OnSceneChange { get => _onSceneChange; set => _onSceneChange = value; }

    private static bool _pause; //一時停止(UIを開くとき、操作を制限したい時)
    public static bool Pause { get => _pause; set => _pause = value; }

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneStartSetting();
        AudioManager.Instance.StartNewScene();
    }

    /// <summary>
    /// 新しいシーンの開始設定
    /// </summary>
    public void SceneStartSetting()
    {
        switch (CurrentScene)
        {
            case SceneType.Title:
                AudioManager.Instance.Play("BackGround", "BGMTitle", true);
                UIManager.Instance.ShowUIView("Title/UITitle");
                break;
            case SceneType.StageSelect:
                AudioManager.Instance.Play("BackGround", "BGMStageSelect", true);
                UIManager.Instance.ShowUIView("StageSelect/UIStageSelectMenu");
                UIManager.Instance.ShowUIView("StageSelect/UIStageInformation");
                UIManager.Instance.ShowUIView("InputHint/UIGameInputHint");
                break;
            case SceneType.Stage1_1:
            case SceneType.Stage1_2:
            case SceneType.Stage1_3:
                AudioManager.Instance.Play("BackGround", "BGMStage", true);
                UIManager.Instance.ShowUIView("Stage/UIStageMenu");
                UIManager.Instance.ShowUIView("Stage/UIStageClear");
                UIManager.Instance.ShowUIView("InputHint/UIGameInputHint");
                break;
        }
    }

    public void StartToLoadNewScene(SceneType targetScene)
    {
        StartCoroutine(LoadNewScene(targetScene));
    }

    /// <summary>
    /// bgmの音量調整
    /// </summary>
    /// <param name="targetScene"></param>
    /// <returns></returns> 
    IEnumerator LoadNewScene(SceneType targetScene)
    {
        _currentScene = targetScene;
        GameManager.Pause = true;
        UIManager.Instance.ClearUI(); //前シーンのUIを削除する

        float volume = 1f;
        while (volume > 0.1f)
        {
            volume = Mathf.Max(volume - Time.deltaTime, 0f);
            AudioManager.Instance.SetAllVolume(volume);
            yield return null;
        }

        AudioManager.Instance.StopAllSource();
        SceneManager.LoadSceneAsync((int)targetScene, LoadSceneMode.Additive); //シーン遷移
        StartCoroutine(SetActiveScene()); 

        SceneStartSetting();
        AudioManager.Instance.StartNewScene();
        FadeInOut.Instance.StartFadeIn(); //シーンロード終了、シーン開始
        GameManager.Pause = false;
        yield return null;
    }

    /// <summary>
    /// メイン活動シーン設置(ライト設定など)
    /// </summary>
    /// <returns></returns>
    IEnumerator SetActiveScene(){
        Scene scene = SceneManager.GetSceneByBuildIndex((int)CurrentScene);
        while (!scene.isLoaded)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(scene);
        yield return null;
    }

    public void SetStartCurrentScene(SceneType type){
        _currentScene = type;
    }
}