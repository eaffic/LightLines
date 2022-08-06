public interface IState<T> {
    T ThisStateType { get; set; }

    void OnEnter(T previewState);
    void OnUpdate(float deltaTime);
    void OnLateUpdate(float deltaTime);
    void OnFixedUpdate();
    void OnExit();
}