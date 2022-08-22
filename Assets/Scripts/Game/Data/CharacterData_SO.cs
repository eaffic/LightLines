using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData_SO", menuName = "LightLines/CharacterData_SO", order = 0)]
public class CharacterData_SO : ScriptableObject {
    [Tooltip("プレイヤー入力空間"), SerializeField] 
    private Transform _playerInputSpace = default;
    public Transform PlayerInputSpace{
        get { return _playerInputSpace; }
        set { _playerInputSpace = value; }
    }

    [Header("Direction")]
    [Tooltip("キャラ向きスムーズタイム"), SerializeField, Min(0f)]
    private float _turnSmoothTime = 0.1f;
    public float TurnSmoothTime => _turnSmoothTime;

    [Header("MaxSpeed")]
    [Tooltip("最大地面歩行速度"), SerializeField, Range(0f, 100f)]
    private float _maxWalkSpeed = 5f;
    public float MaxWalkSpeed => _maxWalkSpeed;
    [Tooltip("最大地面走る速度"), SerializeField, Range(0f, 100f)]
    private float _maxRunSpeed = 10f;
    public float MaxRunSpeed => _maxRunSpeed;
    [Tooltip("最大空中移動速度"), SerializeField, Range(0f,100f)]
    private float _maxAirSpeed = 3f;
    public float MaxAirSpeed => _maxAirSpeed;

    [Tooltip("最大地面捕獲速度"), SerializeField, Range(0f, 100f)]
    private float _maxSnapSpeed = 100f;
    public float MaxSnapSpeed => _maxSnapSpeed;

    [Header("Accleration")]
    [Tooltip("歩行加速度"), SerializeField, Range(0f, 100f)]
    private float _walkAcceleration = 2f;
    public float WalkAcceleration => _walkAcceleration;
    [Tooltip("走る加速度"), SerializeField, Range(0f, 100f)]
    private float _runAcceleration = 10f;
    public float RunAcceleration => _runAcceleration;
    [Tooltip("空中加速度"), SerializeField, Range(0f, 100f)]
    private float _airAcceleration = 1f;
    public float AirAcceleration => _airAcceleration;
    [Tooltip("坂道加速度"), SerializeField, Range(0f, 100f)]
    private float _slopeAcceleration = 15f;
    public float SlopeAcceleration => _slopeAcceleration;

    [Header("Jump")]
    [Tooltip("最大ジャンプ高さ"), SerializeField, Range(0f, 10f)]
    private float _jumpHeight = 2f;
    public float JumpHeight => _jumpHeight;
    [Tooltip("最大空中ジャンプ数(地面ジャンプ除く)"), SerializeField, Range(0, 5)]
    private int _maxAirJumps = 0;
    public int MaxAirJumps => _maxAirJumps;

    [Header("GroundCheck")]
    [Tooltip("地面検知距離"), SerializeField, Min(0f)]
    private float _probeDistance = 1f;
    public float ProbeDistance => _probeDistance;
    [Tooltip("最大斜面通行角度"), SerializeField, Range(0f, 90f)]
    private float _maxGroundAngle = 45f;
    public float MaxGroundAngle => _maxGroundAngle;
    [Tooltip("地面Layer"), SerializeField]
    private LayerMask _probeMask = -1;
    public LayerMask ProbeMask => _probeMask;
}