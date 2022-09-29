using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動プラットフォームの位置操作
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PositionInterpolator : MonoBehaviour
{
    [Tooltip("移動量"), SerializeField] private Vector3 _offset = default; //原位置との距離差
    [SerializeField] private Transform _relativeTo = default; //参考座標(なかったら世界座標を使う)

    private Rigidbody _rigidbody = default;
    private Vector3 _from = default; //現位置
    private Vector3 _to = default; //目標位置

    void Start()
    {
        TryGetComponent(out _rigidbody);
        _from = transform.position;
        _to = _from + _offset;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
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
        _rigidbody.MovePosition(p); //rigidBodyを利用して移動させる
    }

#region 接触処理
    private void OnCollisionEnter(Collision other)
    {
        //このブロックの上に移動できるため、一時的に親オブジェクトを設定する
        if (other.gameObject.tag == "Player")
        {
            other.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.SetParent(null);
        }
    }
#endregion
}
