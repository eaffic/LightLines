using UnityEngine;
using GameEnumList;

/// <summary>
/// 走る状態
/// </summary>
public class PlayerRunState : BaseState<PlayerState> {
    private PlayerFSM _fsm;

    public PlayerRunState(PlayerFSM manager, PlayerState state)
    {
        base.ThisState = state;
        _fsm = manager;
    }

    public override void OnEnter(PlayerState oldState)
    {
        base.OnEnter(oldState);
        _fsm.PlayerMovementControl.SetCurrentState();
        AudioManager.Instance.Play("Player", "PlayerRun", true);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (_fsm.PlayerMovementControl.OnGround == false && _fsm.PlayerMovementControl.Velocity.y < -0.5f)
        {
            _fsm.TransitionState(base.ThisState, PlayerState.Fall);
        }

        //速度は最大歩行速度より低い場合、歩行状態に移行
        float input = GameInputManager.Instance.GetPlayerMoveInput().magnitude;
        float velocity = new Vector2(_fsm.PlayerMovementControl.Velocity.x, _fsm.PlayerMovementControl.Velocity.z).magnitude;
        if (Timer > 0.2f && (input < 0.995f || velocity < _fsm.PlayerData.MaxWalkSpeed * 0.8f))
        {
            _fsm.TransitionState(base.ThisState, PlayerState.Walk);
        }

        if (GameInputManager.Instance.GetPlayerJumpInput())
        {
            _fsm.TransitionState(base.ThisState, PlayerState.Jump);
        }

        if (GameInputManager.Instance.GetPlayerPushInput() && _fsm.PlayerActionsControl.CheckTargetBox())
        {
            _fsm.TransitionState(base.ThisState, PlayerState.Push);
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