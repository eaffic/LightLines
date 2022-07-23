using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class PlayerPushState : BaseState
{
    private PlayerController _player;

    public PlayerPushState(string name, PlayerController player) : base(name)
    {
        _player = player;
        OnEnterCallBack += OnEnterFunction;
        OnUpdateCallBack += OnUpdateFunction;
        OnExitCallBack += OnExitFunction;
    }

    private void OnEnterFunction(IState previewState)
    {
        Debug.Log("Push Enter");
    }

    private void OnUpdateFunction(float deltaTime)
    {
        Debug.Log("Push Update");
    }

    private void OnExitFunction(IState nextState)
    {
        Debug.Log("Push Exit");
    }
}
