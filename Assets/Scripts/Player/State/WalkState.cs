using UnityEngine;

public class WalkState : BaseState<PlayerStateType> {
    private PlayerFSM _manager;
    private CharacterData_SO _data;

    public WalkState(PlayerFSM manager, PlayerStateType type)
    {
        StateType = type;
        _manager = manager;
        _data = manager.PlayerData;
    }

    public override void OnEnter(PlayerStateType previewState)
    {
        base.OnEnter(previewState);
        _manager.PlayerMovementController.UpdateMovement = true;
        _manager.PlayerMovementController.SetStateMoveData(_data.MaxWalkSpeed, _data.WalkAcceleration);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (PreviewState == PlayerStateType.Run &&
            _data.Velocity.magnitude > _data.MaxWalkSpeed - 0.1f)
        {
            _manager.TransitionState(StateType, PlayerStateType.Run);
        }

        if(Timer > 0.2f && _data.Velocity.magnitude < 0.1f){
            _manager.TransitionState(StateType, PlayerStateType.Idle);
        }

        if(InputManager.Instance.GetPlayerRunInput()){
            _manager.TransitionState(StateType, PlayerStateType.Run);
        }

        if(InputManager.Instance.GetPlayerJumpInput()){
            _manager.TransitionState(StateType, PlayerStateType.Jump);
        }
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnExit()
    {

    }
}