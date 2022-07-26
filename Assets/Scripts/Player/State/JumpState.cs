using UnityEngine;

public class JumpState : BaseState<PlayerStateType> {
    private PlayerFSM _manager;
    private CharacterData_SO _data;

    public JumpState(PlayerFSM manager, PlayerStateType type)
    {
        StateType = type;
        _manager = manager;
        _data = manager.PlayerData;
    }

    public override void OnEnter(PlayerStateType previewState)
    {
        base.OnEnter(previewState);

        _manager.PlayerMovementController.Jump();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if(_manager.PlayerMovementController.OnGround){
            _manager.TransitionState(StateType, PreviewState);
        }
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnExit()
    {

    }
}