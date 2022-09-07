using UnityEngine;
using GameEnumList;

/// <summary>
/// プッシュ状態
/// </summary>
public class PlayerPushState : BaseState<PlayerState> {
    private PlayerFSM _fsm;

    public PlayerPushState(PlayerFSM manager, PlayerState state)
    {
        base.ThisState = state;
        _fsm = manager;
    }

    public override void OnEnter(PlayerState oldState)
    {
        base.OnEnter(oldState);
        _fsm.PlayerMovementControl.SetCurrentState();
        _fsm.PlayerAnimationControl.PlayPushAnimation();
        AudioManager.Instance.Stop("Player");
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (_fsm.PlayerMovementControl.Velocity.y < -1f)
        {
            _fsm.TransitionState(base.ThisState, PlayerState.Fall);
        }

        if(Timer > 1.2f){
            _fsm.TransitionState(base.ThisState ,PlayerState.Idle);
        }

        if (GameManager.Pause)
        {
            _fsm.TransitionState(base.ThisState, PlayerState.Menu);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}