using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour {
    private Dictionary<string, GameObject> _dictView = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> DictView { get => _dictView; }

    protected virtual void Awake() {
        LoadAllObject(this.gameObject, "");
    }

    private void LoadAllObject(GameObject root, string path){
        foreach(Transform item in root.transform){
            if(_dictView.ContainsKey(path + item.gameObject.name)){
                continue;
            }
            _dictView.Add(path + item.gameObject.name, item.gameObject);
            //子物件が存在している場合、もう一度実行する
            LoadAllObject(item.gameObject, path + item.gameObject + "/");
        }
    }

}