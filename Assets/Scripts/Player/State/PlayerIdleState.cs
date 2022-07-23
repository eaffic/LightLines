using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class PlayerIdleState : BaseState 
{
    private PlayerController _player;
    
    public PlayerIdleState(string name, PlayerController player) : base(name)
    {
        _player = player;

        OnEnterCallBack += OnEnterFunction;
        OnUpdateCallBack += OnUpdateFunction;
        OnExitCallBack += OnExitFunction;
    }

    private void OnEnterFunction(IState previewState){
        Debug.Log("Idle Enter");
    }

    private void OnUpdateFunction(float deltaTime){
        Debug.Log("Idle Update");
    }

    private void OnExitFunction(IState nextState){
        Debug.Log("Idle Exit");
    }
}