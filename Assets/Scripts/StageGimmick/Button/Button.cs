using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボタン
/// </summary>
public class Button : StageGimmick
{
    [SerializeField] bool _stateLock;   //起動状態を維持する
    [SerializeField] LayerMask _searchLayer = default;

    Light _light;
    MeshRenderer _meshRenderer;

    void Awake()
    {
        _light = GetComponentInChildren<Light>();
        TryGetComponent(out _meshRenderer);
    }

    void Start()
    {
        StageManager._Instance.RegisterButton(this);

        if(TopCheck())
        {
            IsOpen = true;
            StageManager._Instance.OpenSameNumberItem(_Number);
            _light.GetComponent<Light>().color = Color.green;
            _meshRenderer.material.color = Color.green;
        }
        else
        {
            IsOpen = false;
            StageManager._Instance.CloseSameNumberItem(_Number);
            _light.GetComponent<Light>().color = Color.red;
            _meshRenderer.material.color = Color.red;
        }
    }

    /// <summary>
    /// ボタンの上の物体チェック
    /// </summary>
    /// <returns></returns>
    bool TopCheck()
    {
        var colliders = Physics.OverlapBox(transform.position + transform.up * 0.5f, new Vector3(0.4f, 0.4f, 0.4f), Quaternion.identity, _searchLayer);
        if (colliders.Length > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// ボタンの起動、終了チェック
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        if (!IsOpen && TopCheck())
        {
            IsOpen = true;
            StageManager._Instance.OpenSameNumberItem(_Number);
            _light.GetComponent<Light>().color = Color.green;
            _meshRenderer.material.color = Color.green;
        }
        else if (!_stateLock && IsOpen && !TopCheck())
        {
            IsOpen = false;
            StageManager._Instance.CloseSameNumberItem(_Number);
            _light.GetComponent<Light>().color = Color.red;
            _meshRenderer.material.color = Color.red;
        }
    }
    void OnTriggerExit(Collider other) {
        IsOpen = false;
        StageManager._Instance.CloseSameNumberItem(_Number);
        _light.GetComponent<Light>().color = Color.red;
        _meshRenderer.material.color = Color.red;
    }
}
