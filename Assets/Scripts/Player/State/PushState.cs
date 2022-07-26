using UnityEngine;

public class PushState : BaseState<PlayerStateType> {
    private PlayerFSM _manager;
    private CharacterData_SO _data;

    public PushState(PlayerFSM manager, PlayerStateType type)
    {
        StateType = type;
        _manager = manager;
        _data = manager.PlayerData;
    }

    public override void OnEnter(PlayerStateType previewState)
    {
        base.OnEnter(previewState);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnExit()
    {

    }
}