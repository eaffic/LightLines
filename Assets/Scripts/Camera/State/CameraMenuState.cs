using UnityEngine;
using GameEnumList;

public class CameraMenuState : BaseState<StageCameraState> {
    private CameraFSM _fsm;

    public CameraMenuState(CameraFSM manager, StageCameraState type){
        base.ThisState = type;
        _fsm = manager;
    }

    public override void OnEnter(StageCameraState oldState)
    {
        base.OnEnter(oldState);
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