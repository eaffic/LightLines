using UnityEngine;
using GameEnumList;

/// <summary>
/// カメラ操作状態
/// </summary>
public class CameraControlState : BaseState<StageCameraState>
{

    private CameraFSM _fsm;

    public CameraControlState(CameraFSM manager, StageCameraState type){
        base.ThisState = type;
        _fsm = manager;
    }

    public override void OnEnter(StageCameraState oldState)
    {
        base.OnEnter(oldState);
        _fsm.OrbitCameraController.enabled = true;
    }

    public override void OnLateUpdate(float deltaTime)
    {
        base.OnLateUpdate(deltaTime);
    }

    public override void OnExit()
    {
        _fsm.OrbitCameraController.enabled = false;
        base.OnExit();
    }
}