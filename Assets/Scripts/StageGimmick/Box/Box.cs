using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 箱
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Box : MonoBehaviour
{
    [Tooltip("移動時間"), SerializeField] private float _moveTime = 0.3f;
    [Tooltip("障害物Layer"), SerializeField] private LayerMask _wallLayer = default;
    [Tooltip("地面Layer"), SerializeField] private LayerMask _groundLayer = default;
    [SerializeField] private float _duration = 2f;

    [SerializeField] public bool IsPlayerPushTarget;// { get; set; }
    [SerializeField] public bool IsSecretItemInBox;// { get; set; }

    private Rigidbody _rigidBody;
    private Renderer _renderer;
    private float timer;

    private bool _onMove; //移動中
    private bool _onRotate; //回転中
    private bool _onHead => Physics.Raycast(transform.position, Vector3.up, 0.5f, _wallLayer);   //上方向確認
    private bool _onGround => Physics.Raycast(transform.position + new Vector3 (0, 0.1f, 0), Vector3.down, 0.1f, _groundLayer);   //地面確認

    void Awake()
    {
        TryGetComponent(out _rigidBody);
        TryGetComponent(out _renderer);
    }
    
    void Update()
    {
        SetColor();
        Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), Vector3.down * 0.1f, _onGround ? Color.red : Color.green);
    }

    /// <summary>
    /// 点滅
    /// </summary>
    private void SetColor()
    {
        float lerp = Mathf.PingPong(Time.time, _duration) / _duration;

        if (IsPlayerPushTarget)
        {
            _renderer.material.SetColor("_Color", Color.Lerp(Color.red, new Color(0.5f, 0.5f, 0.5f, 1), lerp));
        }
        else if (IsSecretItemInBox)
        {
            _renderer.material.SetColor("_Color", Color.Lerp(Color.yellow, new Color(0.5f, 0.5f, 0.5f, 1), lerp));
        }
        else
        {
            _renderer.material.SetColor("_Color", Color.Lerp(Color.white, new Color(0.5f, 0.5f, 0.5f, 1), lerp));
        }
    }

    public void MoveBox(Vector3 direction)
    {
        if (_onMove || _onRotate) { return; }

        if(MoveChecked(direction))
        {
            StartCoroutine(BoxMove(transform.position + direction));
        }
    }

    //回転
    public void RotateBox(float angle)
    {
        if (_onMove || _onRotate) { return; }

        StartCoroutine(BoxRotate(angle));
    }

    //移動先チェック
    public bool MoveChecked(Vector3 direction)
    {
        Vector3 center = transform.position + new Vector3(0, 0.5f, 0);

        //if(OnHead){ return false; }
        //Debug.Log(!Physics.Raycast(center, direction, out RaycastHit hitInfso, direction.magnitude, _wallLayer));
        //Debug.DrawRay(center, direction, Color.red, 10f);
        return !Physics.Raycast(center, direction, out RaycastHit hitInfo, direction.magnitude, _wallLayer);
    }   

    //箱移動
    IEnumerator BoxMove(Vector3 movePosition)
    {
        //while(_onMove || _onRotate) { yield return null; }

        _rigidBody.isKinematic = true;
        _onMove = true;
        AudioManager.Instance.Play("Box", "BoxSlide", false);

        //移動
        Vector3 velocity = Vector3.zero;
        float distance = new Vector3(transform.position.x - movePosition.x, 0f, transform.position.z - movePosition.z).magnitude;
        while (distance > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, _moveTime);
            distance = new Vector3(transform.position.x - movePosition.x, 0f, transform.position.z - movePosition.z).magnitude;
            yield return null;
        }

        transform.position = movePosition;
        _onMove = false;
        _rigidBody.isKinematic = false;
        yield return null;
    }

    //箱回転
    IEnumerator BoxRotate(float angle){
        while(_onMove || _onRotate) { yield return null; }

        _rigidBody.isKinematic = true;
        _onRotate = true;

        float targetAngle = transform.eulerAngles.y + angle;
        while(Mathf.Abs(transform.eulerAngles.y - targetAngle) > 0.5f){
            float tmp = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * 3.5f);
            transform.eulerAngles = new Vector3(0, tmp, 0);
            yield return null;
        }

        transform.eulerAngles = new Vector3(0, targetAngle, 0);
        _onRotate = false;
        _rigidBody.isKinematic = false;
        yield return null;
    }

    private void OnCollisionEnter(Collision other)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, out hitInfo, 0.1f, _groundLayer)){
            transform.position = hitInfo.point;
        }
    }
}
