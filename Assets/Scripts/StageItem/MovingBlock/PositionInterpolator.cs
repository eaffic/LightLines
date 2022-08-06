using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動プラットフォームの位置操作
/// </summary>
public class PositionInterpolator : MonoBehaviour
{
    
    [Tooltip("移動量"), SerializeField] private Vector3 offset = default;
    [SerializeField] private Transform relativeTo = default; //現在地(参考目標)

    private Rigidbody body = default;
    private Vector3 from = default;
    private Vector3 to = default;


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
