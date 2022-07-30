using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageCameraState{
    IntoStage ,
    Orbit, //プレイヤー操作可能
    Clear,
}

public class CameraFSM : MonoBehaviour {
    public CameraData_SO CameraData;
    private Dictionary<StageCameraState, IState<StageCameraState>> _states = new Dictionary<StageCameraState, IState<StageCameraState>>();
    private IState<StageCameraState> _currentState;

    private void Awake() {
        CameraData = new CameraData_SO();

        _states.Add(StageCameraState.IntoStage, new IntoStageState(this, StageCameraState.IntoStage));
        _states.Add(StageCameraState.Orbit, new OrbitState(this, StageCameraState.Orbit));
        _states.Add(StageCameraState.Clear, new ClearState(this, StageCameraState.Clear));

        
    }

    private void Update() {
        _currentState.OnUpdate(Time.deltaTime);
    }

    private void LateUpdate() {
        _currentState.OnLateUpdate(Time.deltaTime);
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