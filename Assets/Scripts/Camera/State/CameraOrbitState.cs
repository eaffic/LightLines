using UnityEngine;
using GameEnumList;

public class CameraOrbitState : BaseState<StageCameraState>
{

    private CameraFSM _fsm;

    public CameraOrbitState(CameraFSM manager, StageCameraState type){
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