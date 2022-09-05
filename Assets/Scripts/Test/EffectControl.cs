using UnityEngine;

public class EffectControl : UnitySingleton<EffectControl> {
    [Tooltip("Player")]
    [SerializeField] private GameObject[] _player;
    [SerializeField] private float _intoDissolveHeight;
    [SerializeField] private float _exitDissolveHeight;

    [Tooltip("Ground")]
    [SerializeField] private GameObject[] _ground;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start() {
        
    }

    private void Update() {
        
    }

    
}