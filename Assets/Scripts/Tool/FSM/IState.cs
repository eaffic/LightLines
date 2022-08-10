public interface IState<T> {
    T ThisState { get; set; }

    void OnEnter(T oldState);
    void OnUpdate(float deltaTime);
    void OnLateUpdate(float deltaTime);
    void OnFixedUpdate();
    void OnExit();
}