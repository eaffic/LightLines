using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameEnumList;

/// <summary>
/// 入力管理クラス
/// </summary>
public class GameInputManager : Singleton<GameInputManager>
{
    private static bool _banInput;
    public static bool BanInput { get => _banInput; set => _banInput = value; }

    private GameInputController _gameInputController;
    private InputActionMap _player;
    private InputActionMap _camera;
    private InputActionMap _ui;

    protected override void Awake()
    {
        _gameInputController = new GameInputController();
        _player = _gameInputController.Player;
        _camera = _gameInputController.Camera;
        _ui = _gameInputController.UI;

        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        _gameInputController.Enable();
    }

    private void OnDisable()
    {
        _gameInputController.Disable();
    }

    public Vector3 GetPlayerMoveInput()
    {
        if (GameManager.OnSceneChange) { return Vector3.zero; }

        var value = _gameInputController.Player.Move.ReadValue<Vector2>();
        return new Vector3(value.x, 0f, value.y);
    }

    public Vector2 GetCameraRotateInput()
    {
        if (GameManager.OnSceneChange) { return Vector2.zero; }

        var value = _gameInputController.Camera.Rotate.ReadValue<Vector2>();
        return new Vector2(-value.y, value.x);
    }
    
    public bool GetPlayerJumpInput()
    {
        if (GameManager.OnSceneChange) { return false; }

        return _gameInputController.Player.Jump.triggered;
    }

    public bool GetPlayerPushInput()
    {
        if (GameManager.OnSceneChange) { return false; }

        return _gameInputController.Player.Push.triggered;
    }

    public bool GetCameraZoomInInput()
    {
        if (GameManager.OnSceneChange) { return false; }

        return _gameInputController.Camera.ZoomIn.ReadValue<float>() > 0.1f;
    }

    public bool GetCameraZoomOutInput()
    {
        if (GameManager.OnSceneChange) { return false; }

        return _gameInputController.Camera.ZoomOut.ReadValue<float>() > 0.1f;
    }

    public bool GetUISelectInput(out float input)
    {
        if (GameManager.OnSceneChange) { input = 0f; return false; }

        input = _gameInputController.UI.Select.ReadValue<float>();
        return _gameInputController.UI.Select.triggered;
    }

    public bool GetUISubmitInput(){
        if (GameManager.OnSceneChange) { return false; }

        return _gameInputController.UI.Submit.triggered;
    }
    
    public bool GetUIMenuInput()
    {
        if (GameManager.OnSceneChange) { return false; }

        return _gameInputController.UI.OpenCLoseMenu.triggered;
    }

    public void GetExitGameInput()
    {
        Application.Quit();
    }

    /// <summary>
    /// 入力管理
    /// </summary>
    /// <param name="limit"></param>
    public void SetPlayerInput(bool limit)
    {
        if (limit)
            _player.Enable();
        else
            _player.Disable();
    }

    public void SetCameraInput(bool limit)
    {
        if(limit)
            _camera.Enable();
        else
            _camera.Disable();
    }

    public void SetUIInput(bool limit)
    {
        if(limit)
            _ui.Enable();
        else
            _ui.Disable();
    }
}