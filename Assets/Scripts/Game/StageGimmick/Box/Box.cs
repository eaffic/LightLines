using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 箱制御
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Box : MonoBehaviour
{
    [Tooltip("移動時間"), SerializeField] private float _moveTime = 0.3f;
    [Tooltip("障害物Layer"), SerializeField] private LayerMask _wallLayer = default;
    [Tooltip("地面Layer"), SerializeField] private LayerMask _groundLayer = default;
    [SerializeField] private float _colorDuration = 2f;

    [SerializeField] public bool IsPlayerPushTarget; //プレイヤーの動作目標
    [SerializeField] public bool IsSecretItemInBox; //箱の中にアイテムの存在
    [SerializeField] public bool IsContactAccelerator; //加速器

    private Rigidbody _rigidBody;
    private Renderer _renderer;
    private float _timer;
    private int _contactCollider;

    public bool OnMove; //移動中
    public bool OnRotate; //回転中
    public bool OnGround => Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, 0.2f, _groundLayer);   //地面確認

    void Awake()
    {
        TryGetComponent(out _rigidBody);
        TryGetComponent(out _renderer);
    }
    
    void Update()
    {
        SetMaterialColor();
        StateCheck();

        Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), Vector3.down * 0.15f, OnGround ? Color.red : Color.green);
    }

    /// <summary>
    /// 箱状態確認、重力設定する
    /// </summary>
    private void StateCheck()
    {
        if (OnGround == false)
        {
            _rigidBody.useGravity = true;
        }

        if (IsContactAccelerator)
        {
            _rigidBody.useGravity = false;
        }
        else
        {
            _rigidBody.useGravity = true;
        }

        if (transform.position.y < -100){
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 点滅
    /// </summary>
    private void SetMaterialColor()
    {
        float lerp = Mathf.PingPong(Time.time, _colorDuration) / _colorDuration;

        if (IsPlayerPushTarget)
        {
            _renderer.material.SetColor("_Color", Color.Lerp(Color.red, new Color(0.5f, 0.5f, 0.5f, 1), lerp));
        }
        else if (IsSecretItemInBox)
        {
            _renderer.material.SetColor("_Color", Color.Lerp(Color.yellow, new Color(0.5f, 0.5f, 0.5f, 1), lerp));
        }
        else if (IsContactAccelerator)
        {
            _renderer.material.SetColor("_Color", Color.Lerp(Color.green, new Color(0.5f, 0.5f, 0.5f, 1), lerp));
        }
        else
        {
            _renderer.material.SetColor("_Color", Color.Lerp(Color.white, new Color(0.5f, 0.5f, 0.5f, 1), lerp));
        }
    }

    /// <summary>
    /// 箱移動
    /// </summary>
    /// <param name="direction">移動距離</param>
    /// <returns></returns>
    public bool MoveBox(Vector3 direction)
    {
        if (OnMove || OnRotate) { return false; }

        if(MoveChecked(direction))
        {
            StartCoroutine(BoxMove(transform.position + direction));
            return true;
        }

        return false;
    }

    /// <summary>
    /// 箱回転
    /// </summary>
    /// <param name="angle">回転角度</param>
    public void RotateBox(float angle)
    {
        if (OnMove || OnRotate) { return; }

        StartCoroutine(BoxRotate(angle));
    }

    /// <summary>
    /// 移動先チェック
    /// </summary>
    /// <param name="direction">移動距離</param>
    /// <returns></returns>
    public bool MoveChecked(Vector3 direction)
    {
        Vector3 center = transform.position + new Vector3(0, 0.5f, 0);

        //if(OnHead){ return false; }
        //Debug.Log(!Physics.Raycast(center, direction, out RaycastHit hitInfso, direction.magnitude, _wallLayer));
        //Debug.DrawRay(center, direction, Color.red, 10f);
        return !Physics.Raycast(center, direction, out RaycastHit hitInfo, direction.magnitude, _wallLayer);
    }   

    /// <summary>
    /// 箱移動
    /// </summary>
    /// <param name="targetOffset">移動距離</param>
    /// <returns></returns>
    IEnumerator BoxMove(Vector3 targetOffset)
    {
        //while(_onMove || _onRotate) { yield return null; }

        _rigidBody.isKinematic = true;
        OnMove = true;
        AudioManager.Instance.Play("Box", "BoxSlide", false);

        //移動
        Vector3 velocity = Vector3.zero;
        float distance = new Vector3(transform.position.x - targetOffset.x, transform.position.y - targetOffset.y, transform.position.z - targetOffset.z).magnitude;
        while (distance > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetOffset, ref velocity, _moveTime);
            distance = new Vector3(transform.position.x - targetOffset.x, transform.position.y - targetOffset.y, transform.position.z - targetOffset.z).magnitude;
            yield return null;
        }

        transform.position = targetOffset;
        OnMove = false;
        _rigidBody.isKinematic = false;
        yield return null;
    }

    /// <summary>
    /// 箱回転
    /// </summary>
    /// <param name="angle">回転角度</param>
    /// <returns></returns>
    IEnumerator BoxRotate(float angle){
        while(OnMove || OnRotate) { yield return null; }

        _rigidBody.isKinematic = true;
        OnRotate = true;

        float targetAngle = transform.eulerAngles.y + angle;
        while(Mathf.Abs(transform.eulerAngles.y - targetAngle) > 0.5f){
            float tmp = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * 3.5f);
            transform.eulerAngles = new Vector3(0, tmp, 0);
            yield return null;
        }

        transform.eulerAngles = new Vector3(0, targetAngle, 0);
        OnRotate = false;
        _rigidBody.isKinematic = false;
        yield return null;
    }

    #region  接触処理
    private void OnCollisionEnter(Collision other)
    {
        //if(IsContactBeltConveyor) { return; }

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, out hitInfo, 0.2f, _groundLayer)){
            transform.position = hitInfo.point;
            _rigidBody.useGravity = false;
        }
    }
    #endregion
}
