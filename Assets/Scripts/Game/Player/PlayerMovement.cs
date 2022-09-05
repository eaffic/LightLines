using UnityEngine;
using GameEnumList;

/// <summary>
/// 移動制御クラス
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private PlayerFSM _fsm;
    [SerializeField] private Rigidbody _rigidBody;

    [SerializeField] private GameObject _rippleParticle;

    private Vector3 _lastTouchWallPosition;

    //キャラ基本情報
    private float _maxSpeed; //最大速度
    private float _acceleration; //加速度
    private float _turnSmoothVelocity; //転向速度
    private Vector3 _velocity;  //現在速度
    private Vector3 _desiredVelocity;  //予期速度
    private Vector3 _contactNormal; //接触地面の法線合計
    private int _groundContactCount; //接触した地面の数

    public Vector3 Velocity { get => _velocity; }
    public bool OnAir { get; set; } //ジャンプ状態
    public bool OnGround => _groundContactCount > 0 || SnapToGround();  //地面確認
    public bool OnSlope //坂道確認
    {
        get
        {
            return Vector3.Angle(_contactNormal, Vector3.up) > 0 &&
                   Vector3.Angle(_contactNormal, Vector3.up) <= _fsm.PlayerData.MaxGroundAngle;
        }
    }

    private void Awake()
    {
        TryGetComponent(out _rigidBody);
        TryGetComponent(out _fsm);

        _rippleParticle.transform.parent = null;
    }

    private void Update()
    {
        switch (_fsm.CurrentState)
        {
            case PlayerState.Walk:
            case PlayerState.Run:
            case PlayerState.Jump:
            case PlayerState.Fall:
                InputCheck();
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log(OnGround);
        _velocity = _rigidBody.velocity;
        switch (_fsm.CurrentState)
        {
            case PlayerState.Idle:
            case PlayerState.Walk:
            case PlayerState.Run:
            case PlayerState.Jump:
            case PlayerState.Fall:
                SnapToGround();
                Fall();
                AdjustVelocity();
                ClearState();
                break;
            default:
                break;
        }
        _rigidBody.velocity = _velocity;
        //_fsm.PlayerData.Velocity = _rigidBody.velocity;
        //CheckGravity();
    }

    /// <summary>
    /// 入力チェック
    /// </summary>
    private void InputCheck()
    {
        Vector3 moveVector = GameInputManager.Instance.GetPlayerMoveInput();

        //カメラ方向を基に進行方向を調整する
        if (_fsm.PlayerData.PlayerInputSpace)
        {
            Vector3 forward = _fsm.PlayerData.PlayerInputSpace.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 right = _fsm.PlayerData.PlayerInputSpace.right;
            right.y = 0;
            right.Normalize();
            _desiredVelocity = (right * moveVector.x + forward * moveVector.z) * _maxSpeed; //希望速度
        }
        else
        {
            _desiredVelocity = new Vector3(moveVector.x, 0f, moveVector.z) * _maxSpeed; //希望速度
        }
    }

    /// <summary>
    /// 速度方向修正
    /// </summary>
    private void AdjustVelocity()
    {
        //地面のxz軸の方向
        Vector3 xAxis = DirectionOnConnectPlane(Vector3.right).normalized;
        Vector3 zAxis = DirectionOnConnectPlane(Vector3.forward).normalized;

        //現在地面方向のxz軸の速度(斜面の上なら減るはず)
        float currentX = Vector3.Dot(_velocity, xAxis);
        float currentZ = Vector3.Dot(_velocity, zAxis);

        //加速度率(坂道の加速度は別設定)
        float maxSpeedChange = (OnSlope ? _fsm.PlayerData.SlopeAcceleration : _acceleration) * Time.fixedDeltaTime;

        //新しい速度
        float newX = Mathf.MoveTowards(currentX, _desiredVelocity.x, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, _desiredVelocity.z, maxSpeedChange);

        //速度更新(新旧速度の差から方向を調整する)
        _velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);

        //速度から向き調整
        if (new Vector3(_velocity.x, 0f, _velocity.z).magnitude > 0.05f)
        {
            float turnAngle = Mathf.Atan2(_velocity.x, _velocity.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, turnAngle, ref _turnSmoothVelocity, _fsm.PlayerData.TurnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    /// <summary>
    /// 地面接触判断(一瞬地面で離しても戻れるように)
    /// </summary>
    /// <returns>true OnGroundと同じ意味/false 空中移動</returns>
    private bool SnapToGround()
    {
        //ジャンプ中
        if (OnAir)
        {
            return false;
        }
        //速度は地面検知速度以内のこと
        float speed = _velocity.magnitude;
        if (speed > _fsm.PlayerData.MaxSnapSpeed)
        {
            return false;
        }
        //足元は地面コライダー(一定の距離を離れても戻れるため、地面探索距離を設定する)
        if (!Physics.Raycast(_rigidBody.position, Vector3.down, out RaycastHit hit, _fsm.PlayerData.ProbeDistance, _fsm.PlayerData.ProbeMask))
        {
            return false;
        }
        //足元のコライダーの角度確認(移動不可の斜面ではない)
        if (hit.normal.y < GetMinDot(hit.collider.gameObject.layer))
        {
            return false;
        }

        //坂道ではない場合、キャラの位置は射線と地面の接触位置に移動する
        if (OnSlope == false) { transform.position = hit.point; }

        //現在速度は地面法線方向の大きさ
        float dot = Vector3.Dot(_velocity, hit.normal);
        //この物体は地面から離れている時、速度を修正する
        if (dot > 0)
        {
            _velocity = (_velocity - hit.normal * dot).normalized * speed;
        }
        return true;
    }

    /// <summary>
    /// 階段を自動に登る、降りる
    /// </summary>
    private void CheckStep(){
        //TODO
    }

    /// <summary>
    /// 重力判定
    /// </summary>
    private void Fall(){
        if(OnGround == false){
            _velocity += Physics.gravity * Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    private void Jump()
    {
        Vector3 jumpDirection = Vector3.up;
        Vector3 _velocity = _rigidBody.velocity;

        //ジャンプ初期速度は設定した高さから計算する v = √-2gh
        float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * _fsm.PlayerData.JumpHeight);
        float alignedSpeed = Vector3.Dot(_velocity, jumpDirection); //現在速度のジャンプ方向の長さ（大きさ）
        //現在速度とジャンプ速度の調整
        if (alignedSpeed > 0f)
        {
            //ジャンプ速度をマイナスにならないように制限する
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
        }

        _velocity += jumpDirection * jumpSpeed; //ジャンプ速度を追加する
        _rigidBody.velocity = _velocity;
    }

    #region 接触コライダー更新
    private void OnCollisionEnter(Collision other)
    {
        EvaluateCollision(other);

        // 壁の接触エフェクト位置、角度更新
        if (other.gameObject.tag == "Wall")
        {
            Vector3 player = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 contact = new Vector3(other.collider.ClosestPoint(player).x, 0, other.collider.ClosestPoint(player).z);

            float angle = Vector3.Angle(player - contact, Vector3.forward);

            _rippleParticle.transform.eulerAngles = new Vector3(0, angle, 0);
            _rippleParticle.transform.position = other.collider.ClosestPoint(player);
            _lastTouchWallPosition = other.contacts[0].point;
            _rippleParticle.SetActive(true);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        EvaluateCollision(other);

        //壁接触エフェクト位置更新  
        if (other.gameObject.tag == "Wall")
        {
            Vector3 player = new Vector3(transform.position.x, 0f, transform.position.z);
            _rippleParticle.transform.position = new Vector3(_lastTouchWallPosition.x, this.transform.position.y + 1f, _lastTouchWallPosition.z);
            _rippleParticle.SetActive(true);
            _lastTouchWallPosition = other.collider.ClosestPoint(player);
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "Wall")
        {
            _rippleParticle.SetActive(false);
        }
    }

    /// <summary>
    /// 接触コライダーの法線、地面判断など
    /// </summary>
    /// <param name="collision"></param>
    private void EvaluateCollision(Collision collision)
    {
        float minDot = GetMinDot(collision.gameObject.layer);
        for (int i = 0; i < collision.contactCount; ++i)
        {
            Vector3 normal = collision.GetContact(i).normal;
            if (normal.y >= minDot)
            {
                _groundContactCount++;
                _contactNormal += normal;
            }
            else if (normal.y > -0.01f)
            {
                //移動不可の斜面に挟まれたとき
            }
        }
    }
    #endregion 

    /// <summary>
    /// 壁の接触情報と法線をリセットする
    /// </summary>
    private void ClearState()
    {
        _groundContactCount = 0;
        _contactNormal = Vector3.zero;
    }

    /// <summary>
    /// 通過可能角度の最小内積値(最大角度)
    /// </summary>
    /// <param name="layer">障害物のレイヤ</param>
    /// <returns></returns>
    private float GetMinDot(int layer)
    {
        return Mathf.Cos(_fsm.PlayerData.MaxGroundAngle * Mathf.Deg2Rad); //斜面角度のcosθ
    }

    /// <summary>
    /// 地面方向ベクトル
    /// </summary>
    /// <param name="vector">地面方向</param>
    /// <returns></returns>
    private Vector3 DirectionOnConnectPlane(Vector3 vector)
    {
        return vector - _contactNormal * Vector3.Dot(vector, _contactNormal);
    }

    /// <summary>
    /// 重力使用設定
    /// </summary>
    private void CheckGravity()
    {
        //坂道の上に止まる
        if (OnSlope)
        {
            _rigidBody.useGravity = false;
        }
        else
        {
            _rigidBody.useGravity = true;
        }
    }

    //---------------------------------------------------------------------------
    //---------------------------------------------------------------------------
    /// <summary>
    /// キャラ状態設定
    /// </summary>
    public void SetCurrentState()
    {
        switch (_fsm.CurrentState)
        {
            case PlayerState.Walk:
                _acceleration = _fsm.PlayerData.WalkAcceleration;
                _maxSpeed = _fsm.PlayerData.MaxWalkSpeed;
                break;
            case PlayerState.Run:
                _acceleration = _fsm.PlayerData.RunAcceleration;
                _maxSpeed = _fsm.PlayerData.MaxRunSpeed;
                break;
            case PlayerState.Jump:
                _acceleration = _fsm.PlayerData.AirAcceleration;
                OnAir = true;
                Jump();
                break;
            case PlayerState.Fall:
                _acceleration = _fsm.PlayerData.AirAcceleration;
                break;
            case PlayerState.Idle:
            case PlayerState.Push:
            case PlayerState.Menu:
                ResetMoveSpeed();
                break;
            default:
                break;
        }
    }

    public void ResetMoveSpeed()
    {
        _rigidBody.velocity = Vector3.zero;
    }
}