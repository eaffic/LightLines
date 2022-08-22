using UnityEngine;

/// <summary>
/// ステージ内のリアルデータ更新、管理
/// </summary>
public class StageDataManager : UnitySingleton<StageDataManager> {

    public int TotalScretItemCountInStage = 0;
    private StageInfo _currentStageInfo;

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

    private void Start() {
        _currentStageInfo = DataManager.GetStageInfo(GameManager.CurrentScene);
    }

    private void Update() {
        if (GameManager.Pause) { return; }

        //経過時間計算
        _timer += Time.deltaTime;
    }

    public void GetItemNotify(){
        ++_getItemCount;
    }

    public void SaveStageClearData(){
        _currentStageInfo.StageType = GameManager.CurrentScene;
        _currentStageInfo.ClearTime = _timer;
        _currentStageInfo.SecretItemMaxCount = TotalScretItemCountInStage;
        _currentStageInfo.SecretItemCount = _getItemCount;
        DataManager.SaveStageClearData(_currentStageInfo);
    }

    public StageInfo GetCurrentStageInfo(){
        StageInfo info = new StageInfo();
        info.StageType = GameManager.CurrentScene;
        info.ClearTime = _timer;
        info.SecretItemMaxCount = TotalScretItemCountInStage;
        info.SecretItemCount = _getItemCount;
        return info;
    }
}