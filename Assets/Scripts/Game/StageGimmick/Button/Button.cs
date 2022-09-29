using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボタン制御
/// </summary>
public class Button : BaseStageGimmick {
    [SerializeField] private float _colorDuration; //色点滅時間
    [SerializeField] private bool _stateLock;   //一回起動した後状態を維持する
    [SerializeField] private LayerMask _searchLayer = default; //反応可能のレイヤー

    private Vector3 _position;
    private MeshRenderer _meshRenderer;

    void Awake()
    {
        TryGetComponent(out _meshRenderer);

        _position = transform.position;
    }

    private void Update() {
        SetMaterialColor();
    }

    /// <summary>
    /// ボタンの上の物体チェック
    /// </summary>
    /// <returns></returns>
    bool TopCheck()
    {
        var colliders = Physics.OverlapBox(transform.position + transform.up * 0.3f, new Vector3(0.2f, 0.4f, 0.2f), Quaternion.identity, _searchLayer);
        if (colliders.Length > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    private void SetMaterialColor(){
        float lerp = Mathf.PingPong(Time.time, _colorDuration) / _colorDuration;
        if(IsOpen){
            _meshRenderer.material.SetColor("_EmissionColor", Color.Lerp(Color.blue, new Color(0.5f, 0.5f, 0.5f, 1f), lerp));
        }else{
            _meshRenderer.material.SetColor("_EmissionColor", Color.Lerp(Color.green, new Color(0.5f, 0.5f, 0.5f, 1f), lerp));
        }
    }

#region 接触処理
    private void OnTriggerStay(Collider other) {
        if(_isOpen == false && TopCheck()){
            _isOpen = true;
            transform.position = _position - new Vector3(0, 0.02f, 0);
            EventCenter.ButtonNotify(ID, IsOpen);
            AudioManager.Instance.Play("Button", "ButtonClick", false);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(_isOpen == true && TopCheck() == false){
            _isOpen = false;
            transform.position = _position;
            EventCenter.ButtonNotify(ID, IsOpen);
            AudioManager.Instance.Play("Button", "ButtonClick", false);
        }
    }
#endregion

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.up * 0.3f, new Vector3(0.2f, 0.4f, 0.2f));
    }
}