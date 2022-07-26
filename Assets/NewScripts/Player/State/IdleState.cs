using UnityEngine;

/// <summary>
/// 通常状態
/// </summary>
public class IdleState : BaseState<PlayerStateType> {
    private PlayerFSM _fsm;

    public IdleState(PlayerFSM manager, PlayerStateType type){
        base.ThisStateType = type;
        _fsm = manager;
    }

    public override void OnEnter(PlayerStateType previewstate){
        base.OnEnter(previewstate);
        _fsm.PlayerMovementController.SetCurrentState(ThisStateType);
    }

    public override void OnUpdate(float deltaTime){
        base.OnUpdate(deltaTime);

        if(InputManager.Instance.GetPlayerMoveInput().magnitude > 0){
            _fsm.TransitionState(base.ThisStateType, PlayerStateType.Walk);
        }

        if(InputManager.Instance.GetPlayerJumpInput()){
            _fsm.TransitionState(base.ThisStateType, PlayerStateType.Jump);
        }

        if(InputManager.Instance.GetPlayerPushInput()){
            _fsm.TransitionState(base.ThisStateType, PlayerStateType.Push);
        }
    }

    public override void OnFixedUpdate(){
        base.OnFixedUpdate();
    }

    public override void OnExit(){
        base.OnExit();
    }
}