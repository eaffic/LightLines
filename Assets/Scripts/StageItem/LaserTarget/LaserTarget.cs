using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザー目標
/// </summary>
public class LaserTarget : BaseStageGimmick
{
    [SerializeField] private Color _openColor;
    [SerializeField] private Color _closeColor;

    private AudioSource _audioSource;
    //状態によって色を変更する
    private MeshRenderer _meshRenderer;
    private ParticleSystem.MainModule _particle;

    void Start()
    {
        TryGetComponent(out _audioSource);
        TryGetComponent(out _meshRenderer);
        _particle = GetComponentInChildren<ParticleSystem>().main;

        _meshRenderer.material.color = _closeColor;
        _particle.startColor = Color.white;
    }

    private void OnEnable() {
        EventCenter.AddLaserTarget(this);
    }

    private void OnDisable(){
        EventCenter.RemoveLaserTarget(this);
    }

    public override void Notify(int num, bool state)
    {
        if (this.Number != num) { return; }

        //状態切り替え
        if(state){
            _isOpen = true;
            _meshRenderer.material.color = _openColor;
            _particle.startColor = _openColor;
            _audioSource.Play();
            EventCenter.StageClearCheckNotify();
        }else{
            _isOpen = false;
            _meshRenderer.material.color = _closeColor;
            _particle.startColor = Color.white;
            EventCenter.StageClearCheckNotify();
        }
    }
}
