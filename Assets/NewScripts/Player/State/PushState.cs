using UnityEngine;

/// <summary>
/// プッシュ状態
/// </summary>
public class PushState : BaseState<PlayerStateType> {
    private PlayerFSM _fsm;

    public PushState(PlayerFSM manager, PlayerStateType type)
    {
        base.ThisStateType = type;
        _fsm = manager;
    }

    public override void OnEnter(PlayerStateType previewState)
    {
        base.OnEnter(previewState);
        _fsm.PlayerMovementController.SetCurrentState(base.ThisStateType);
        _fsm.PlayerAnimationController.PlayPushAnimation();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if(Timer > 2f){
            _fsm.TransitionState(base.ThisStateType ,PlayerStateType.Idle);
        }
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnExit()
    {

    }
}