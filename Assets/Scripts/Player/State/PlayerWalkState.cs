using UnityEngine;
using GameEnumList;

/// <summary>
/// 歩く状態
/// </summary>
public class PlayerWalkState : BaseState<PlayerState> {
    private PlayerFSM _fsm;

    public PlayerWalkState(PlayerFSM manager, PlayerState state)
    {
        base.ThisState = state;
        _fsm = manager;
    }

    public override void OnEnter(PlayerState oldState)
    {
        base.OnEnter(oldState);
        _fsm.PlayerMovementController.SetCurrentState();
        AudioManager.Instance.Play("Player", "Walk", true);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (_fsm.PlayerMovementController.OnGround == false && _fsm.PlayerData.Velocity.y < -0.5f)
        {
            _fsm.TransitionState(base.ThisState, PlayerState.Fall);
        }

        //前状態はRunの場合、一定の速度を達したらRunに移行する
        float input = GameInputManager.Instance.GetPlayerMoveInput().magnitude;
        float velocity = new Vector2(_fsm.PlayerData.Velocity.x, _fsm.PlayerData.Velocity.z).magnitude;
        if (velocity > _fsm.PlayerData.MaxWalkSpeed * 0.8f && input > 0.995f)
        {
            _fsm.TransitionState(base.ThisState, PlayerState.Run);
        }

        if(Timer > 0.2f && velocity < 0.1f && input < 0.05f){
            _fsm.TransitionState(base.ThisState, PlayerState.Idle);
        }

        if(GameInputManager.Instance.GetPlayerJumpInput()){
            _fsm.TransitionState(base.ThisState, PlayerState.Jump);
        }

        if (GameInputManager.Instance.GetPlayerPushInput() && _fsm.PlayerActionsController.CheckTargetBox())
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