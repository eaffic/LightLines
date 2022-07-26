using UnityEngine;

/// <summary>
/// 歩く状態
/// </summary>
public class WalkState : BaseState<PlayerStateType> {
    private PlayerFSM _fsm;

    public WalkState(PlayerFSM manager, PlayerStateType type)
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
        //前状態はRunの場合、一定の速度を達したらRunに移行する
        if (PreviewState == PlayerStateType.Run &&
            velocity.magnitude > _fsm.PlayerData.MaxWalkSpeed - 0.1f)
        {
            _fsm.TransitionState(base.ThisStateType, PlayerStateType.Run);
        }

        if(Timer > 0.2f && velocity.magnitude < 0.1f){
            _fsm.TransitionState(base.ThisStateType, PlayerStateType.Idle);
        }

        if(InputManager.Instance.GetPlayerRunInput()){
            _fsm.TransitionState(base.ThisStateType, PlayerStateType.Run);
        }

        if(InputManager.Instance.GetPlayerJumpInput()){
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