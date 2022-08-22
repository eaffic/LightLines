using UnityEngine;
using GameEnumList;

/// <summary>
/// メニュを開く
/// </summary>
public class PlayerMenuState : BaseState<PlayerState> {
    private PlayerFSM _fsm;

    public PlayerMenuState(PlayerFSM manager, PlayerState state)
    {
        base.ThisState = state;
        _fsm = manager;
    }

    public override void OnEnter(PlayerState oldState)
    {
        base.OnEnter(oldState);
        _fsm.PlayerMovementControl.SetCurrentState();
        AudioManager.Instance.Stop("Player");
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if(GameManager.Pause == false){
            _fsm.TransitionState(base.ThisState, base.OldState);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}