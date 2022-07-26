using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイトルの選択肢対応
/// </summary>
public class TitleSceneController : MonoBehaviour
{
    [SerializeField] FadeInOut _fade;
    [SerializeField] Text _start;
    [SerializeField] Text _deleteData;
    [SerializeField] Text _exit;
    [SerializeField] GameObject _deleteText;

    enum SELECTION
    {
        START = 0,
        DELETE = 1,
        EXIT = 2
    }
    SELECTION _selectIcon;

    // Start is called before the first frame update
    void Start()
    {
        _selectIcon = SELECTION.START;
        _start.color = Color.red;
        _deleteData.color = Color.white;
        _exit.color = Color.white;
    }

    //セレクト
    public void SetSelectInput(int input)
    {
        AudioManager.PlayClickAudio();

        if(input > 0)
        {
            _selectIcon = (SELECTION)Mathf.Max((int)--_selectIcon, 0);
            _deleteText.SetActive(false);
        }
        else
        {
            _selectIcon = (SELECTION)Mathf.Min((int)++_selectIcon, 2);
            _deleteText.SetActive(false);
        }

        //テキストの色変更
        switch(_selectIcon)
        {
            case SELECTION.START:
                _start.color = Color.red;
                _deleteData.color = Color.white;
                _exit.color = Color.white;
                break;
            case SELECTION.DELETE:
                _start.color = Color.white;
                _deleteData.color = Color.red;
                _exit.color = Color.white;
                break;
            case SELECTION.EXIT:
                _start.color = Color.white;
                _deleteData.color = Color.white;
                _exit.color = Color.red;
                break;
        }
    }

    //選択したボタンの応対
    public void SetSubmitInput()
    {
        AudioManager.PlaySubmitAudio();

        switch(_selectIcon)
        {
            case SELECTION.START:
                _fade.StartFadeOut();
                break;
            case SELECTION.DELETE:
                DataManager.DeleteSaveFile("savedata.json");
                _deleteText.SetActive(true);
                break;
            case SELECTION.EXIT:
                Application.Quit();
                break;
        }
    }
}
