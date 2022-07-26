public interface IState<T> {
    void OnEnter(T previewState);
    void OnUpdate(float deltaTime);
    void OnFixedUpdate();
    void OnLateUpdate(float deltaTime);
    void OnExit();
}