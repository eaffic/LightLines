using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;


public class PlayerJumpState : BaseState
{
    private PlayerController _player;

    public PlayerJumpState(string name, PlayerController player) : base(name)
    {
        _player = player;

        OnEnterCallBack += OnEnterFunction;
        OnUpdateCallBack += OnUpdateFunction;
        OnExitCallBack += OnExitFunction;
    }

    private void OnEnterFunction(IState previewState)
    {
        Debug.Log("Jump Enter");
    }

    private void OnUpdateFunction(float deltaTime)
    {
        Debug.Log("Jump Update");
    }

    private void OnExitFunction(IState nextState)
    {
        Debug.Log("Jump Exit");
    }
}