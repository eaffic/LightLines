using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動以外のアクション制御
/// </summary>
public class PlayerActions : MonoBehaviour { 
    [SerializeField] private GameObject _pushInputHintPanel;

    [Tooltip("目の高さ"), SerializeField] 
    private float _eyeHeight = 0.7f;
    [Tooltip("前方レイの長さ(箱判定用)"), SerializeField] 
    private float _forwardRayLength = 0.5f;

    private GameObject _targetBox;
    private bool _inBoxPushArea;    //箱のプッシュ可能範囲との接触
    private Vector3 _pushPoint;

    private void Awake() {
        _pushInputHintPanel.SetActive(false);
    }

    private void Update() {
        SearchBox();
        if(_pushInputHintPanel.activeSelf){
        }
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
        }
    }
    #endregion

    /// <summary>
    /// 前方の箱を探す
    /// </summary>
    /// <returns></returns>
    public bool SearchBox(){
        if (_inBoxPushArea == false) { return false; } //指定位置に入らないと確認しない

        //目先の確認レイ
        Ray ray = new Ray(transform.position + new Vector3(0f, _eyeHeight, 0f), transform.forward);
        
        //if(前方の箱は存在しているか){
        //  if(箱の記録あり && 見た箱と記録の箱は違う)
        //    記録の箱を更新する
        //  else if(箱の記録なし)
        //    箱を記録する 
        //}
        //else if(箱の記録あり){
        //  箱の記録を消す
        //}
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo, _forwardRayLength, LayerMask.GetMask("Box")))
        {
            //前フレームと別の箱をハイライトしたとき、リセットする
            if (_targetBox && hitInfo.collider.gameObject != _targetBox)
            {
                _targetBox.GetComponent<Box>().IsPlayerPushTarget = false;
                _targetBox = null;
                _pushInputHintPanel.SetActive(false);

                _targetBox = hitInfo.collider.gameObject;
                _targetBox.GetComponent<Box>().IsPlayerPushTarget = true;
                _pushInputHintPanel.SetActive(CheckTargetBox()); //入力提示のため、箱の移動可能状態確認
            }
            else if(_targetBox == null)
            {
                //前フレームはハイライトの箱は存在していないとき
                _targetBox = hitInfo.collider.gameObject;
                _targetBox.GetComponent<Box>().IsPlayerPushTarget = true;
                _pushInputHintPanel.SetActive(CheckTargetBox()); //入力提示のため、箱の移動可能状態確認
            }
            return true;
        }
        else if (_targetBox)
        {
            _targetBox.GetComponent<Box>().IsPlayerPushTarget = false;
            _targetBox = null;
            _pushInputHintPanel.SetActive(false);
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
        if (_targetBox.GetComponent<Box>().IsContactAccelerator) { return false; }

        bool check = false;
        float angle = 0f;
        //プレイヤーの向きから箱のプッシュ方向を決める
        //箱の移動可能を確認
        Vector2 distance = new Vector2(transform.position.x - _targetBox.transform.position.x, transform.position.z - _targetBox.transform.position.z);
        if (distance.y < -0.5f)
        {
            check = _targetBox.GetComponent<Box>().MoveChecked(Vector3.forward);
            angle = 0f;
        }
        else if (distance.y > 0.5f)
        {
            check = _targetBox.GetComponent<Box>().MoveChecked(Vector3.back);
            angle = 180f;
        }
        else if (distance.x < -0.5f)
        {
            check = _targetBox.GetComponent<Box>().MoveChecked(Vector3.right);
            angle = 90f;
        }
        else if (distance.x > 0.5f)
        {
            check = _targetBox.GetComponent<Box>().MoveChecked(Vector3.left);
            angle = 270f;
        }

        if (check){
            //transform.position = new Vector3(_pushPoint.x, transform.position.y, _pushPoint.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
            //_pushPoint = Vector3.zero;
        }

        return check;
    }

    //アニメーションのタイミングと合わせて呼び出す
    public void PushTargetBox()
    {
        AudioManager.Instance.Play("Player", "PlayerPush", false);

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

        _targetBox.GetComponent<Box>().IsPlayerPushTarget = false;
        _targetBox = null;
    }
}