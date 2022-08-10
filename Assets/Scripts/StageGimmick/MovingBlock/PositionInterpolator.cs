using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動プラットフォームの位置操作
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PositionInterpolator : MonoBehaviour
{
    [Tooltip("移動量"), SerializeField] private Vector3 _offset = default;
    [SerializeField] private Transform _relativeTo = default; //現在地(参考目標)

    private Rigidbody _rigidbody = default;
    private Vector3 _from = default;
    private Vector3 _to = default;

    void Start()
    {
        TryGetComponent(out _rigidbody);
        _from = transform.position;
        _to = _from + _offset;
    }

    public void Interpolate(float t)
    {
        Vector3 p;
        if (_relativeTo)
        {
            //参考目標のローカル座標に変換する(調整しやすい)
            p = Vector3.LerpUnclamped(_relativeTo.TransformPoint(_from), _relativeTo.TransformPoint(_to), t);
        }
        else
        {
            p = Vector3.LerpUnclamped(_from, _to, t);
        }
        _rigidbody.MovePosition(p);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player"){
            other.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit(Collision other) {
        if(other.gameObject.tag == "Player"){
            other.transform.SetParent(null);
        }
    }
}
