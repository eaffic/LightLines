using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Button : BaseStageGimmick {
    [SerializeField] private float _colorDuration;
    [SerializeField] private bool _stateLock;   //起動状態を維持する
    [SerializeField] private LayerMask _searchLayer = default;

    private Vector3 _position;
    private Light _light;
    private MeshRenderer _meshRenderer;

    void Awake()
    {
        _light = GetComponentInChildren<Light>();
        TryGetComponent(out _meshRenderer);

        _position = transform.position;
    }

    private void Update() {
        SetEmissionColor();
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

    private void SetEmissionColor(){
        float lerp = Mathf.PingPong(Time.time, _colorDuration) / _colorDuration;
        if(IsOpen){
            _meshRenderer.material.SetColor("_EmissionColor", Color.Lerp(Color.blue, new Color(0.5f, 0.5f, 0.5f, 1f), lerp));
        }else{
            _meshRenderer.material.SetColor("_EmissionColor", Color.Lerp(Color.green, new Color(0.5f, 0.5f, 0.5f, 1f), lerp));
        }
    }

    private void OnTriggerStay(Collider other) {
        if(_isOpen == false && TopCheck()){
            _isOpen = true;
            transform.position = _position - new Vector3(0, 0.02f, 0);
            EventCenter.ButtonNotify(Number, IsOpen);
            AudioManager.Instance.Play("Button", "ButtonClick", false);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(_isOpen == true && TopCheck() == false){
            _isOpen = false;
            transform.position = _position;
            EventCenter.ButtonNotify(Number, IsOpen);
            AudioManager.Instance.Play("Button", "ButtonClick", false);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.up * 0.3f, new Vector3(0.2f, 0.4f, 0.2f));
    }
}