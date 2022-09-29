using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回転ブロック
/// </summary>
public class RotateBlock : BaseStageGimmick
{
    [SerializeField] float _rayLength = 10; //検知距離

    private void OnEnable()
    {
        EventCenter.AddButtonListener(OnNotify);
    }

    private void OnDisable()
    {
        EventCenter.RemoveButtonListener(OnNotify);
    }

    /// <summary>
    /// EventCenterからの呼び出しを受け取る
    /// </summary>
    /// <param name="id">ギミックのID</param>
    /// <param name="state">呼び側の状態(ボタンの場合押すと離す)</param>
    public override void OnNotify(int id, bool state)
    {
        if (state == false) { return; }
        if (ID != id) { return; }

        //レイを利用して、上にある箱を全部回転させる
        RaycastHit[] hitInfo = Physics.RaycastAll(transform.position, Vector3.up, _rayLength, LayerMask.GetMask("Box"));
        if (hitInfo.Length > 0)
        {
            foreach (var item in hitInfo)
            {
                item.collider.gameObject.GetComponent<Box>().RotateBox(90);
            }
        }
    }
}
