using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ResultUIController : MonoBehaviour
{
    [SerializeField] GameObject _resultUIPanel;
    [SerializeField] Text _stageNameText;
    [SerializeField] Text _clearTimeText;
    [SerializeField] Text _getItemText;
    [SerializeField] Text _nextStageText;
    [SerializeField] Text _stageSelectText;

    enum SELECTION { NEXTSTAGE, STAGESELECT }; //移行目標シーンの選択
    [SerializeField] SELECTION _selection;

    StageInfo _clearInfo;

    void Start()
    {
        StageManager._Instance.RegisterResultUI(this);
    }

    public void SetClearStageInfo(StageInfo stageInfo)
    {
        _clearInfo = stageInfo;

        //テキスト設定
        _resultUIPanel.SetActive(true);
        _stageNameText.text = _clearInfo.StageName;
        string time = String.Format("TIme: {0:00}:{1:00}:{2:00}",
            (int)_clearInfo.ClearTime / 60,
            (int)_clearInfo.ClearTime % 60,
            (_clearInfo.ClearTime - (int)_clearInfo.ClearTime) * 100);
        _clearTimeText.text = time;
        _getItemText.text = "Item: " + _clearInfo.SecretItemCount.ToString() + " / " + _clearInfo.SecretItemMaxCount.ToString();
    }

    public void SetSelectInput(int input)
    {
        AudioManager.PlayClickAudio();

        if(input > 0)
        {
            _selection = (SELECTION)Mathf.Max((int)--_selection, 0);
        }
        else
        {
            _selection = (SELECTION)Mathf.Min((int)++_selection, 1);
        }

        switch (_selection)
        {
            case SELECTION.NEXTSTAGE:
                _nextStageText.color = Color.red;
                _stageSelectText.color = Color.white;
                break;
            case SELECTION.STAGESELECT:
                _nextStageText.color = Color.white;
                _stageSelectText.color = Color.red;
                break;
        }
    }
    
    public void SetSubmitInput()
    {
        AudioManager.PlaySubmitAudio();

        switch (_selection)
        {
            case SELECTION.NEXTSTAGE:
                StageManager._Instance.MoveToNextStageScene();
                break;
            case SELECTION.STAGESELECT:
                StageManager._Instance.MoveToSelectScene("StageSelect");
                break;
        }
    }
}
