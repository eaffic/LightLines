public class BaseState<T> : IState<T>
{
    private T _oldState;
    public T OldState => _oldState;

    private T _thisState;
    public T ThisState { 
        get => _thisState; 
        set => _thisState = value;
    }

    private float _timer;
    public float Timer => _timer;
    
    public virtual void OnEnter(T oldState){
        _oldState = oldState;
        _timer = 0;
    }

    public virtual void OnUpdate(float deltaTime){
        _timer += deltaTime;
    }

    public virtual void OnLateUpdate(float deltaTime)
    {
        _timer += deltaTime;
    }

    public virtual void OnFixedUpdate(){

    }

    public virtual void OnExit(){

    }
} 