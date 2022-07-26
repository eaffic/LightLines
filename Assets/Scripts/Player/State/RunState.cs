using UnityEngine;

public class RunState : BaseState<PlayerStateType> {
    private PlayerFSM _manager;
    private CharacterData_SO _data;

    public RunState(PlayerFSM manager, PlayerStateType type)
    {
        StateType = type;
        _manager = manager;
        _data = manager.PlayerData;
    }

    public override void OnEnter(PlayerStateType previewState)
    {
        base.OnEnter(previewState);
        _manager.PlayerMovementController.UpdateMovement = true;
        _manager.PlayerMovementController.SetStateMoveData(_data.MaxRunSpeed, _data.RunAcceleration);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (Timer > 0.2f && _data.Velocity.magnitude < _data.MaxWalkSpeed)
        {
            _manager.TransitionState(StateType, PlayerStateType.Walk);
        }

        if (InputManager.Instance.GetPlayerRunInput())
        {
            _manager.TransitionState(StateType, PlayerStateType.Walk);
        }

        if (InputManager.Instance.GetPlayerJumpInput())
        {
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