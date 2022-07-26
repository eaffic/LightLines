using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
//セーブデータ
public class SaveData
{
    [SerializeField]
    private int _clearStageCount; //クリアのステージ数
    [SerializeField]
    private List<StageInfo> _stageInfoList = new List<StageInfo>(); //各ステージの記録

    public int ClearStageCount { get => _clearStageCount; set => _clearStageCount = value; }
    public List<StageInfo> StageInfoList
    {
        get => _stageInfoList;
    }

    public SaveData()
    {
        this.ClearStageCount = 0;
    }
    public SaveData(int clearStage, List<StageInfo> stageInfo)
    {
        this.ClearStageCount = clearStage;
    }
}

[System.Serializable]
//ステージ記録
public struct StageInfo
{
    [SerializeField]
    private string _stageName;
    [SerializeField]
    private float _clearTime;
    [SerializeField]
    private int _secretItemCount;
    [SerializeField]
    private int _secretItemMaxCount;

    public string StageName { get => _stageName; set => _stageName = value; }
    public float ClearTime
    {
        get => _clearTime;
        set
        {
            if (value < 0) { return; }

            if (this._clearTime > value || this._clearTime == 0)
            {
                this._clearTime = value;
            }
        }
    }
    public int SecretItemCount
    {
        get => _secretItemCount;
        set
        {
            if (value > 3) { return; }

            if (this._secretItemCount < value)
            {
                this._secretItemCount = value;
            }
        }
    }

    public int SecretItemMaxCount { get => this._secretItemMaxCount; set => this._secretItemMaxCount = value; }
}

/// <summary>
/// データ管理、操作用クラス
/// </summary>
public class DataManager : MonoBehaviour
{
    [SerializeField] SaveData _saveData;

    public static DataManager _Instance;

    private void Awake()
    {
        if (_Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _Instance = this;
        DontDestroyOnLoad(this.gameObject);

        _saveData = LoadFromJson<SaveData>("savedata.json"); //既有データをロードする
    }

    /// <summary>
    /// クリア記録を保存、または更新する
    /// </summary>
    /// <param name="stageInfo"></param>
    public void SaveStageClearData(StageInfo stageInfo)
    {
        bool checkdata = false;

        for(int i = 0; i < _saveData.StageInfoList.Count; ++i)
        {
            //データが存在しているとき、データを更新する
            if(stageInfo.StageName == _saveData.StageInfoList[i].StageName)
            {
                checkdata = true;
                StageInfo temp = _saveData.StageInfoList[i];
                //クリア時間
                temp.ClearTime = stageInfo.ClearTime;
                //隠しアイテム
                temp.SecretItemCount = stageInfo.SecretItemCount;
                _saveData.StageInfoList[i] = temp;
            }
        }

        //データが存在していないとき、データを追加する
        if (!checkdata)
        {
            _saveData.StageInfoList.Add(stageInfo);
            _saveData.ClearStageCount++;
        }

        SaveByJson("savedata.json" ,_saveData);
    }

    /// <summary>
    /// 記録があるステージ記録を取得する
    /// </summary>
    /// <param name="stageName"></param>
    /// <returns></returns>
    public StageInfo GetStageInfo(string stageName)
    {
        if (_saveData == null) return new StageInfo();

        for (int i = 0; i < _saveData.StageInfoList.Count; ++i)
        {
            if(_saveData.StageInfoList[i].StageName == stageName)
            {
                return _saveData.StageInfoList[i];
            }
        }

        return new StageInfo();
    }

    /// <summary>
    /// データ保存
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    /// <param name="data">セーブデータ</param>
    public static void SaveByJson(string fileName, object data)
    {
        DeleteSaveFile(fileName);
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
    public static T LoadFromJson<T>(string fileName)
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
        catch(Exception e)
        {
            Debug.LogError(e);
            return default;
        }
    }

    /// <summary>
    /// セーブデータ削除
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    public static void DeleteSaveFile(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            File.Delete(path);
            Debug.Log("delete success");
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }
}
