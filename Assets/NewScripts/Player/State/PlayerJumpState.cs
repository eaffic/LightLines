using UnityEngine;

/// <summary>
/// ジャンプ状態
/// </summary>
public class PlayerJumpState : BaseState<PlayerState> {
    private PlayerFSM _fsm;
    
    public PlayerJumpState(PlayerFSM manager, PlayerState type)
    {
        base.ThisStateType = type;
        _fsm = manager;
    }

    public override void OnEnter(PlayerState previewState)
    {
        base.OnEnter(previewState);
        _fsm.PlayerMovementController.SetCurrentState(base.ThisStateType);
        _fsm.PlayerAnimationController.PlayJumpAnimation();
        _fsm.PlayerAudioController.SetPlayerJumpAudio();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if(Timer > 0.2f && _fsm.PlayerMovementController.OnGround){
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