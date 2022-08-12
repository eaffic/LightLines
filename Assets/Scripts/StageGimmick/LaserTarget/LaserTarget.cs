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

    //状態によって色を変更する
    private Light _light;
    private MeshRenderer _meshRenderer;
    private ParticleSystem.MainModule _particle;

    private void OnEnable()
    {
        //EventCenterに登録
        EventCenter.AddLaserTarget(this);
    }

    private void OnDisable()
    {
        //登録を外す
        EventCenter.RemoveLaserTarget(this);
    }

    void Start()
    {
        TryGetComponent(out _light);
        TryGetComponent(out _meshRenderer);
        _particle = GetComponentInChildren<ParticleSystem>().main;

        _light.color = _closeColor;
        _meshRenderer.material.color = _closeColor;
        _particle.startColor = Color.white;
    }

    public override void OnNotify(int num, bool state)
    {
        if (this.ID != num) { return; }

        //状態切り替え
        if (state)
        {
            _isOpen = true;
            _light.color = _openColor;
            _meshRenderer.material.color = _openColor;
            _particle.startColor = _openColor;
            EventCenter.StageClearCheckNotify();
            AudioManager.Instance.Play("LaserTarget", "OpenTarget", false);
        }
        else
        {
            _isOpen = false;
            _light.color = _closeColor;
            _meshRenderer.material.color = _closeColor;
            _particle.startColor = Color.white;
            EventCenter.StageClearCheckNotify();
        }
    }
}
