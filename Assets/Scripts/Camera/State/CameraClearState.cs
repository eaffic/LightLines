using UnityEngine;
using GameEnumList;

/// <summary>
/// ステージクリア時のカメラ動画
/// </summary>
public class CameraClearState : BaseState<StageCameraState> {
    private CameraFSM _fsm;

    public CameraClearState(CameraFSM manager, StageCameraState type)
    {
        base.ThisState = type;
        _fsm = manager;
    }

    public override void OnEnter(StageCameraState oldState)
    {
        base.OnEnter(oldState);
    }

    public override void OnLateUpdate(float deltaTime)
    {
        base.OnLateUpdate(deltaTime);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}