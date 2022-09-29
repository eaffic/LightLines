using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザー
/// </summary>
public class Laser : BaseStageGimmick
{
    [SerializeField] private GameObject _startBeam; //エフェクト
    [SerializeField] private GameObject _startPartical; //エフェクト
    [SerializeField] private GameObject _endBeam;   //エフェクト
    [SerializeField] private GameObject _endPartical;   //エフェクト
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LayerMask _blockLayer; //障害物レイヤー
    [SerializeField] private float _laserDistance; //レーザー長さ
    [SerializeField, Range(0, 50)] private int _maxReflectCount = 5; //反射可能回数

    private GameObject _currentHitObject; //当たったもの
    private GameObject _currentHitTarget; //当たったターゲット

    void Start()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    void Update()
    {
        OnRay();
        CheckTarget();
    }

    /// <summary>
    /// レーザ経路確認
    /// </summary>
    void OnRay()
    {
        //レンダラー位置リセット
        _lineRenderer.positionCount = 2;

        //初期方向、位置
        Vector3 direction = transform.forward;
        Vector3 pos = transform.position;

        RaycastHit hitInfo;
        //反射射線記録リスト
        List<Ray> rays = new List<Ray>();
        List<Vector3> laserPoint = new List<Vector3>(); //レイの経過、転向ポイント
        rays.Add(new Ray(pos, transform.forward)); //初期射線
        laserPoint.Add(pos);
        _lineRenderer.SetPosition(0, pos);

        int reflectCount = 0;
        for (int i = 0; i < rays.Count; ++i)
        {
            //射線判定
            if (Physics.Raycast(rays[i], out hitInfo, _laserDistance, _blockLayer))
            {
                //障害物位置記録(エフェクト位置設定のため)
                _currentHitObject = hitInfo.collider.gameObject;
                _lineRenderer.SetPosition(i + 1, hitInfo.point);

                if (hitInfo.collider.tag == "Target")
                {
                    //クリア目標
                    if (_currentHitObject.GetComponent<IStageGimmick>().ID == this.ID)
                    {
                        _currentHitTarget = _currentHitObject;
                        if (!_currentHitTarget.GetComponent<IStageGimmick>().IsOpen)
                        {
                            _currentHitTarget.GetComponent<IStageGimmick>().OnNotify(ID, true);
                        }
                    }
                }
                else if (reflectCount < _maxReflectCount && hitInfo.collider.tag == "Mirror")
                {
                    //反射方向を決める
                    direction = Vector3.Reflect((hitInfo.point - laserPoint[i]).normalized, hitInfo.normal);
                    laserPoint.Add(hitInfo.point);
                    rays.Add(new Ray(hitInfo.point, direction)); //新しい射線を作る

                    _lineRenderer.positionCount++;
                    reflectCount++;
                }
                else if(hitInfo.collider.tag == "SecretItem"){
                    //アイテムを破壊する
                    hitInfo.transform.gameObject.GetComponent<Item>().StartDissolve();
                }

                _endBeam.transform.position = hitInfo.point;
                _endPartical.transform.position = hitInfo.point;

                //反射がある場合、エフェクトの角度を調整する
                if (rays.Count > 0)
                {
                    _endPartical.transform.LookAt(rays[i].origin);
                }
            }
            else
            {
                //何も当たらない
                //エフェクト位置は最大距離で設定する
                _currentHitObject = null;
                _lineRenderer.SetPosition(i + 1, laserPoint[i] + direction * _laserDistance);

                _endBeam.transform.position = laserPoint[i] + direction * _laserDistance;
                _endPartical.transform.position = laserPoint[i] + direction * _laserDistance;

                //反射がある場合、粒子エフェクトの角度を調整する
                if (rays.Count > 0)
                {
                    _endPartical.transform.LookAt(hitInfo.point);
                }
            }
            //Debug.DrawRay(rays[i].origin, rays[i].direction * _laserDistance, Color.green, 0.1f);
        }
    }

    /// <summary>
    /// 目標確認
    /// </summary>
    void CheckTarget()
    {
        if (_currentHitTarget == null) return;

        if (_currentHitTarget != _currentHitObject)
        {
            _currentHitTarget.GetComponent<IStageGimmick>().OnNotify(this.ID, false);
            _currentHitTarget = null;
        }
    }

    /// <summary>
    /// エフェクト表示設定
    /// </summary>
    void SetParticalEffect()
    {
        _startPartical.SetActive(!_startPartical.activeSelf);
        _startBeam.SetActive(!_startBeam.activeSelf);
        _endPartical.SetActive(!_endPartical.activeSelf);
        _endBeam.SetActive(!_endBeam.activeSelf);
    }

    /// <summary>
    /// EventCenterから呼び出す
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    public override void OnNotify(int id, bool state)
    {
        _isOpen = state;
        SetParticalEffect();
    }
}
