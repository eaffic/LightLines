using UnityEngine;

/// <summary>
/// 走る状態
/// </summary>
public class RunState : BaseState<PlayerStateType> {
    private PlayerFSM _fsm;

    public RunState(PlayerFSM manager, PlayerStateType type)
    {
        base.ThisStateType = type;
        _fsm = manager;
    }

    public override void OnEnter(PlayerStateType previewState)
    {
        base.OnEnter(previewState);
        _fsm.PlayerMovementController.SetCurrentState(base.ThisStateType);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        Vector2 velocity = new Vector2(_fsm.PlayerData.Velocity.x, _fsm.PlayerData.Velocity.z);
        //速度は最大歩行速度より低い場合、歩く状態に移行
        if (Timer > 0.2f && velocity.magnitude < _fsm.PlayerData.MaxWalkSpeed)
        {
            _fsm.TransitionState(base.ThisStateType, PlayerStateType.Walk);
        }

        if (InputManager.Instance.GetPlayerJumpInput())
        {
            _fsm.TransitionState(base.ThisStateType, PlayerStateType.Jump);
        }
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnExit()
    {

    }
}