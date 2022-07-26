using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動プラットフォームの位置操作
/// </summary>
public class PositionInterpolator : MonoBehaviour
{
    [SerializeField] Rigidbody body = default;
    [Tooltip("移動量"), SerializeField] Vector3 offset = default;
    [SerializeField] Transform relativeTo = default; //現在地(参考目標)

    [Tooltip("原点"), SerializeField] Vector3 from = default;
    [Tooltip("目標点"), SerializeField] Vector3 to = default;
    

    void Start()
    {
        TryGetComponent(out body);
        from = transform.position;
        to = from + offset;
    }

    public void Interpolate(float t)
    {
        Vector3 p;
        if (relativeTo)
        {
            //参考目標のローカル座標に変換する(調整しやすい)
            p = Vector3.LerpUnclamped(relativeTo.TransformPoint(from), relativeTo.TransformPoint(to), t);
        }
        else
        {
            p = Vector3.LerpUnclamped(from, to, t);
        }
        body.MovePosition(p);
    }
}
