using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlock : StageGimmick
{
    [SerializeField] float _rayLength = 10;
    [SerializeField] List<GameObject> _boxList;

    void Start()
    {
        StageManager._Instance.RegisterRotateBlock(this);
    }

    void Update() {
        _boxList.Clear();

        RaycastHit[] hitInfo = Physics.RaycastAll(transform.position, Vector3.up, _rayLength);
        foreach (var item in hitInfo){
            if(item.collider.tag == "Box"){
                _boxList.Add(item.collider.gameObject);
            }
        }

        Debug.DrawRay(transform.position, Vector3.up * 100, Color.red);
    }

    public override void Open()
    {
        IsOpen = true;
        foreach(var item in _boxList){
            item.GetComponent<Box>().RotateBox(90);
        }
    }

    public override void Close()
    {
        IsOpen = false;
    }
}
