using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class PlayerRunState : BaseState
{
    private PlayerController _player;

    public PlayerRunState(string name, PlayerController player) : base(name)
    {
        _player = player;

        OnEnterCallBack += OnEnterFunction;
        OnUpdateCallBack += OnUpdateFunction;
        OnExitCallBack += OnExitFunction;
    }

    private void OnEnterFunction(IState previewState)
    {
        Debug.Log("Run Enter");
    }

    private void OnUpdateFunction(float deltaTime)
    {
        Debug.Log("Run Update");
    }

    private void OnExitFunction(IState nextState)
    {
        Debug.Log("Run Exit");
    }
}