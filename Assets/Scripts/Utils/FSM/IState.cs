/// <summary>
/// 状態マシンの基本構成
/// </summary>
/// <typeparam name="T">対応マシンの列挙型</typeparam>
public interface IState<T> {
    /// <summary>
    /// このStateの種類
    /// </summary>
    /// <value></value>
    T ThisState { get; set; } 

    /// <summary>
    /// Stateに入る時
    /// </summary>
    /// <param name="oldState"></param>
    void OnEnter(T oldState); 

    /// <summary>
    /// Stageの更新
    /// </summary>
    /// <param name="deltaTime">更新間隔</param>
    void OnUpdate(float deltaTime);

    /// <summary>
    /// Stageの更新、Update終了の後実行する
    /// </summary>
    /// <param name="deltaTime">更新間隔</param>
    void OnLateUpdate(float deltaTime);

    /// <summary>
    /// Stageの更新、更新間隔固定
    /// </summary>
    void OnFixedUpdate();

    /// <summary>
    /// 離脱する時
    /// </summary>
    void OnExit();
}