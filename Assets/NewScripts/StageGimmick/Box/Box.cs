using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 箱
/// </summary>
public class Box : MonoBehaviour
{
    [Tooltip("移動時間"), SerializeField] float _moveTime = 0.3f;
    [Tooltip("障害物Layer"), SerializeField] LayerMask _wallLayer = default;
    [Tooltip("地面Layer"), SerializeField] LayerMask _groundLayer = default;

    //優先順 プッシュ > アイテム > 通常
    [Tooltip("通常マテリアル"), SerializeField] Material _normalMaterial = default;
    [Tooltip("プッシュ目標マテリアル"), SerializeField] Material _pushTargetMaterial = default;
    [Tooltip("隠しアイテム持ちマテリアル"), SerializeField] Material _ownSecretItemMaterial = default;

    public bool IsPlayerPushTarget { get; set; }
    public bool IsSecretItemInBox { get; set; }

    Rigidbody _rigidBody;
    Renderer _renderer;
    
    bool OnMove { get; set; } //移動中
    bool OnRotate { get; set; } //回転中
    bool OnHead => Physics.Raycast(transform.position, Vector3.up, 0.5f, _wallLayer);   //上方向確認
    bool OnGround => Physics.Raycast(transform.position + new Vector3 (0, 0.1f, 0), Vector3.down, 0.2f, _groundLayer);   //地面確認

    void Awake()
    {
        TryGetComponent(out _rigidBody);
        TryGetComponent(out _renderer);
    }
    
    void Update()
    {
        //SetMaterial();

        Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), Vector3.down * 0.2f, OnGround ? Color.red : Color.green);
    }

    void SetMaterial()
    {
        if (IsPlayerPushTarget)
        {
            _renderer.material = _pushTargetMaterial;
        }else if (IsSecretItemInBox)
        {
            _renderer.material = _ownSecretItemMaterial;
        }
        else
        {
            _renderer.material = _normalMaterial;
        }
    }

    
    public void MoveBox(Vector3 direction)
    {
        if (OnMove || OnRotate) { return; }

        if(MoveChecked(direction))
        {
            StartCoroutine(BoxMove(transform.position + direction));
        }
    }

    //回転
    public void RotateBox(float angle)
    {
        if (OnMove || OnRotate) { return; }

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
        //while(OnMove || OnRotate) { yield return null; }

        _rigidBody.useGravity = false;
        OnMove = true;
        //AudioManager.PlayBoxMoveAudio();

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
        OnMove = false;
        _rigidBody.useGravity = true;
        yield return null;
    }

    //箱回転
    IEnumerator BoxRotate(float angle){
        //while(OnMove || OnRotate) { yield return null; }

        //_rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        OnRotate = true;

        float targetAngle = transform.eulerAngles.y + angle;
        while(Mathf.Abs(transform.eulerAngles.y - targetAngle) > 0.5f){
            float tmp = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * 2);
            transform.eulerAngles = new Vector3(0, tmp, 0);
            yield return null;
        }

        transform.eulerAngles = new Vector3(0, targetAngle, 0);
        OnRotate = false;
        //_rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        yield return null;
    }
}
