using System;
using System.Collections;
using System.Collections.Generic;
using GameEnumList;
using UnityEngine;

/// <summary>
/// ステージ内のイベント管理
/// </summary>
public class EventCenter
{
    public static bool Enabled = true;

    //クリア目標リスト
    private static List<IStageGimmick> _laserTargetList = new List<IStageGimmick>(16);

    #region 委託
    private static List<Action> _stageSecretItemObserver = new List<Action>(8);
    private static List<Action<bool>> _stageClearObserver = new List<Action<bool>>(8);
    private static List<Action<int, bool>> _buttonObserver = new List<Action<int, bool>>(256); //サイズは先に決まります
    private static List<Action<SceneType>> _fadeInOutObserver = new List<Action<SceneType>>(8);
    private static List<Action<SceneType>> _stageInfomationObserver = new List<Action<SceneType>>(8);
    #endregion

    #region ボタンと連動するギミック
    /// <summary>
    /// ギミック登録
    /// </summary>
    /// <param name="action"></param>
    public static void AddButtonListener(Action<int, bool> action)
    {
        if (!_buttonObserver.Contains(action))
        {
            _buttonObserver.Add(action);
        }
    }

    /// <summary>
    /// ギミック登録解除
    /// </summary>
    /// <param name="action"></param>
    public static void RemoveButtonListener(Action<int, bool> action)
    {
        _buttonObserver.Remove(action);
    }

    /// <summary>
    /// ボタンからの接触通知
    /// </summary>
    /// <param name="num"></param>
    public static void ButtonNotify(int num, bool state)
    {
        if (Enabled == false) { return; }
        for (int i = _buttonObserver.Count - 1; i >= 0; --i)
        {
            if (_buttonObserver[i] != null)
            {
                _buttonObserver[i].Invoke(num, state);
            }
        }
    }
    #endregion

    #region フェードインアウト効果
    public static void AddFadeListener(Action<SceneType> action)
    {
        if (!_fadeInOutObserver.Contains(action))
        {
            _fadeInOutObserver.Add(action);
        }
    }
    public static void RemoveFadeListener(Action<SceneType> action)
    {
        _fadeInOutObserver.Remove(action);
    }
    public static void FadeNotify(SceneType type)
    {
        if (Enabled == false) { return; }
        for (int i = _fadeInOutObserver.Count - 1; i >= 0; --i)
        {
            if (_fadeInOutObserver[i] != null)
            {
                _fadeInOutObserver[i].Invoke(type);
            }
        }
    }
    #endregion

    #region ステージクリア確認
    public static void AddLaserTarget(IStageGimmick target)
    {
        if (!_laserTargetList.Contains(target))
        {
            _laserTargetList.Add(target);
        }
    }
    public static void RemoveLaserTarget(IStageGimmick target)
    {
        _laserTargetList.Remove(target);
    }

    public static void AddStageClearListener(Action<bool> action)
    {
        if (!_stageClearObserver.Contains(action))
        {
            _stageClearObserver.Add(action);
        }
    }
    public static void RemoveStageClearListener(Action<bool> action)
    {
        _stageClearObserver.Remove(action);
    }

    public static void StageClearCheckNotify()
    {
        if (Enabled == false) { return; }

        //クリア確認
        bool check = true;
        for (int i = _laserTargetList.Count - 1; i >= 0; --i)
        {
            if (_laserTargetList[i] != null)
            {
                if (_laserTargetList[i].IsOpen == false)
                {
                    check = false;
                    break;
                }
            }
        }

        //クリア状況通知
        for (int i = _stageClearObserver.Count - 1; i >= 0; --i)
        {
            if (_stageClearObserver[i] != null)
            {
                _stageClearObserver[i].Invoke(check);
            }
        }
    }
    #endregion

    #region ステージ内の収集道具
    public static void AddStageSecretItemListener(Action action)
    {
        if (!_stageSecretItemObserver.Contains(action))
        {
            _stageSecretItemObserver.Add(action);
        }
    }
    public static void RemoveStageSecretItemListener(Action action)
    {
        _stageSecretItemObserver.Remove(action);
    }

    public static void GetSecretItemNotify()
    {
        if (Enabled == false) { return; }

        for (int i = _stageSecretItemObserver.Count - 1; i >= 0; --i)
        {
            if (_stageSecretItemObserver[i] != null)
            {
                _stageSecretItemObserver[i].Invoke();
            }
        }
    }
    #endregion

    #region ステージ選択画面のUI更新
    public static void AddStageInfomationListener(Action<SceneType> action){
        if(!_stageInfomationObserver.Contains(action)){
            _stageInfomationObserver.Add(action);
        }
    }
    public static void RemoveStageInfomationListener(Action<SceneType> action){
        _stageInfomationObserver.Remove(action);
    }

    public static void StageInfomationUpdateNotify(SceneType type){
        if (Enabled == false) { return; }

        for (int i = _stageInfomationObserver.Count - 1; i >= 0; --i){
            if(_stageInfomationObserver[i] != null){
                _stageInfomationObserver[i].Invoke(type);
            }
        }
    }
    #endregion
}