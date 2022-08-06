using UnityEngine;
using GameEnumList;

public class OrbitState : BaseState<StageCameraState>
{

    private CameraFSM _fsm;

    public OrbitState(CameraFSM manager, StageCameraState type){
        base.ThisStateType = type;
        _fsm = manager;
    }

    public override void OnEnter(StageCameraState previewState)
    {
        base.OnEnter(previewState);
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