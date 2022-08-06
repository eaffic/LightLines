using UnityEngine;
using GameEnumList;

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
        _fsm.PlayerMovementController.SetCurrentState();
        _fsm.PlayerMovementController.ResetMoveSpeed();
        _fsm.PlayerAudioController.StopAudio();
    }

    public override void OnUpdate(float deltaTime){
        base.OnUpdate(deltaTime);

        if(GameInputManager.Instance.GetPlayerMoveInput().magnitude > 0.2f){
            _fsm.TransitionState(ThisStateType, PlayerState.Walk);
        }

        if(GameInputManager.Instance.GetPlayerJumpInput()){
            _fsm.TransitionState(ThisStateType, PlayerState.Jump);
        }

        if(GameInputManager.Instance.GetPlayerPushInput() && _fsm.PlayerActionsController.CheckTargetBox()){
            _fsm.TransitionState(ThisStateType, PlayerState.Push);
        }

        if(GameManager.OpenMenu){
            _fsm.TransitionState(ThisStateType, PlayerState.Menu);
        }
    }

    public override void OnExit(){
        base.OnExit();
    }
}