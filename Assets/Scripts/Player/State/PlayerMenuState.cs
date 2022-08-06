using UnityEngine;
using GameEnumList;

/// <summary>
/// メニュを開く
/// </summary>
public class PlayerMenuState : BaseState<PlayerState> {
    private PlayerFSM _fsm;

    public PlayerMenuState(PlayerFSM manager, PlayerState type)
    {
        base.ThisStateType = type;
        _fsm = manager;
    }

    public override void OnEnter(PlayerState previewstate)
    {
        base.OnEnter(previewstate);
        _fsm.PlayerMovementController.SetCurrentState();
        _fsm.PlayerMovementController.ResetMoveSpeed();
        _fsm.PlayerAudioController.StopAudio();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if(GameManager.OpenMenu == false){
            _fsm.TransitionState(base.ThisStateType, base.PreviewState);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}