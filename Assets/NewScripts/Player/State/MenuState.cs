using UnityEngine;

/// <summary>
/// メニュを開く
/// </summary>
public class MenuState : BaseState<PlayerState> {
    private PlayerFSM _fsm;

    public MenuState(PlayerFSM manager, PlayerState type)
    {
        base.ThisStateType = type;
        _fsm = manager;
    }

    public override void OnEnter(PlayerState previewstate)
    {
        base.OnEnter(previewstate);
        _fsm.PlayerMovementController.SetCurrentState(ThisStateType);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (GameInputManager.Instance.GetUIMenuInput())
        {
            _fsm.TransitionState(base.ThisStateType, PreviewState);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}