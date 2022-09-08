using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using GameEnumList;

[System.Serializable]
//セーブデータ
public class SaveData
{
    [SerializeField]
    private SceneType _lastClearStage; //クリアしたの最新ステージ
    [SerializeField]
    private List<StageInfo> _stageInfoList = new List<StageInfo>(); //各ステージの記録
    [SerializeField]
    private int _itemCount; //取集したすべてのアイテム数


    public SceneType LastClearStage { 
        get => _lastClearStage;
        set => _lastClearStage = _lastClearStage < value ? value : _lastClearStage;
    }
    public List<StageInfo> StageInfoList
    {
        get => _stageInfoList;
    }
    public int ItemCount{
        get{
            int count = 0;
            for (int i = 0; i < _stageInfoList.Count; ++i){
                count += _stageInfoList[i].SecretItemCount;
            }
            return count;
        }
    }

    public SaveData()
    {
        this.LastClearStage = 0;
    }
    public SaveData(SceneType clearStage, List<StageInfo> stageInfo)
    {
        this.LastClearStage = clearStage;
        _stageInfoList = stageInfo;
    }
}

[System.Serializable]
//ステージ記録
public struct StageInfo
{
    [SerializeField]
    private SceneType _stageType; //ステージ種類
    [SerializeField]
    private float _clearTime; //クリア時間
    [SerializeField]
    private int _secretItemCount; //取ったアイテム数
    [SerializeField]
    private int _secretItemMaxCount; // ステージ内のアイテム総数
    [SerializeField]
    private int _clearCount; //クリア回数
    [SerializeField]
    private bool _isCompleteClear; //コンプリートクリア記録

    public SceneType StageType { get => _stageType; set => _stageType = value; }
    public float ClearTime
    {
        get => _clearTime;
        set
        {
            if (value < 0) { return; }

            if (_clearTime > value || _clearTime == 0)
            {
                _clearTime = value;
            }
        }
    }
    public int SecretItemCount
    {
        get => _secretItemCount;
        set
        {
            if (value > _secretItemMaxCount) { return; }

            if (_secretItemCount < value)
            {
                _secretItemCount = value;
            }
        }
    }

    public int SecretItemMaxCount { 
        get => _secretItemMaxCount; 
        set => _secretItemMaxCount = value;
    }

    public int ClearCount {
        get => _clearCount;
        set => _clearCount = value;
    }

    public bool IsCompleteClear{
        get => _isCompleteClear;
        set => _isCompleteClear |= value;
    }
}

/// <summary>
/// データ管理、操作用クラス
/// </summary>
public class DataManager : UnitySingleton<DataManager>
{
    [SerializeField] private SaveData _saveData;

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(this.gameObject);

        _saveData = LoadFromJson<SaveData>("savedata.json"); //既有データをロードする
    }

    public SceneType GetLastClearStage(){
        return _saveData.LastClearStage;
    }

    /// <summary>
    /// クリア記録を保存、または更新する
    /// </summary>
    /// <param name="stageInfo"></param>
    public void SaveStageClearData(StageInfo stageInfo)
    {
        

        bool checkdata = false;
        for (int i = 0; i < _saveData.StageInfoList.Count; ++i)
        {
            //データが存在しているとき、データを更新する
            if (stageInfo.StageType == _saveData.StageInfoList[i].StageType)
            {
                checkdata = true;
                StageInfo temp = _saveData.StageInfoList[i];
                //クリア時間
                temp.ClearTime = stageInfo.ClearTime;
                //隠しアイテム
                temp.SecretItemCount = stageInfo.SecretItemCount;
                temp.ClearCount = stageInfo.ClearCount;
                temp.IsCompleteClear = stageInfo.IsCompleteClear;
                _saveData.StageInfoList[i] = temp;
            }
        }

        //データが存在していないとき、データを追加する
        if (!checkdata)
        {
            _saveData.StageInfoList.Add(stageInfo);
            _saveData.LastClearStage = GameManager.CurrentScene;
        }

        SaveByJson("savedata.json", _saveData);
    }

    /// <summary>
    /// 記録があるステージ記録を取得する
    /// </summary>
    /// <param name="stageName"></param>
    /// <returns></returns>
    public StageInfo GetStageInfo(SceneType type)
    {
        if (_saveData == null) return new StageInfo();

        for (int i = 0; i < _saveData.StageInfoList.Count; ++i)
        {
            if (_saveData.StageInfoList[i].StageType == type)
            {
                return _saveData.StageInfoList[i];
            }
        }

        // 該当ステージは存在していない場合、空白の記録データを返す
        return new StageInfo();
    }

    /// <summary>
    /// データ保存
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    /// <param name="data">セーブデータ</param>
    public void SaveByJson(string fileName, object data)
    {
        //DeleteSaveFile(fileName);
        var json = JsonUtility.ToJson(data);
        //保存位置、デバイスによって変更する
        var path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            File.WriteAllText(path, json);
            Debug.Log("save success");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// ファイルからの読み込み
    /// </summary>
    /// <typeparam name="T">データタイプ</typeparam>
    /// <param name="fileName">ファイル名</param>
    /// <returns>データ</returns>
    public T LoadFromJson<T>(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<T>(json);
                Debug.Log("load success");
                return data;
            }
            else
            {
                Debug.Log("create new data");
                return System.Activator.CreateInstance<T>();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return default;
        }
    }

    /// <summary>
    /// セーブデータ削除
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    public void DeleteSaveFile(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            File.Delete(path);
            _saveData = LoadFromJson<SaveData>("savedata.json");
            Debug.Log("delete success");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
