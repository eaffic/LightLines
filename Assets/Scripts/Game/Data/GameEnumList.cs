namespace GameEnumList
{
    /// <summary>
    /// ビルドしたのシーンリスト
    /// </summary>
    public enum SceneType
    {
        Title,
        UI,
        Manager,
        StageSelect,
        Stage1_1,
        Stage1_2,
        Stage1_3,
        Stage1_4,
        Stage1_5,
        Stage1_6,
        Stage1_7,
        Stage1_8,
        Stage1_9,
        Stage1_10,
        NULLSCENE, //nextStageシーン遷移確認用
    }

    /// <summary>
    /// プレイヤーキャラの状態
    /// </summary>
    public enum PlayerState
    {
        Idle, 
        Walk, 
        Run, 
        Jump, 
        Fall,
        Push, 
        Menu  //メニューなどのUIを開くとき
    }

    /// <summary>
    /// ステージ内のカメラ状態
    /// </summary>
    public enum StageCameraState
    {
        IntoStage, //ステージの開始動画カメラ
        Control, //プレイヤー操作可能
        Clear, //クリア動画カメラ
    }
}
