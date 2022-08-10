using UnityEngine;
using GameEnumList;

/// <summary>
/// 通常状態
/// </summary>
public class PlayerIdleState : BaseState<PlayerState>
{
    private PlayerFSM _fsm;

    public PlayerIdleState(PlayerFSM manager, PlayerState state)
    {
        base.ThisState = state;
        _fsm = manager;
    }

    public override void OnEnter(PlayerState oldState)
    {
        base.OnEnter(oldState);
        _fsm.PlayerMovementController.SetCurrentState();
        AudioManager.Instance.Stop("Player");
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (GameManager.Pause)
        {
            _fsm.TransitionState(base.ThisState, PlayerState.Menu);
        }

        if(_fsm.PlayerMovementController.OnGround == false && _fsm.PlayerData.Velocity.y < -0.5f)
        {
            _fsm.TransitionState(base.ThisState, PlayerState.Fall);
        }

        if (GameInputManager.Instance.GetPlayerMoveInput().magnitude > 0.2f)
        {
            _fsm.TransitionState(base.ThisState, PlayerState.Walk);
        }

        if (GameInputManager.Instance.GetPlayerJumpInput())
        {
            _fsm.TransitionState(base.ThisState, PlayerState.Jump);
        }

        if (GameInputManager.Instance.GetPlayerPushInput() && _fsm.PlayerActionsController.CheckTargetBox())
        {
            _fsm.TransitionState(base.ThisState, PlayerState.Push);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}