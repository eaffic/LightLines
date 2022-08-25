using UnityEngine;

/// <summary>
/// ステージ内のリアルデータ更新、管理
/// </summary>
public class StageDataManager : UnitySingleton<StageDataManager> {

    public int TotalScretItemCountInStage = 0;
    private StageInfo _currentStageClearInfo;

    private float _timer = 0f;
    [SerializeField]private int _getItemCount;

    private void OnEnable() {
        EventCenter.AddStageSecretItemListener(GetItemNotify);
    }

    private void OnDisable() {
        EventCenter.RemoveStageSecretItemListener(GetItemNotify);
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
        DataManager.SaveStageClearData(_currentStageClearInfo);
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
        return info;
    }

    // ステージ入る前のリセット
    public void StartNewStage(){
        // ステージのクリア状況を取得する
        _currentStageClearInfo = DataManager.GetStageInfo(GameManager.CurrentScene);
        _currentStageClearInfo.StageType = GameManager.CurrentScene;

        _timer = 0f;
        _getItemCount = 0;
        TotalScretItemCountInStage = 0;
    }
}