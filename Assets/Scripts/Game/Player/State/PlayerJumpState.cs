using UnityEngine;
using GameEnumList;

/// <summary>
/// ジャンプ状態
/// </summary>
public class PlayerJumpState : BaseState<PlayerState> {
    private PlayerFSM _fsm;
    
    public PlayerJumpState(PlayerFSM manager, PlayerState state)
    {
        base.ThisState = state;
        _fsm = manager;
    }

    public override void OnEnter(PlayerState oldState)
    {
        base.OnEnter(oldState);
        _fsm.PlayerMovementControl.SetCurrentState();
        _fsm.PlayerAnimationControl.PlayJumpAnimation();
        AudioManager.Instance.Play("Player", "PlayerJump", false);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        _fsm.PlayerActionsControl.SearchBox();

        if (_fsm.PlayerMovementControl.OnGround == false && _fsm.PlayerMovementControl.Velocity.y < -0.1f)
        {
            _fsm.TransitionState(base.ThisState, PlayerState.Fall);
        }

        //Fall状態に移行する前に地面と接触すると、前状態に戻る（高さちょうどいい段階を上るときなど）
        if(Timer > 0.2f && _fsm.PlayerMovementControl.OnGround){
            _fsm.TransitionState(base.ThisState, base.OldState);
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