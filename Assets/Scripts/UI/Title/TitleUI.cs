using UnityEngine;
using UnityEngine.UI;
using GameEnumList;

public class TitleUI : MonoBehaviour {

    [SerializeField] private Text _startText;
    [SerializeField] private Text _deleteText;
    [SerializeField] private Text _deleteHintText;
    [SerializeField] private Text _exitText;
    [SerializeField] private Image _starIcon;

    private enum TitleSelect { GameStart, DeleteData, ExitGame };
    private TitleSelect _currentSelect = default;
    private float _oldinput;

    private void Start() {
        _startText.color = Color.red;
        _deleteText.color = Color.white;
        _exitText.color = Color.white;
        _starIcon.rectTransform.localPosition  = new Vector2(-_startText.rectTransform.rect.width / 1.8f, _startText.transform.localPosition.y);
    }

    private void Update()
    {
        _starIcon.transform.Rotate(Vector3.forward);

        UpdateCursor();
        Submit();
    }

    private void UpdateCursor(){
        float input;
        if (GameInputManager.Instance.GetUISelectInput(out input))
        {
            TitleSelect oldSelect = _currentSelect;
            //連続入力防止
            if (input > 0.8f && _oldinput < 0.8f)
            {
                _currentSelect = (TitleSelect)Mathf.Max((int)--_currentSelect, 0);
                _deleteHintText.enabled = false;
            }
            else if(input < -0.8f && _oldinput > -0.8f)
            {
                _currentSelect = (TitleSelect)Mathf.Min((int)++_currentSelect, 2);
                _deleteHintText.enabled = false;
            }

            if (_currentSelect == oldSelect) { return; }
            AudioManager.Instance.Play("UI", "Select", false);

            //テキストの色変更
            switch (_currentSelect)
            {
                case TitleSelect.GameStart:
                    _startText.color = Color.red;
                    _deleteText.color = Color.white;
                    _starIcon.rectTransform.localPosition = new Vector2(-_startText.rectTransform.rect.width / 1.8f, _startText.transform.localPosition.y);
                    break;
                case TitleSelect.DeleteData:
                    _startText.color = Color.white;
                    _deleteText.color = Color.red;
                    _exitText.color = Color.white;
                    _starIcon.rectTransform.localPosition = new Vector2(-_deleteText.rectTransform.rect.width / 1.8f, _deleteText.transform.localPosition.y);
                    break;
                case TitleSelect.ExitGame:
                    _deleteText.color = Color.white;
                    _exitText.color = Color.red;
                    _starIcon.rectTransform.localPosition = new Vector2(-_exitText.rectTransform.rect.width / 1.8f, _exitText.transform.localPosition.y);
                    break;
            }
        }

        _oldinput = input;
    }
    
    private void Submit(){
        if (GameInputManager.Instance.GetUISubmitInput())
        {
            switch (_currentSelect)
            {
                case TitleSelect.GameStart:
                    AudioManager.Instance.Play("UI", "Submit", false);
                    GameStart();
                    break;
                case TitleSelect.DeleteData:
                    AudioManager.Instance.Play("UI", "DeleteData", false);
                    DeleteData();
                    break;
                case TitleSelect.ExitGame:
                    ExitGame();
                    break;
            }
        }
    }

    private void GameStart()
    {
        EventCenter.FadeNotify(SceneType.StageSelect);
    }

    private void DeleteData()
    {
        _deleteHintText.enabled = true;
        DataManager.DeleteSaveFile("savedata.json");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}