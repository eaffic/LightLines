using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 新機能テスト（キャラエフェクト）
/// </summary>
public class IntoNewScene : UnitySingleton<IntoNewScene> {
    [SerializeField] private Material _playerBody;
    [SerializeField] private Material _playerHair;
    [SerializeField] private Material _playerFace;

    [SerializeField] private float _intoDissolveHeight;
    [SerializeField] private float _exitDissolveHeight;

    protected override void Awake()
    {
        base.Awake();
    }

    public void IntoScene(){
        StartCoroutine(StartIntoScene());
    }

    public void ExitScene(){
        Debug.Log("exit");
        StartCoroutine(StartExitScene());
    }   

    IEnumerator StartIntoScene(){
        float height = _exitDissolveHeight;
        while(height >= _intoDissolveHeight){
            height -= 0.75f * Time.deltaTime;

            _playerBody.SetFloat("_DissolveY", height);
            _playerFace.SetFloat("_DissolveY", height);
            _playerHair.SetFloat("_DissolveY", height);
        }
        yield return null;
    }

    IEnumerator StartExitScene(){
        float height = _intoDissolveHeight;
        while(height <= _exitDissolveHeight){
            height += 0.75f * Time.deltaTime;

            _playerBody.SetFloat("_DissolveY", height);
            _playerFace.SetFloat("_DissolveY", height);
            _playerHair.SetFloat("_DissolveY", height);
        }
        yield return null;
    }
}