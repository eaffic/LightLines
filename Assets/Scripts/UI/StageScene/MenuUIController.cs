using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] GameObject _menuUIPanel;
    [SerializeField] Text _stageNameText;
    [SerializeField] Text _timeText;
    [SerializeField] Text _secretItemCountText;

    StageInfo _stageInfo;

    // Start is called before the first frame update
    void Start()
    {
        StageManager._Instance.RegisterMenuUI(this);
    }

    /// <summary>
    /// ÉÅÉjÉÖÅ[âÊñ ÇÃï∂éöê›íË
    /// </summary>
    void SetMenuText()
    {
        string s = String.Format("TIme: {0:00}:{1:00}:{2:00}", 
            (int)_stageInfo.ClearTime / 60, 
            (int)_stageInfo.ClearTime % 60, 
            (_stageInfo.ClearTime - (int)_stageInfo.ClearTime) * 100);
        _timeText.text = s;
        _stageNameText.text = GameManager._ActiveSceneIndex.ToString();
        _secretItemCountText.text = "Item: " + _stageInfo.SecretItemCount + " / " + _stageInfo.SecretItemMaxCount;

        _menuUIPanel.SetActive(!_menuUIPanel.activeInHierarchy);
    }

    public void SetMenuUIInfo(StageInfo stageInfo)
    {
        this._stageInfo = stageInfo;
        SetMenuText();
    }
}
