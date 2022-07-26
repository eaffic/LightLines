public interface IState<T> {
    T ThisStateType { get; set; }

    void OnEnter(T previewState);
    void OnUpdate(float deltaTime);
    void OnFixedUpdate();
    void OnLateUpdate(float deltaTime);
    void OnExit();
}