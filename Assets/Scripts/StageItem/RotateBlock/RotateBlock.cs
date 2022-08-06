using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlock : BaseStageGimmick
{
    [SerializeField] float _rayLength = 10;
    [SerializeField] List<GameObject> _boxList;

    private void OnEnable() {
        EventCenter.AddButtonListener(Notify);
    }

    private void OnDisable() {
        EventCenter.RemoveButtonListener(Notify);
    }

    public override void Notify(int num, bool state)
    {
        if (state == false) { return; }
        if (Number != num) { return; }

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
