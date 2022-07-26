using UnityEngine;

public class OrbitState : BaseState<CameraStateType>
{

    private CameraFSM _manager;

    public OrbitState(CameraFSM manager, CameraStateType type){
        base.ThisStateType = type;
        _manager = manager;
    }

}