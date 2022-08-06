using UnityEngine;
using GameEnumList;

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
        _fsm.PlayerMovementController.SetCurrentState();
        _fsm.PlayerAudioController.SetPlayerRunAudio();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        _fsm.PlayerActionsController.SearchBox();

        //速度は最大歩行速度より低い場合、歩行状態に移行
        float input = GameInputManager.Instance.GetPlayerMoveInput().magnitude;
        float velocity = new Vector2(_fsm.PlayerData.Velocity.x, _fsm.PlayerData.Velocity.z).magnitude;
        if (Timer > 0.2f && (input < 0.99f || velocity < _fsm.PlayerData.MaxWalkSpeed))
        {
            _fsm.TransitionState(ThisStateType, PlayerState.Walk);
        }

        if (GameInputManager.Instance.GetPlayerJumpInput())
        {
            _fsm.TransitionState(ThisStateType, PlayerState.Jump);
        }

        if (GameManager.OpenMenu)
        {
            _fsm.TransitionState(ThisStateType, PlayerState.Menu);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}