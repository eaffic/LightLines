using UnityEngine;

/// <summary>
/// 歩く状態
/// </summary>
public class PlayerWalkState : BaseState<PlayerState> {
    private PlayerFSM _fsm;

    public PlayerWalkState(PlayerFSM manager, PlayerState type)
    {
        base.ThisStateType = type;
        _fsm = manager;
    }

    public override void OnEnter(PlayerState previewState)
    {
        base.OnEnter(previewState);
        _fsm.PlayerMovementController.SetCurrentState(base.ThisStateType);
        _fsm.PlayerAudioController.SetPlayerWalkAudio();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        Vector2 velocity = new Vector2(_fsm.PlayerData.Velocity.x, _fsm.PlayerData.Velocity.z);
        //前状態はRunの場合、一定の速度を達したらRunに移行する
        if (PreviewState == PlayerState.Run &&
            velocity.magnitude > _fsm.PlayerData.MaxWalkSpeed - 0.1f)
        {
            _fsm.TransitionState(base.ThisStateType, PlayerState.Run);
        }

        if(Timer > 0.2f && velocity.magnitude < 0.1f){
            _fsm.TransitionState(base.ThisStateType, PlayerState.Idle);
        }

        if(GameInputManager.Instance.GetPlayerRunInput()){
            _fsm.TransitionState(base.ThisStateType, PlayerState.Run);
        }

        if(GameInputManager.Instance.GetPlayerJumpInput()){
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