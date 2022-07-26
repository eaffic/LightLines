using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの入場エフェクト
/// </summary>
public class PlayerMaterial : MonoBehaviour {
    [SerializeField] Renderer[] _renderers;

    [SerializeField] private float _intoDissolveValue;
    [SerializeField] private float _exitDissolveValue;

    private void Start() {
        _exitDissolveValue = transform.position.y + 3; //キャラが消える高さ
        _intoDissolveValue = transform.position.y - 1; //キャラが現れる高さ
        IntoScene();
    }

    public void IntoScene()
    {
        StartCoroutine(StartIntoScene());
    }

    public void ExitScene()
    {
        StartCoroutine(StartExitScene());
    }

    /// <summary>
    /// シーンに入るときのエフェクト
    /// </summary>
    /// <returns></returns>
    IEnumerator StartIntoScene()
    {
        float height = _exitDissolveValue;
        while (height >= _intoDissolveValue)
        {
            height -= Time.deltaTime;

            for (int i = 0; i < _renderers.Length; ++i)
            {
                _renderers[i].material.SetFloat("_DissolveY", height);
            }
            yield return null;
        }

        for (int i = 0; i < _renderers.Length; ++i)
        {
            _renderers[i].material.SetFloat("_DissolveY", -100);
        }
        yield return null;
    }

    IEnumerator StartExitScene()
    {
        _intoDissolveValue = transform.position.y - 2;
        _exitDissolveValue = transform.position.y + 1;

        float height = _intoDissolveValue;

        for (int i = 0; i < _renderers.Length; ++i)
        {
            _renderers[i].material.SetFloat("_DissolveY", height);
        }

        while (height <= _exitDissolveValue)
        {
            height += Time.deltaTime * 2f;

            for (int i = 0; i < _renderers.Length; ++i)
            {
                _renderers[i].material.SetFloat("_DissolveY", height);
            }
            yield return null;
        }
        yield return null;
    }
}