using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動以外のアクション制御
/// </summary>
public class PlayerActions : MonoBehaviour {
    private PlayerFSM _fsm;
    [SerializeField]
    private GameObject _targetBox;

    [Tooltip("目の高さ"), SerializeField] 
    private float _eyeHeight = 0.7f;
    [Tooltip("前方レイの長さ(箱判定用)"), SerializeField] 
    private float _forwardRayLength = 0.5f;
    
    private bool _inBoxPushArea;    //箱のプッシュ可能範囲との接触
    private Vector3 _pushPoint;


    private void Awake() {
        TryGetComponent(out _fsm);
    }

    #region 接触コライダー更新
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "BoxPushArea")
        {
            _inBoxPushArea = true;
            _pushPoint = other.transform.position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BoxPushArea")
        {
            _inBoxPushArea = false;
            _pushPoint = Vector3.zero;
        }
    }
    #endregion

    public bool SearchBox(){
        if (_inBoxPushArea == false) { return false; } //指定位置に入らないと確認しない

        //目先の確認レイ
        Ray ray = new Ray(transform.position + new Vector3(0f, _eyeHeight, 0f), transform.forward);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo, _forwardRayLength, LayerMask.GetMask("Box")))
        {
            //前フレームと別の箱をハイライトしたとき、リセットする
            if (_targetBox && hitInfo.collider.gameObject != _targetBox)
            {
                _targetBox.GetComponent<Box>().IsPlayerPushTarget = false;
                _targetBox = null;
                _targetBox = hitInfo.collider.gameObject;
                _targetBox.GetComponent<Box>().IsPlayerPushTarget = true;
            }
            else
            {
                //前フレームはハイライトの箱は存在していないとき
                _targetBox = hitInfo.collider.gameObject;
                _targetBox.GetComponent<Box>().IsPlayerPushTarget = true;
            }
            return true;
        }
        else if (_targetBox)
        {
            _targetBox.GetComponent<Box>().IsPlayerPushTarget = false;
            _targetBox = null;
        }
        return false;
    }

    /// <summary>
    /// 目標箱の移動可能確認
    /// </summary>
    /// <returns></returns>
    public bool CheckTargetBox()
    {
        if (_targetBox == null) { return false; }

        bool check = false;
        //プレイヤーの向きから箱のプッシュ方向を決める
        //箱の移動可能を確認
        Vector2 distance = new Vector2(transform.position.x - _targetBox.transform.position.x, transform.position.z - _targetBox.transform.position.z);
        if (distance.y < -0.5f)
        {
            check = _targetBox.GetComponent<Box>().MoveChecked(Vector3.forward);
        }
        else if (distance.y > 0.5f)
        {
            check = _targetBox.GetComponent<Box>().MoveChecked(Vector3.back);
        }
        else if (distance.x < -0.5f)
        {
            check = _targetBox.GetComponent<Box>().MoveChecked(Vector3.right);
        }
        else if (distance.x > 0.5f)
        {
            check = _targetBox.GetComponent<Box>().MoveChecked(Vector3.left);
        }

        return check;
    }

    //アニメーションのタイミングと合わせて呼び出す
    public void PushTargetBox()
    {
        transform.position = new Vector3(_pushPoint.x, transform.position.y, _pushPoint.z);

        Vector2 distance = new Vector2(transform.position.x - _targetBox.transform.position.x, transform.position.z - _targetBox.transform.position.z);
        if (distance.y < -0.5f)
        {
            _targetBox.GetComponent<Box>().MoveBox(Vector3.forward);
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (distance.y > 0.5f)
        {
            _targetBox.GetComponent<Box>().MoveBox(Vector3.back);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (distance.x < -0.5f)
        {
            _targetBox.GetComponent<Box>().MoveBox(Vector3.right);
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (distance.x > 0.5f)
        {
            _targetBox.GetComponent<Box>().MoveBox(Vector3.left);
            transform.eulerAngles = new Vector3(0, -90, 0);
        }
    }
}