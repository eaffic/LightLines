using UnityEngine;
using GameEnumList;

/// <summary>
/// プッシュ状態
/// </summary>
public class PlayerPushState : BaseState<PlayerState> {
    private PlayerFSM _fsm;

    public PlayerPushState(PlayerFSM manager, PlayerState type)
    {
        base.ThisStateType = type;
        _fsm = manager;
    }

    public override void OnEnter(PlayerState previewState)
    {
        base.OnEnter(previewState);
        _fsm.PlayerMovementController.SetCurrentState();
        _fsm.PlayerAnimationController.PlayPushAnimation();
        _fsm.PlayerAudioController.StopAudio();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if(Timer > 1f){
            _fsm.TransitionState(ThisStateType ,PlayerState.Idle);
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