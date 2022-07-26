using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



/// <summary>
/// ステージ進行の管理、ステージデータを一時記録する
/// </summary>
public class StageManager : MonoBehaviour
{
    public static StageManager _Instance;

    [Header("ステージ")]
    [SerializeField] ResultUIController _resultUI; //ステージクリア表示用リザルトUI
    [SerializeField] MenuUIController _menuUI; //ステージ内のメニュー表示
    [SerializeField] StageGimmick _clearArea;  //クリアエリア
    [SerializeField] FadeInOut _fadeInOut; //シーン移行
    [SerializeField] List<StageGimmick> _targetGroup; //レーザー目標
    [SerializeField] List<StageGimmick> _movingFloorGroup; //移動地面
    [SerializeField] List<StageGimmick> _laserGroup; //レーザー
    [SerializeField] List<StageGimmick> _buttonGroup; //ボタン
    [SerializeField] List<StageGimmick> _rotateBlockGroup; //回転ブロック 
    [SerializeField] List<GameObject> _secretItemGroup; //隠しアイテム

    StageInfo _stageInfo; //一時保存データ
    bool _stageClear;
    float _timer; //クリア時間記録

    public bool IsStageClear { get => _stageClear; set => _stageClear = value; }

    void Awake()
    {
        //シングルトーン
        if (_Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _Instance = this;

        _targetGroup = new List<StageGimmick>();
        _movingFloorGroup = new List<StageGimmick>();
        _laserGroup = new List<StageGimmick>();
        _buttonGroup = new List<StageGimmick>();
        _rotateBlockGroup = new List<StageGimmick>();
        _secretItemGroup = new List<GameObject>();
    }

    void Start()
    {
        Cursor.visible = false; //マウスを隠す
    }

    //-------------------------------------------------------------------------------------
    //ステージ開始（開始タイム記録）
    public void StageStart()
    {
        if(GameManager._ActiveSceneIndex == SCENEINDEX.TITLE || GameManager._ActiveSceneIndex == SCENEINDEX.STAGESELECT) { return; }

        //ステージのデータを取得
        _stageInfo = DataManager._Instance.GetStageInfo(SceneManager.GetSceneByBuildIndex((int)GameManager._ActiveSceneIndex).name);
        _stageInfo.SecretItemMaxCount = this._secretItemGroup.Count;
        _timer = Time.time; //開始時間記録
    }

    /// <summary>
    /// メニューのステージ情報アップデート（ステージ選択画面以外）
    /// </summary>
    public void UpdateStageInfomation()
    {
        if (GameManager._ActiveSceneIndex == SCENEINDEX.TITLE) return;

        float tempTime = Time.time - _timer;
        _stageInfo.ClearTime = tempTime;
        _stageInfo.SecretItemCount = _stageInfo.SecretItemMaxCount - _secretItemGroup.Count;
        _menuUI.SetMenuUIInfo(_stageInfo);
    }

    //-------------------------------------------------------------------------------------
    //ステージクリアUI
    public void RegisterResultUI(ResultUIController result)
    {
        _resultUI = result;
    }
    
    //ステージメニューUI
    public void RegisterMenuUI(MenuUIController menu)
    {
        _menuUI = menu;
    }

    //シーンエフェクト
    public void RegisterFadeInOut(FadeInOut fade)
    {
        _fadeInOut = fade;
    }

    //アイテム登録
    public void RegisterTarget(StageGimmick target)
    {
        if(!_targetGroup.Contains(target))
        {
            _targetGroup.Add(target);
        }

        StageClearCheck();
    }
    public void RegisterMovingFloor(StageGimmick floor)
    {
        if(!_movingFloorGroup.Contains(floor))
        {
            _movingFloorGroup.Add(floor);
        }
    }
    public void RegisterLaser(StageGimmick laser)
    {
        if (!_laserGroup.Contains(laser))
        {
            _laserGroup.Add(laser);
        }
    }
    public void RegisterButton(StageGimmick button)
    {
        if (!_buttonGroup.Contains(button))
        {
            _buttonGroup.Add(button);
        }
    }
    public void RegisterClearArea(StageGimmick clearArea)
    {
        _clearArea = clearArea;
    }
    public void RegisterRotateBlock(StageGimmick rotateBlock)
    {
        if(!_rotateBlockGroup.Contains(rotateBlock))
        {
            _rotateBlockGroup.Add(rotateBlock);
        }
    }
    public void RegisterSecretItem(GameObject secretItem)
    {
        if (!_secretItemGroup.Contains(secretItem))
        {
            _secretItemGroup.Add(secretItem);
        }
    }

    //起動したターゲットを除去
    public void RemoveTarget(StageGimmick target)
    {
        if (_targetGroup.Contains(target))
        {
            _targetGroup.Remove(target);
        }

        //クリアチェック
        StageClearCheck();
    }

    //獲得したアイテムを除去
    public void RemoveGetSecretItem(GameObject item)
    {
        if (_secretItemGroup.Contains(item))
        {
            _secretItemGroup.Remove(item);
        }
    }

    /// <summary>
    /// 対応番号のアイテムを起動する
    /// </summary>
    /// <param name="num"></param>
    public void OpenSameNumberItem(int num)
    {
        foreach (var item in _movingFloorGroup)
        {
            if (item._Number == num && !item.IsOpen)
            {
                item.Open();
            }
        }

        foreach (var item in _laserGroup)
        {
            if(item._Number == num && !item.IsOpen)
            {
                item.Open();
            }
        }

        foreach (var item in _rotateBlockGroup)
        {
            if(item._Number == num && !item.IsOpen){
                item.Open();
            }   
        }
    }

    /// <summary>
    /// 対応番号のアイテムを終了する
    /// </summary>
    /// <param name="num"></param>
    public void CloseSameNumberItem(int num)
    {
        foreach (var item in _movingFloorGroup)
        {
            if (item._Number == num && item.IsOpen)
            {
                item.Close();
            }
        }

        foreach (var item in _laserGroup)
        {
            if (item._Number == num && item.IsOpen)
            {
                item.Close();
            }
        }

        foreach (var item in _rotateBlockGroup)
        {
            if(item._Number == num && item.IsOpen)
            {
                item.Close();
            }
        }
    }

    /// <summary>
    /// 同じ番号のボタンを取得する
    /// </summary>
    /// <param name="num"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    public bool GetSameNumberButton(int num, out List<StageGimmick> items)
    {
        items = new List<StageGimmick>();
        foreach (var button in _buttonGroup)
        {
            if (button._Number == num)
            {
                items.Add(button);
            }
        }

        return items.Count > 0;
    }

    //クリア判定、クリアエフェクト起動
    void StageClearCheck()
    {
        if(_clearArea == null) { return; }

        if (_targetGroup.Count == 0)
        {
            _clearArea.Open();
            AudioManager.PlayMagicCircleAudio();
        }
        else
        {
            _clearArea.Close();
        }
    }

    /// <summary>
    /// 指定のシーンに移行
    /// </summary>
    /// <param name="sceneName"></param>
    public void MoveToSelectScene(string sceneName)
    {
        _fadeInOut._SceneToLoad = sceneName;
        _fadeInOut.StartFadeOut();
    }

    /// <summary>
    /// 次のステージに移行
    /// </summary>
    public void MoveToNextStageScene()
    {
        if((int)GameManager._ActiveSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            _fadeInOut._SceneToLoad = "StageSelect";
            _fadeInOut.StartFadeOut();
            return;
        }

        string path = SceneUtility.GetScenePathByBuildIndex((int)GameManager._ActiveSceneIndex + 1);
        string name = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
        
        _fadeInOut._SceneToLoad = name;
        _fadeInOut.StartFadeOut();
    }

    /// <summary>
    /// ステージクリア処理
    /// </summary>
    public void Clear()
    {
        //_fadeInOut.StartFadeOut();
        IsStageClear = true;

        //クリア時間を記録、保存する
        _stageInfo.StageName = SceneManager.GetSceneByBuildIndex((int)GameManager._ActiveSceneIndex).name;
        _stageInfo.ClearTime = Time.time - _timer;
        _stageInfo.SecretItemCount = _stageInfo.SecretItemMaxCount - _secretItemGroup.Count;
        DataManager._Instance.SaveStageClearData(_stageInfo);

        //カメラ移動
        Camera.main.GetComponent<OrbitCamera>().StageSelectPeriod(true);
        //リザルトUI表示
        _resultUI.SetClearStageInfo(this._stageInfo);
    }

    //シーンリセット
    public void ResetGameStart()
    {
        _fadeInOut.Reset = true;
    }

    public void ResetGameStop()
    {
        _fadeInOut.Reset = false;
    }
}
