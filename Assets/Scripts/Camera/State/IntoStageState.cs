using UnityEngine;
using GameEnumList;

/// <summary>
/// ステージに入った時のカメラ動画
/// </summary>
public class IntoStageState : BaseState<StageCameraState> {
    private CameraFSM _fsm;

    public IntoStageState(CameraFSM manager, StageCameraState type)
    {
        base.ThisStateType = type;
        _fsm = manager;
    }

    public override void OnEnter(StageCameraState previewState)
    {
        base.OnEnter(previewState);
        _fsm.TransitionState(ThisStateType, StageCameraState.Orbit);
    }

    public override void OnLateUpdate(float deltaTime)
    {
        base.OnLateUpdate(deltaTime);
        if (Timer > 2.0f)
        {
            _fsm.TransitionState(ThisStateType, StageCameraState.Orbit);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}