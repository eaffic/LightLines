using UnityEngine;
using GameEnumList;

/// <summary>
/// ステージに入った時のカメラ動画
/// </summary>
public class CameraIntoStageState : BaseState<StageCameraState> {
    private CameraFSM _fsm;

    public CameraIntoStageState(CameraFSM manager, StageCameraState type)
    {
        base.ThisState = type;
        _fsm = manager;
    }

    public override void OnEnter(StageCameraState oldState)
    {
        base.OnEnter(oldState);
        _fsm.TransitionState(base.ThisState, StageCameraState.Control);
    }

    public override void OnLateUpdate(float deltaTime)
    {
        base.OnLateUpdate(deltaTime);
        if (Timer > 2.0f)
        {
            _fsm.TransitionState(base.ThisState, StageCameraState.Control);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}