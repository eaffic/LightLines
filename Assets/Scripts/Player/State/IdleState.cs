using UnityEngine;

public class IdleState : BaseState<PlayerStateType> {
    private PlayerFSM _manager;
    private CharacterData_SO _data;

    public IdleState(PlayerFSM manager, PlayerStateType type){
        StateType = type;
        _manager = manager;
        _data = manager.PlayerData;
    }

    public override void OnEnter(PlayerStateType previewstate){
        base.OnEnter(previewstate);
        _manager.PlayerMovementController.UpdateMovement = false;
    }

    public override void OnUpdate(float deltaTime){
        base.OnUpdate(deltaTime);

        if(InputManager.Instance.GetPlayerMoveInput().magnitude > 0){
            _manager.TransitionState(StateType, PlayerStateType.Walk);
        }

        if(InputManager.Instance.GetPlayerJumpInput()){
            _manager.TransitionState(StateType, PlayerStateType.Jump);
        }

        if(InputManager.Instance.GetPlayerPushInput()){
            _manager.TransitionState(StateType, PlayerStateType.Push);
        }
    }

    public override void OnFixedUpdate(){
        base.OnFixedUpdate();
    }

    public override void OnExit(){
        base.OnExit();
    }
}