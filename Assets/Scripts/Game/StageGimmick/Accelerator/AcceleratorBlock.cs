using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 箱加速ブロック
/// </summary>
public class AcceleratorBlock : BaseStageGimmick
{
    [SerializeField] private float _moveTime = 0.3f; //箱の移動時間
    [SerializeField] private float _colorDuration = 2f; //色点滅時間
    [SerializeField] private Vector3 _moveOffset = Vector3.zero; //箱の移動先

    [SerializeField] private List<GameObject> _targetBox; //移動させる目標
    [SerializeField] private GameObject _nextAccelerator; //次の移動先の加速器

    private Vector3 _targetPositon;
    private LineRenderer _lineRenderer;

    private void OnEnable() {
        EventCenter.AddButtonListener(OnNotify);
    }

    private void OnDisable() {
        EventCenter.RemoveButtonListener(OnNotify);
    }

    private void Awake()
    {
        // 移動先次の加速器の存在確認
        RaycastHit[] hitInfo = Physics.RaycastAll(transform.position, _moveOffset.normalized, _moveOffset.magnitude);
        foreach (var item in hitInfo)
        {
            if (item.transform.tag == "AcceleratorBlock")
            {
                if ((item.transform.position - (transform.position + _moveOffset)).magnitude < 0.1f){
                    _nextAccelerator = item.transform.gameObject;
                }
            }
        }

        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position + _moveOffset);
    }

    private void Update() {
        SetMaterialColor();
        MoveTargetBox();
    }

    /// <summary>
    /// 状態と合わせて色を設定する
    /// </summary>
    private void SetMaterialColor(){
        float lerp = Mathf.PingPong(Time.time, _colorDuration) / _colorDuration;

        if(IsOpen)
        {
            if(_targetBox.Count > 0){
                _lineRenderer.material.SetColor("_MainColor", Color.Lerp(Color.red, new Color(0.5f, 0f, 0f, 1f), lerp));
                _lineRenderer.material.SetColor("_EmissionColor", Color.Lerp(Color.red, new Color(0.5f, 0f, 0f, 1f), lerp));
                _lineRenderer.material.SetFloat("_ArrowSpeed", 2.5f);
            }else{
                _lineRenderer.material.SetColor("_MainColor", Color.Lerp(Color.green, new Color(0f, 0.5f, 0f, 1f), lerp));
                _lineRenderer.material.SetColor("_EmissionColor", Color.Lerp(Color.green, new Color(0f, 0.5f, 0f, 1f), lerp));
                _lineRenderer.material.SetFloat("_ArrowSpeed", 1f);
            }
        }
        else
        {
            _lineRenderer.material.SetColor("_MainColor", Color.Lerp(Color.blue, new Color(0f, 0f, 0.5f, 1f), lerp));
            _lineRenderer.material.SetColor("_EmissionColor", Color.Lerp(Color.blue, new Color(0f, 0f, 0.5f, 1f), lerp));
            _lineRenderer.material.SetFloat("_ArrowSpeed", 0.25f);
        }
    }

    /// <summary>
    /// 箱を移動させる
    /// </summary>
    private void MoveTargetBox(){
        if (IsOpen == false) { return; }

        if(_targetBox.Count > 0){
            foreach(var item in _targetBox){
                //箱の状態を確認し(地面上、移動なし、回転なし)、加速器と連結する
                if (item.GetComponent<Box>().OnGround && item.GetComponent<Box>().OnMove == false && item.GetComponent<Box>().OnRotate == false)
                {
                    item.GetComponent<Box>().IsContactAccelerator = true;
                }

                //加速器と連結している時
                if (item.GetComponent<Box>().IsContactAccelerator)
                {
                    //移動を実行した時
                    if (item.GetComponent<Box>().MoveBox(_moveOffset))
                    {
                        //次の加速器はない場合、連結を除去する
                        if (_nextAccelerator == null)
                        {
                            item.GetComponent<Box>().IsContactAccelerator = false;
                        }
                    }
                }
            }
        }
    }

    #region 接触処理
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Box")
        {
            if (_targetBox.Contains(other.gameObject) == false)
            {
                _targetBox.Add(other.gameObject);

            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Box"){
            if (_targetBox.Contains(other.gameObject))
            {
                _targetBox.Remove(other.gameObject);
            }
        }
    }

    /// <summary>
    /// EventCenterから呼び出す
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    public override void OnNotify(int id, bool state)
    {
        if (ID != id) { return; }
        _isOpen = state;
    }
    #endregion
}
