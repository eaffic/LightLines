using UnityEngine;
using FSM;

public class PlayerController : MonoBehaviour 
{
    public float flame = 0;

    public CharacterData_SO Data;
    public GameStateMachine Fsm;
    public InputActions Input;

    //状態
    private BaseState _idle;
    private BaseState _move;
    private BaseState _run;
    private BaseState _jump;
    private BaseState _push;

    //遷移状態
    private BaseTransition _idleMove;
    private BaseTransition _idleJump;
    private BaseTransition _idlePush;

    private BaseTransition _moveIdle;
    private BaseTransition _moveRun;
    private BaseTransition _moveJump;

    private BaseTransition _runMove;
    private BaseTransition _runJump;

    private BaseTransition _jumpIdle;

    private BaseTransition _pushIdle;

    private void Awake() {
        Data.PlayerInputSpace = Camera.main.gameObject.transform;
        InitializeFSM();
    }

    private void Update() {
        Fsm.OnUpdate(Time.deltaTime);
    }

    private void InitializeFSM(){
        Input = new InputActions();
        _idle = new PlayerIdleState("Idle", this);
        _idleMove = new BaseTransition("IdleMove", _idle, _move);
        _idleJump = new BaseTransition("IdleJump", _idle, _jump);
        _idlePush = new BaseTransition("IdlePush", _idle, _push);

        _move = new PlayerMoveState("Move", this);
        _moveIdle = new BaseTransition("MoveIdle", _move, _idle);
        _moveJump = new BaseTransition("MoveJump", _move, _jump);
        _moveRun = new BaseTransition("MoveRun", _move, _run);

        _run = new PlayerRunState("Run", this);
        _runMove = new BaseTransition("RunMove", _run, _move);
        _runJump = new BaseTransition("RunJump", _run, _jump);

        _jump = new PlayerJumpState("Jump", this);
        _jumpIdle = new BaseTransition("JumpIdle", _jump, _idle);

        _push = new PlayerPushState("Push", this);
        _pushIdle = new BaseTransition("PushIdle", _push, _idle);
    
        Fsm = new GameStateMachine("PlayerState", _idle);
        Fsm.AddState(_idle);
        Fsm.AddState(_move);
        Fsm.AddState(_run);
        Fsm.AddState(_jump);
        Fsm.AddState(_push);

        Fsm.OnEnter(_idle); //デフォルート状態の初期化
    }
}