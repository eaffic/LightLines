using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class PlayerMoveState : BaseState
{
    private PlayerController _player;

    public PlayerMoveState(string name, PlayerController player) : base(name)
    {
        _player = player;

        OnEnterCallBack += OnEnterFunction;
        OnUpdateCallBack += OnUpdateFunction;
        OnExitCallBack += OnExitFunction;
    }

    private void OnEnterFunction(IState previewState)
    {
        Debug.Log("Move Enter");
    }

    private void OnUpdateFunction(float deltaTime)
    {
        Debug.Log("Move Update");
    }

    private void OnExitFunction(IState nextState)
    {
        Debug.Log("Move Exit");
    }
}