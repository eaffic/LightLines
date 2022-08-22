using UnityEngine;
using GameEnumList;

public class PlayerFallState : BaseState<PlayerState> {
    private PlayerFSM _fsm;

    public PlayerFallState(PlayerFSM manager, PlayerState state){
        base.ThisState = state;
        _fsm = manager;
    }

    public override void OnEnter(PlayerState oldState)
    {
        base.OnEnter(oldState);
        _fsm.PlayerMovementControl.SetCurrentState();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if(_fsm.PlayerMovementControl.OnGround){
            _fsm.TransitionState(base.ThisState, PlayerState.Idle);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        _fsm.PlayerMovementControl.ResetMoveSpeed();
        _fsm.PlayerMovementControl.OnAir = false;
    }
}