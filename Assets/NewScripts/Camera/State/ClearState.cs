using UnityEngine;

/// <summary>
/// ステージクリア時のカメラ動画
/// </summary>
public class ClearState : BaseState<StageCameraState> {
    private CameraFSM _fsm;

    public ClearState(CameraFSM manager, StageCameraState type)
    {
        base.ThisStateType = type;
        _fsm = manager;
    }

    public override void OnEnter(StageCameraState previewState)
    {
        base.OnEnter(previewState);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    public override void OnLateUpdate(float deltaTime)
    {
        base.OnLateUpdate(deltaTime);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}