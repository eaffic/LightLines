using UnityEngine;

/// <summary>
/// 走る状態
/// </summary>
public class PlayerRunState : BaseState<PlayerState> {
    private PlayerFSM _fsm;

    public PlayerRunState(PlayerFSM manager, PlayerState type)
    {
        base.ThisStateType = type;
        _fsm = manager;
    }

    public override void OnEnter(PlayerState previewState)
    {
        base.OnEnter(previewState);
        _fsm.PlayerMovementController.SetCurrentState(base.ThisStateType);
        _fsm.PlayerAudioController.SetPlayerRunAudio();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        Vector2 velocity = new Vector2(_fsm.PlayerData.Velocity.x, _fsm.PlayerData.Velocity.z);
        //速度は最大歩行速度より低い場合、歩行状態に移行
        if (Timer > 0.2f && velocity.magnitude < _fsm.PlayerData.MaxWalkSpeed)
        {
            _fsm.TransitionState(base.ThisStateType, PlayerState.Walk);
        }

        if (GameInputManager.Instance.GetPlayerJumpInput())
        {
            _fsm.TransitionState(base.ThisStateType, PlayerState.Jump);
        }
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnExit()
    {

    }
}