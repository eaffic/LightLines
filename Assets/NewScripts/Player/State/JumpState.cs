using UnityEngine;

/// <summary>
/// ジャンプ状態
/// </summary>
public class JumpState : BaseState<PlayerStateType> {
    private PlayerFSM _fsm;
    
    public JumpState(PlayerFSM manager, PlayerStateType type)
    {
        base.ThisStateType = type;
        _fsm = manager;
    }

    public override void OnEnter(PlayerStateType previewState)
    {
        base.OnEnter(previewState);
        _fsm.PlayerMovementController.SetCurrentState(base.ThisStateType);
        _fsm.PlayerAnimationController.PlayJumpAnimation();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if(_fsm.PlayerMovementController.OnGround){
            _fsm.TransitionState(base.ThisStateType, PreviewState);
        }
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnExit()
    {
        _fsm.PlayerMovementController.OnJump = false;
        _fsm.PlayerMovementController.ResetMoveSpeed();
    }
}