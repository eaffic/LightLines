using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザー目標
/// </summary>
public class LaserTarget : StageGimmick
{
    [SerializeField] Color _openColor;
    [SerializeField] Color _closeColor;

    //状態によって色を変更する
    MeshRenderer _meshRenderer;
    ParticleSystem.MainModule _particle;

    // Start is called before the first frame update
    void Start()
    {
        StageManager._Instance.RegisterTarget(this);
        TryGetComponent(out _meshRenderer);
        _particle = GetComponentInChildren<ParticleSystem>().main;

        if (IsOpen)
        {
            _meshRenderer.material.color = _openColor;
            _particle.startColor = _openColor;
        }
        else
        {
            _meshRenderer.material.color = _closeColor;
            _particle.startColor = Color.white;
        }
    }

    public override void Open()
    {
        IsOpen = true;
        StageManager._Instance.RemoveTarget(this);
        //StageManager.Instance.OpenSameNumberItem(this._Number);
        _meshRenderer.material.color = _openColor;
        _particle.startColor = _openColor;
        AudioManager.PlayOpenTargetAudio();
    }

    public override void Close()
    {
        IsOpen = false;
        StageManager._Instance.RegisterTarget(this);
        //StageManager.Instance.CloseSameNumberItem(this._Number);
        _meshRenderer.material.color = _closeColor;
        _particle.startColor = Color.white;
    }
}
