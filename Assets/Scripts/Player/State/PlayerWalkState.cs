using UnityEngine;
using GameEnumList;

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
        _fsm.PlayerMovementController.SetCurrentState();
        _fsm.PlayerAudioController.SetPlayerWalkAudio();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        _fsm.PlayerActionsController.SearchBox();


        //前状態はRunの場合、一定の速度を達したらRunに移行する
        float input = GameInputManager.Instance.GetPlayerMoveInput().magnitude;
        float velocity = new Vector2(_fsm.PlayerData.Velocity.x, _fsm.PlayerData.Velocity.z).magnitude;
        if (velocity > _fsm.PlayerData.MaxWalkSpeed * 0.9f && input > 0.99f)
        {
            _fsm.TransitionState(ThisStateType, PlayerState.Run);
        }

        if(Timer > 0.2f && velocity < 0.1f && input < 0.05f){
            _fsm.TransitionState(ThisStateType, PlayerState.Idle);
        }

        if(GameInputManager.Instance.GetPlayerJumpInput()){
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