namespace GameEnumList
{
    /// <summary>
    /// ビルド中のシーンリスト
    /// </summary>
    public enum SceneType
    {
        Title,
        StageSelect,
        Stage1_1,
        Stage1_2,
        Stage1_3,
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
        Push, 
        Menu  //メニューなどUIを開くとき
    }

    /// <summary>
    /// ギミックタイプ
    /// </summary>
    public enum GimmickType
    {
        RotateBlock,
        MoveBlock,
    }

    /// <summary>
    /// ステージ内のカメラ状態
    /// </summary>
    public enum StageCameraState
    {
        IntoStage,
        Orbit, //プレイヤー操作可能
        Clear,
    }

    /// <summary>
    /// インプットタイプリスト
    /// </summary>
    public enum InputType
    {
        PlayerMove,
        PlayerRun,
        PlayerJump,
        PlayerPush,
        CameraRotate,
        CameraZoomIn,
        CameraZoomOut,
        UIMenu,
        UISelect,
        UISubmit,
        ExitGame,
    }
}
