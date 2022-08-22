using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnumList;

// [RequireComponent(typeof(PlayerMovement))]
// [RequireComponent(typeof(PlayerAnimation))]
// [RequireComponent(typeof(PlayerActions))]
public class PlayerFSM : MonoBehaviour {
    public CharacterData_SO PlayerData; //キャラデータ
    public PlayerMovement PlayerMovementControl; //移動制御
    public PlayerAnimation PlayerAnimationControl; //動画制御
    public PlayerActions PlayerActionsControl; //動作制御
    
    private IState<PlayerState> _currentState;
    public PlayerState CurrentState => _currentState.ThisState;
    private Dictionary<PlayerState, IState<PlayerState>> _states = new Dictionary<PlayerState, IState<PlayerState>>();

    private void Awake() {
        PlayerData.PlayerInputSpace = Camera.main.gameObject.transform;
        

        TryGetComponent(out PlayerMovementControl);
        TryGetComponent(out PlayerAnimationControl);
        TryGetComponent(out PlayerActionsControl);

        _states.Add(PlayerState.Idle, new PlayerIdleState(this, PlayerState.Idle));
        _states.Add(PlayerState.Walk, new PlayerWalkState(this, PlayerState.Walk));
        _states.Add(PlayerState.Run, new PlayerRunState(this, PlayerState.Run));
        _states.Add(PlayerState.Jump, new PlayerJumpState(this, PlayerState.Jump));
        _states.Add(PlayerState.Fall, new PlayerFallState(this, PlayerState.Fall));
        _states.Add(PlayerState.Push, new PlayerPushState(this, PlayerState.Push));
        _states.Add(PlayerState.Menu, new PlayerMenuState(this, PlayerState.Menu));
    }

    private void Start() {
        TransitionState(default, PlayerState.Idle); //初期状態
    }

    private void Update() {
        PlayerActionsControl.SearchBox();
        _currentState.OnUpdate(Time.deltaTime);
        
        //Debug.Log(_currentState.ThisState);
    }

    /// <summary>
    /// 状態遷移
    /// </summary>
    /// <param name="type"></param>
    public void TransitionState(PlayerState now, PlayerState next){
        if(_currentState != null){
            _currentState.OnExit();
        }
        _currentState = _states[next];
        _currentState.OnEnter(now);
    }
}