using UnityEngine;

/// <summary>
/// UI 入力提示
/// </summary>
public class UIGameInputHint_UIControl : UIControl {
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out _animator);
    }

    private void Update() {
        _animator.SetBool("Enable" ,GameManager.Pause);
    }
}