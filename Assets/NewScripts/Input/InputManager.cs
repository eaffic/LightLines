using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 入力管理クラス
/// </summary>
public class InputManager : Singleton<InputManager>
{
    private GameInputController _gameInputController;

    private InputActionMap _player;
    private InputActionMap _camera;
    private InputActionMap _ui;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        _gameInputController = new GameInputController();
        _player = _gameInputController.Player;
        _camera = _gameInputController.Camera;
        _ui = _gameInputController.UI;
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
        var value = _gameInputController.Player.Move.ReadValue<Vector2>();
        return new Vector3(value.x, 0f, value.y);
    }

    public Vector2 GetCameraRotateInput()
    {
        var value = _gameInputController.Camera.Rotate.ReadValue<Vector2>();
        return new Vector2(-value.y, value.x);
    }

    public bool GetPlayerRunInput()
    {
        return _gameInputController.Player.Run.triggered;
    }

    public bool GetPlayerJumpInput()
    {
        return _gameInputController.Player.Jump.triggered;
    }

    public bool GetPlayerPushInput()
    {
        return _gameInputController.Player.Push.triggered;
    }

    public bool GetCameraZoomInInput()
    {
        return _gameInputController.Camera.ZoomIn.ReadValue<float>() > 0.1f;
    }

    public bool GetCameraZoomOutInput()
    {
        return _gameInputController.Camera.ZoomOut.ReadValue<float>() > 0.1f;
    }

    public bool GetUIMenuInput()
    {
        return _gameInputController.UI.OpenCLoseMenu.triggered;
    }

    public float GetUISelectUpInput()
    {
        return _gameInputController.UI.Select.ReadValue<float>();
    }

    public bool GetExitGameInput()
    {
        return _gameInputController.UI.ExitGame.triggered;
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