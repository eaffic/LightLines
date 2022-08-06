using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnumList;

public class CameraFSM : MonoBehaviour {
    public OrbitCamera OrbitCameraController; //カメラコントローラ

    private Dictionary<StageCameraState, IState<StageCameraState>> _states = new Dictionary<StageCameraState, IState<StageCameraState>>();
    private IState<StageCameraState> _currentState;
    

    private void Awake() {
        TryGetComponent(out OrbitCameraController);

        _states.Add(StageCameraState.IntoStage, new IntoStageState(this, StageCameraState.IntoStage));
        _states.Add(StageCameraState.Orbit, new OrbitState(this, StageCameraState.Orbit));
        _states.Add(StageCameraState.Clear, new ClearState(this, StageCameraState.Clear));

        TransitionState(default, StageCameraState.IntoStage);
    }

    private void LateUpdate() {
        _currentState.OnLateUpdate(Time.deltaTime);
        Debug.Log(_currentState.ThisStateType);
    }

    /// <summary>
    /// 状態遷移
    /// </summary>
    /// <param name="type"></param>
    public void TransitionState(StageCameraState now, StageCameraState next)
    {
        if (_currentState != null)
        {
            _currentState.OnExit();
        }
        _currentState = _states[next];
        _currentState.OnEnter(now);
    }
}