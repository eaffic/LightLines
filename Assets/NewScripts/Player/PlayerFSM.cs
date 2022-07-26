using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType{
    Idle, Walk, Run, Jump, Push, Menu
}

public class PlayerFSM : MonoBehaviour {
    public CharacterData_SO PlayerData; //キャラデータ
    public PlayerMovement PlayerMovementController; //移動制御
    public PlayerAnimation PlayerAnimationController; //動画制御
    public PlayerActions PlayerActionsController; //動作制御
    private IState<PlayerStateType> _currentState;
    private Dictionary<PlayerStateType, IState<PlayerStateType>> _states = new Dictionary<PlayerStateType, IState<PlayerStateType>>();

    private void Awake() {
        PlayerData.PlayerInputSpace = Camera.main.gameObject.transform;

        TryGetComponent(out PlayerMovementController);
        TryGetComponent(out PlayerAnimationController);

        _states.Add(PlayerStateType.Idle, new IdleState(this, PlayerStateType.Idle));
        _states.Add(PlayerStateType.Walk, new WalkState(this, PlayerStateType.Walk));
        _states.Add(PlayerStateType.Run, new RunState(this, PlayerStateType.Run));
        _states.Add(PlayerStateType.Jump, new JumpState(this, PlayerStateType.Jump));
        _states.Add(PlayerStateType.Push, new PushState(this, PlayerStateType.Push));
        _states.Add(PlayerStateType.Menu, new MenuState(this, PlayerStateType.Menu));

        TransitionState(default, PlayerStateType.Idle); //初期状態
    }

    private void Update() {
        _currentState.OnUpdate(Time.deltaTime);
        Debug.Log(_currentState);
    }

    private void FixedUpdate() {
        _currentState.OnFixedUpdate();
    }

    /// <summary>
    /// 状態遷移
    /// </summary>
    /// <param name="type"></param>
    public void TransitionState(PlayerStateType now, PlayerStateType next){
        if(_currentState != null){
            _currentState.OnExit();
        }
        _currentState = _states[next];
        _currentState.OnEnter(now);
    }

    public PlayerStateType GetCurrentStateType(){
        return _currentState.ThisStateType;
    }
}