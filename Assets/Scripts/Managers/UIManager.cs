using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnumList;

/// <summary>
/// UI生成、管理クラス
/// </summary>
public class UIManager : UnitySingleton<UIManager>
{
    [SerializeField] private Transform _canvas = null;
    [SerializeField] private List<UIControl> _uiList = new List<UIControl>(); //起動中すべてのUI

    protected override void Awake()
    {
        base.Awake();

        _canvas = GameObject.Find("Canvas").transform;
    }

    private void Update()
    {
        if (GameManager.Pause) { return; }

        if (GameInputManager.Instance.GetUIMenuInput())
        {
            EventCenter.UINotify("Menu");
        }
    }

    /// <summary>
    /// 指定のUIを呼び出し、対応のスクリプトをアタッチする
    /// </summary>
    /// <param name="name">UIオブジェクト名</param>
    /// <returns></returns>
    public UIControl ShowUIView(string name)
    {
        string path = "Prefabs/UI/" + name;
        GameObject uiPrefab = Resources.Load<GameObject>(path); 
        GameObject uiView = GameObject.Instantiate(uiPrefab);
        uiView.name = uiPrefab.name;
        uiView.transform.SetParent(_canvas, false);

        int lastIndex = name.LastIndexOf("/");
        if (lastIndex > 0)
        {
            name = name.Substring(lastIndex + 1);
        }

        Type type = Type.GetType(name + "_UIControl");
        UIControl control = (UIControl)uiView.AddComponent(type);

        if (!_uiList.Contains(control)){
            _uiList.Add(control);
        }
        return control;
    }

    /// <summary>
    /// 現在起動中すべてのUIオブジェクトを外す
    /// </summary>
    public void ClearUI(){
        foreach(var item in _uiList){
            Destroy(item.gameObject);
        }
        _uiList.Clear();
    }
}