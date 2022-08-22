using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnumList;

/// <summary>
/// ステージ名表示、入力提示
/// </summary>
public class StageIconUI : MonoBehaviour
{
    [SerializeField] private SceneType _targetStage; //移行先のステージ
    [SerializeField] private GameObject _canvas; //ステージ名などのUi
    [SerializeField] private GameObject _inputPanels; //入力提示パネル

    private bool _enabled; //起動確認

    private void Update()
    {
        _canvas.transform.rotation = Camera.main.transform.rotation;

        InputCheck();
    }

    /// <summary>
    /// 入力チェック
    /// </summary>
    private void InputCheck()
    {
        if (_enabled == false) { return; }
        if (GameManager.Pause) { return; }
        
        if (GameInputManager.Instance.GetUISubmitInput()){
            EventCenter.StageInfomationUpdateNotify(_targetStage);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            _enabled = true;
            _inputPanels.SetActive(_enabled);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            _enabled = false;
            _inputPanels.SetActive(_enabled);
        }
    }
}
