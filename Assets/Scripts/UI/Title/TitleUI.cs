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

    private void Start() {
        _startText.color = Color.red;
        _starIcon.rectTransform.localPosition  = new Vector2(-_startText.rectTransform.rect.width / 1.8f, _startText.transform.localPosition.y);
    }

    private void Update()
    {
        _starIcon.transform.Rotate(Vector3.forward);

        float input;
        if (GameInputManager.Instance.GetUISelectInput(out input))
        {
            if (input > 0)
            {
                _currentSelect = (TitleSelect)Mathf.Max((int)--_currentSelect, 0);
                _deleteHintText.enabled = false;
            }
            else{
                _currentSelect = (TitleSelect)Mathf.Min((int)++_currentSelect, 2);
                _deleteHintText.enabled = false;
            }

            //テキストの色変更
            switch (_currentSelect)
            {
                case TitleSelect.GameStart:
                    _startText.color = Color.red;
                    _deleteText.color = Color.white;
                    _exitText.color = Color.white;
                    _starIcon.rectTransform.localPosition = new Vector2(-_startText.rectTransform.rect.width / 1.8f, _startText.transform.localPosition.y);
                    break;
                case TitleSelect.DeleteData:
                    _startText.color = Color.white;
                    _deleteText.color = Color.red;
                    _exitText.color = Color.white;
                    _starIcon.rectTransform.localPosition = new Vector2(-_deleteText.rectTransform.rect.width / 1.8f, _deleteText.transform.localPosition.y);
                    break;
                case TitleSelect.ExitGame:
                    _startText.color = Color.white;
                    _deleteText.color = Color.white;
                    _exitText.color = Color.red;
                    _starIcon.rectTransform.localPosition = new Vector2(-_exitText.rectTransform.rect.width / 1.8f, _exitText.transform.localPosition.y);
                    break;
            }
        }

        if(GameInputManager.Instance.GetUISubmitInput()){
            switch(_currentSelect){
                case TitleSelect.GameStart:
                    GameStart();
                    break;
                case TitleSelect.DeleteData:
                    DeleteData();
                    break;
                case TitleSelect.ExitGame:
                    ExitGame();
                    break;
            }
        }
    }

    public void GameStart()
    {
        if (FadeInOut.OnSceneChange) { return; }
        EventCenter.FadeNotify(SceneType.StageSelect);
    }

    public void DeleteData()
    {
        _deleteHintText.enabled = true;
        DataManager.DeleteSaveFile("savedata.json");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}