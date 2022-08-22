using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回転ブロック、このブロックの上にいる箱を毎回90度回転する
/// </summary>
public class RotateBlock : BaseStageGimmick
{
    /// <summary>
    /// 回転探索の距離(二階構造があるステージの場合が必要)
    /// </summary>
    [SerializeField] float _rayLength = 10;
    /// <summary>
    /// 回転範囲内の箱リスト
    /// </summary>
    [SerializeField] List<GameObject> _boxList;

    private void OnEnable()
    {
        //起動時、EventCenterに登録する
        EventCenter.AddButtonListener(OnNotify);
    }

    private void OnDisable()
    {
        //終了時、登録を外す
        EventCenter.RemoveButtonListener(OnNotify);
    }

    /// <summary>
    /// EventCenterからの呼び出しを受け取る、対応する
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
