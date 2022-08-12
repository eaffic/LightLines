using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 移動プラットフォームの設定
/// </summary>
public class AutomaticSlider : BaseStageGimmick
{
    [System.Serializable]
    public class OnValueChangedEvent : UnityEvent<float> { }
    [SerializeField] OnValueChangedEvent _onValueChanged = default;
    [SerializeField, Min(0.01f)] float _duration = 1f; //移動時間
    [SerializeField] bool _autoReversed = false; //往復移動
    [SerializeField] bool _offToReturn = false; //停止後原位置に戻る
    [SerializeField] bool _smoothstep = false; //よりスムーズの移動

    float _value; //現在位置の割合（0原位置 1目標位置）
    float _smoothedValue => 3f * _value * _value - 2f * _value * _value * _value; //スムーズ関数
    private bool _reversed; //往復確認

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

    void FixedUpdate()
    {
        float delta = Time.deltaTime / _duration;
        if (!IsOpen)
        {
            //元の位置に戻す
            ReturnToDefaultPosition();
            return;
        }

        
        if (_autoReversed)
        {
            //往復移動
            AutoReserveseMove();
        }
        else
        {
            //単方向移動
            NormalMove();
        }
    }

    /// <summary>
    /// 原位置に戻す
    /// </summary>
    void ReturnToDefaultPosition()
    {
        if (_offToReturn == false || _value <= 0f) { return; }

        float delta = Time.deltaTime / _duration;
        _value -= delta;
        _onValueChanged.Invoke(_smoothstep ? _smoothedValue : _value);
    }

    /// <summary>
    /// 自動往復
    /// </summary>
    void AutoReserveseMove()
    {
        float delta = Time.deltaTime / _duration;
        if (_reversed)
        {
            // 目標位置(1)-->原位置(0)
            _value -= delta;
            if (_value <= 0f)
            {
                _value = Mathf.Min(1f, -_value);
                _reversed = false;
            }
        }
        else
        {
            // 原位置(0)-->目標位置(1)
            _value += delta;
            if (_value >= 1f)
            {
                //毎フレームの移動量は固定しているので、ここの_valueの値は直接変更不可です(_value = 1f;　のこと)
                _value = Mathf.Max(0f, 2f - _value);
                _reversed = true;
            }
        }
        _onValueChanged.Invoke(_smoothstep ? _smoothedValue : _value);
    }

    /// <summary>
    /// 単方向移動
    /// </summary>
    void NormalMove()
    {
        float delta = Time.deltaTime / _duration;
        // 原位置(0)-->目標位置(1)
        _value += delta;
        if (_value >= 1f)
        {
            _value = 1f;
        }
        _onValueChanged.Invoke(_smoothstep ? _smoothedValue : _value);
    }

    /// <summary>
    /// EventCenterから呼び出す
    /// </summary>
    /// <param name="num"></param>
    /// <param name="state"></param>
    public override void OnNotify(int num, bool state)
    {
        if (ID != num) { return; }
        _isOpen = state;
    }
}
