using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameEnumList;

public class LoadManagerScene : MonoBehaviour
{
    [SerializeField] private SceneType _startScene;
    private static bool _loaded;

    private void Awake() {
        if(_loaded) return;

        _loaded = true;
        SceneManager.LoadSceneAsync((int)SceneType.UI, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync((int)SceneType.Manager, LoadSceneMode.Additive);
    }

    private void Start() {
        GameManager.Instance.SetStartCurrentScene(_startScene);
    }
}
