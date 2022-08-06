using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnumList;

public class StageIcon : MonoBehaviour
{
    [SerializeField] private SceneType _targetStage;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _inputPanels;

    private bool _enabled;

    private void Update()
    {
        _canvas.transform.rotation = Camera.main.transform.rotation;

        InputCheck();
    }

    private void InputCheck()
    {
        if (_enabled == false) { return; }
        if (GameManager.OpenMenu) { return; }

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
