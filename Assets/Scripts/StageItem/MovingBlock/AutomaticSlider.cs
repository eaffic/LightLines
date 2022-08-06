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
    [SerializeField] bool _autoReversed = false;
    [SerializeField] bool _offToReturn = false;
    [SerializeField] bool _smoothstep = false; //スムーズ移動

    float _value;
    float _smoothedValue => 3f * _value * _value - 2f * _value * _value * _value; //スムーズ関数
    bool Reversed { get; set; } //往復確認
    bool AutoReverse //往復移動
    {
        get => _autoReversed;
        set => _autoReversed = value;
    }
    bool OffToReturn //off状態は元の位置に戻る
    {
        get => _offToReturn;
        set => _offToReturn = value;
    }

    private void OnEnable()
    {
        EventCenter.AddButtonListener(Notify);
    }

    private void OnDisable()
    {
        EventCenter.RemoveButtonListener(Notify);
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

        if (AutoReverse)
        {
            AutoReserveseMove();
        }
        else
        {
            NormalMove();
        }
    }

    void ReturnToDefaultPosition()
    {
        if (!OffToReturn || _value <= 0f) { return; }

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
        if (Reversed)
        {
            // 1-->0
            _value -= delta;
            if (_value <= 0f)
            {
                _value = Mathf.Min(1f, -_value);
                Reversed = false;
            }
        }
        else
        {
            // 0-->1
            _value += delta;
            if (_value >= 1f)
            {
                //毎フレームの移動量は固定しているので、ここの_valueの値は直接変更不可です(_value = 1f;　のこと)
                _value = Mathf.Max(0f, 2f - _value);
                Reversed = true;
            }
        }
        _onValueChanged.Invoke(_smoothstep ? _smoothedValue : _value);
    }

    void NormalMove()
    {
        float delta = Time.deltaTime / _duration;
        // 0-->1
        _value += delta;
        if (_value >= 1f)
        {
            _value = 1f;
        }
        _onValueChanged.Invoke(_smoothstep ? _smoothedValue : _value);
    }

    public override void Notify(int num, bool state)
    {
        if (Number != num) { return; }
        _isOpen = state;
    }
}
