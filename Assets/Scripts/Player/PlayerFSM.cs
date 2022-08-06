using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnumList;

public class PlayerFSM : MonoBehaviour {
    public CharacterData_SO PlayerData; //キャラデータ
    public PlayerMovement PlayerMovementController; //移動制御
    public PlayerAnimation PlayerAnimationController; //動画制御
    public PlayerActions PlayerActionsController; //動作制御
    public PlayerAudio PlayerAudioController; //音声制御
    
    private IState<PlayerState> _currentState;
    public PlayerState CurrentState => _currentState.ThisStateType;
    private Dictionary<PlayerState, IState<PlayerState>> _states = new Dictionary<PlayerState, IState<PlayerState>>();

    private void Awake() {
        //PlayerData = ScriptableObject.CreateInstance<CharacterData_SO>();
        PlayerData.PlayerInputSpace = Camera.main.gameObject.transform;
        

        TryGetComponent(out PlayerMovementController);
        TryGetComponent(out PlayerAnimationController);
        TryGetComponent(out PlayerActionsController);
        TryGetComponent(out PlayerAudioController);

        _states.Add(PlayerState.Idle, new PlayerIdleState(this, PlayerState.Idle));
        _states.Add(PlayerState.Walk, new PlayerWalkState(this, PlayerState.Walk));
        _states.Add(PlayerState.Run, new PlayerRunState(this, PlayerState.Run));
        _states.Add(PlayerState.Jump, new PlayerJumpState(this, PlayerState.Jump));
        _states.Add(PlayerState.Push, new PlayerPushState(this, PlayerState.Push));
        _states.Add(PlayerState.Menu, new PlayerMenuState(this, PlayerState.Menu));

        TransitionState(default, PlayerState.Idle); //初期状態
    }

    private void Update() {
        _currentState.OnUpdate(Time.deltaTime);
        Debug.Log(_currentState.ThisStateType);
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