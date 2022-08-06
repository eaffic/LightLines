using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    private PlayerFSM _fsm;
    private Animator _animator;

    private void Awake() {
        TryGetComponent(out _fsm);
        TryGetComponent(out _animator);
    }

    private void Update()
    {
        float moveInput = GameInputManager.Instance.GetPlayerMoveInput().magnitude;
        _animator.SetFloat("MoveInput", moveInput);
        float velocity = new Vector2(_fsm.PlayerData.Velocity.x, _fsm.PlayerData.Velocity.z).magnitude;
        _animator.SetFloat("Velocity", velocity / _fsm.PlayerData.MaxRunSpeed);

        _animator.SetBool("OnGround", _fsm.PlayerMovementController.OnGround);

        _animator.SetBool("Fall", _fsm.PlayerData.Velocity.y < -0.1f);
    }

    public void PlayJumpAnimation(){
        _animator.SetTrigger("Jump");
        if(!_animator.GetCurrentAnimatorStateInfo(0).IsName("JumpToAir")){
            _animator.Play("JumpToAir", 0);
        }
    }

    public void PlayPushAnimation(){
        _animator.SetTrigger("Push");
        if(!_animator.GetCurrentAnimatorStateInfo(0).IsName("Push")){
            _animator.Play("Push", 0);
        }
    }
}