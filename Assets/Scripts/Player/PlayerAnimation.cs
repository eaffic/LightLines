using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    private Animator _animator;

    private void Awake() {
        TryGetComponent(out _animator);
    }

    
}