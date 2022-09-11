using UnityEngine;

/// <summary>
/// ステージ内のリアルデータ更新、管理
/// </summary>
public class StageDataManager : UnitySingleton<StageDataManager> {

    public int TotalScretItemCountInStage = 0;
    [SerializeField] private StageInfo _currentStageClearInfo;

    private float _timer = 0f;
    [SerializeField]private int _getItemCount;

    private void OnEnable() {
        EventCenter.AddStageItemListener(GetItemNotify);
    }

    private void OnDisable() {
        EventCenter.RemoveStageItemListener(GetItemNotify);
    }

    protected override void Awake() {
        base.Awake();
    }

    private void Update() {
        if (GameManager.Pause) { return; }

        //経過時間計算
        _timer += Time.deltaTime;
    }

    public void GetItemNotify(){
        ++_getItemCount;
    }

    /// <summary>
    /// 今回のクリアデータと過去のクリアデータを比較して、セーブする
    /// </summary>
    public void SaveStageClearData(){
        _currentStageClearInfo.ClearTime = _timer;
        _currentStageClearInfo.SecretItemMaxCount = TotalScretItemCountInStage;
        _currentStageClearInfo.SecretItemCount = _getItemCount;

        _currentStageClearInfo.ClearCount++;
        _currentStageClearInfo.IsCompleteClear = (_getItemCount == TotalScretItemCountInStage); //アイテム全部取得
        //Debug.Log(_getItemCount == TotalScretItemCountInStage);
        //Debug.Log(_currentStageClearInfo.IsCompleteClear);
        DataManager.Instance.SaveStageClearData(_currentStageClearInfo);
    }

    /// <summary>
    /// 現在のステージ状況を返す
    /// </summary>
    /// <returns></returns>
    public StageInfo GetCurrentStageInfo(){
        StageInfo info = new StageInfo();
        info.StageType = GameManager.CurrentScene;
        info.ClearTime = _timer;
        info.SecretItemMaxCount = TotalScretItemCountInStage;
        info.SecretItemCount = _getItemCount;
        info.ClearCount = _currentStageClearInfo.ClearCount;
        info.IsCompleteClear = _currentStageClearInfo.IsCompleteClear;
        return info;
    }

    // ステージ入る前のリセット
    public void StartNewStage(){
        // ステージのクリア状況を取得する
        _currentStageClearInfo = DataManager.Instance.GetStageInfo(GameManager.CurrentScene);
        _currentStageClearInfo.StageType = GameManager.CurrentScene;

        _timer = 0f;
        _getItemCount = 0;
        TotalScretItemCountInStage = 0;
    }

    public bool NewTimeRecord(){
        return _currentStageClearInfo.ClearTime == 0 || _currentStageClearInfo.ClearTime > _timer;
    }

    public bool NewItemRecord(){
        return _currentStageClearInfo.SecretItemCount < _getItemCount;
    }
}