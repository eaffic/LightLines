using UnityEngine;

/// <summary>
/// 移動制御クラス
/// </summary>
public class PlayerMovement : MonoBehaviour {
    private PlayerFSM _fsm;
    private Rigidbody _rigidBody;
    private PlayerStateType _currentStateType;

    //キャラ基本情報
    [SerializeField] private float _maxSpeed; //最大速度
    [SerializeField] private float _acceleration; //加速度
    [SerializeField] private float _turnSmoothVelocity; //転向速度
    [SerializeField] private Vector3 _velocity;  //現在速度
    [SerializeField] private Vector3 _desiredVelocity;  //予期速度
    [SerializeField] private Vector3 _contactNormal; //接触地面の法線合計

    [SerializeField] private int _groundContactCount; //接触した地面の数

    [SerializeField] private bool _canUpdate; //更新確認
    public bool OnJump { get; set; } //ジャンプ状態
    public bool OnGround => _groundContactCount > 0;  //地面確認
    public bool OnSlope //坂道確認
    {
        get
        {
            return Vector3.Angle(_contactNormal, Vector3.up) > 0 &&
                   Vector3.Angle(_contactNormal, Vector3.up) <= _fsm.PlayerData.MaxGroundAngle;
        }
    }

    private void Awake() {
        TryGetComponent(out _rigidBody);
        TryGetComponent(out _fsm);
    }

    private void Update() {
        if (_canUpdate == false) { return; }

        InputCheck();
    }

    private void FixedUpdate() {
        _fsm.PlayerData.Velocity = _rigidBody.velocity;
        if (_canUpdate == false) { return; }

        _velocity = _rigidBody.velocity;

        SnapToGround();
        CheckGravity();
        AdjustVelocity();
        ClearState();

        _rigidBody.velocity = _velocity;
    }

    /// <summary>
    /// 入力チェック
    /// </summary>
    private void InputCheck()
    {
        Vector3 moveVector = InputManager.Instance.GetPlayerMoveInput();
        
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
        if (new Vector3(_velocity.x, 0f, _velocity.z).magnitude > 0.01f)
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
        if(_fsm.GetCurrentStateType() == PlayerStateType.Jump)
        {
            return false;
        }
        //速度は地面検知速度以内のこと
        float speed = _velocity.magnitude;
        if (speed > _fsm.PlayerData.MaxSnapSpeed)
        {
            return false;
        }
        //足元は地面コライダー
        if (!Physics.Raycast(_rigidBody.position, Vector3.down, out RaycastHit hit, _fsm.PlayerData.ProbeDistance, _fsm.PlayerData.ProbeMask))
        {
            return false;
        }
        //足元のコライダーの角度確認(移動不可の斜面ではない)
        if (hit.normal.y < GetMinDot(hit.collider.gameObject.layer))
        {
            return false;
        }

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
    }

    private void OnCollisionStay(Collision other)
    {
        EvaluateCollision(other);
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
        if (OnSlope && OnGround)
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
    /// キャラ基本データ設定
    /// </summary>
    /// <param name="data"></param>
    public void SetData(CharacterData_SO data)
    {
        _fsm.PlayerData = data;
    }

    /// <summary>
    /// キャラ状態設定
    /// </summary>
    public void SetCurrentState(PlayerStateType type){
        switch (type){
            case PlayerStateType.Walk:
                _canUpdate = true;
                _acceleration = _fsm.PlayerData.WalkAcceleration;
                _maxSpeed = _fsm.PlayerData.MaxWalkSpeed;
                break;
            case PlayerStateType.Run:
                _canUpdate = true;
                _acceleration = _fsm.PlayerData.RunAcceleration;
                _maxSpeed = _fsm.PlayerData.MaxRunSpeed;
                break;
            case PlayerStateType.Jump:
                _canUpdate = true;
                _acceleration = _fsm.PlayerData.AirAcceleration;
                OnJump = true;
                Jump();
                break;
            default:
                _canUpdate = false;
                break;
        }
    }

    public void ResetMoveSpeed(){
        _rigidBody.velocity = Vector3.zero;
    }
}