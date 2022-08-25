using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnumList;

public class UIManager : UnitySingleton<UIManager>
{
    [SerializeField] private Transform _canvas = null;
    [SerializeField] private List<UIControl> _uiList = new List<UIControl>();

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(this);

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
    /// 指定のUIを呼び出す
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public UIControl ShowUIView(string name)
    {
        string path = "Prefabs/UI/" + name;
        GameObject uiPrefab = Resources.Load<GameObject>(path); //ResourcesManager.Instance.GetAssetCache<GameObject>(path);
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
    /// 現在のUIパネルを外す
    /// </summary>
    public void ClearUI(){
        foreach(var item in _uiList){
            Destroy(item.gameObject);
        }
        _uiList.Clear();
    }
}