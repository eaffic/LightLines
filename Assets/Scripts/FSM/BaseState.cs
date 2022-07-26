public class BaseState<T> : IState<T>
{
    private T _previewState;
    public T PreviewState => _previewState;

    private T _stateType;
    public T StateType { 
        get => _stateType; 
        set => _stateType = value;
    }

    private float _timer;
    public float Timer => _timer;
    
    public virtual void OnEnter(T previewState){
        _previewState = previewState;
        _timer = 0;
    }

    public virtual void OnUpdate(float deltaTime){
        _timer += deltaTime;
    }

    public virtual void OnFixedUpdate(){

    }

    public virtual void OnLateUpdate(float deltaTime){
        _timer += deltaTime;
    }

    public virtual void OnExit(){

    }
} 