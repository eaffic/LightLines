using UnityEngine;

/// <summary>
/// 通常状態
/// </summary>
public class PlayerIdleState : BaseState<PlayerState> {
    private PlayerFSM _fsm;

    public PlayerIdleState(PlayerFSM manager, PlayerState type){
        base.ThisStateType = type;
        _fsm = manager;
    }

    public override void OnEnter(PlayerState previewstate){
        base.OnEnter(previewstate);
        _fsm.PlayerMovementController.SetCurrentState(ThisStateType);
        _fsm.PlayerMovementController.ResetMoveSpeed();
        _fsm.PlayerAudioController.StopAudio();
    }

    public override void OnUpdate(float deltaTime){
        base.OnUpdate(deltaTime);

        if(GameInputManager.Instance.GetPlayerMoveInput().magnitude > 0){
            _fsm.TransitionState(base.ThisStateType, PlayerState.Walk);
        }

        if(GameInputManager.Instance.GetPlayerJumpInput()){
            _fsm.TransitionState(base.ThisStateType, PlayerState.Jump);
        }

        if(GameInputManager.Instance.GetPlayerPushInput() && _fsm.PlayerActionsController.CheckTargetBox()){
            _fsm.TransitionState(base.ThisStateType, PlayerState.Push);
        }
    }

    public override void OnFixedUpdate(){
        base.OnFixedUpdate();
    }

    public override void OnExit(){
        base.OnExit();
    }
}