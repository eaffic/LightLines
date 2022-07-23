using UnityEngine;

[CreateAssetMenu(fileName = "Character_SO", menuName = "LightLines/Character_SO", order = 0)]
public class CharacterData_SO : ScriptableObject {
    [Tooltip("プレイヤー入力空間"), SerializeField] 
    Transform _playerInputSpace = default;
    public Transform PlayerInputSpace{
        get { return _playerInputSpace; }
        set { _playerInputSpace = value; }
    }

    [Header("Direction")]
    [Tooltip("キャラ向きスムーズタイム"), SerializeField, Min(0f)]
    float _turnSmoothTime = 0.1f;
    public float TurnSmoothTime
    {
        get { return _turnSmoothTime; }
    }

    [Header("MaxSpeed")]
    [Tooltip("最大地面歩行速度"), SerializeField, Range(0f, 100f)] 
    float _maxWalkSpeed = 5f;
    [Tooltip("最大地面走る速度"), SerializeField, Range(0f, 100f)] 
    float _maxRunSpeed = 10f;
    [Tooltip("最大地面捕獲速度"), SerializeField, Range(0f, 100f)] 
    float _maxSnapSpeed = 100f;
    public float MaxWalkSpeed{
        get { return _maxWalkSpeed; }
    }
    public float MaxRunSpeed{
        get { return _maxRunSpeed; }
    }
    public float MaxSnapSpeed{
        get { return _maxSnapSpeed; }
    }

    [Header("Accleration")]
    [Tooltip("歩行加速度"), SerializeField, Range(0f, 100f)]
    float _maxWalkAccleration = 2f;
    [Tooltip("走る加速度"), SerializeField, Range(0f, 100f)]
    float _maxRunAccleration = 10f;
    [Tooltip("空中加速度"), SerializeField, Range(0f, 100f)]
    float _maxAirAccleration = 1f;
    public float MaxWalkAccleration{
        get { return _maxWalkAccleration; }
    }
    public float MaxRunAccleration{
        get { return _maxRunAccleration; }
    }
    public float MaxAirAccleration{
        get { return _maxAirAccleration; }
    }


    [Header("Jump")]
    [Tooltip("最大ジャンプ高さ"), SerializeField, Range(0f, 10f)]
    float _jumpHeight = 2f;
    [Tooltip("最大空中ジャンプ数(地面ジャンプ除く)"), SerializeField, Range(0, 5)]
    int _maxAirJumps = 0;
    public float JumpHeight{
        get { return _jumpHeight; }
    }
    public int MaxAirJumps{
        get { return _maxAirJumps; }
    }

    [Header("Check")]
    [Tooltip("地面検知距離"), SerializeField, Min(0f)] 
    float _probeDistance = 1f;
    [Tooltip("最大斜面通行角度"), SerializeField, Range(0f, 90f)]
    float _maxGroundAngle = 45f;
    [Tooltip("地面Layer"), SerializeField]
    LayerMask _probeMask = -1;
    public float ProbeDistance{
        get { return _probeDistance; }
    }
    public float MaxGroundAngle{
        get { return _maxGroundAngle; }
    }
    public LayerMask ProbeMask{
        get { return _probeMask; }
    }



}