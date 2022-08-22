using UnityEngine;
using UnityEngine.UI;
using GameEnumList;

public class UITitle_UIControl : UIControl {
    [SerializeField] private Text _startText;
    [SerializeField] private Text _deleteText;
    [SerializeField] private Text _deleteHintText;
    [SerializeField] private Text _exitText;
    [SerializeField] private Image _starIcon; //選択のカーソルマーク

    private enum TitleSelect { GameStart, DeleteData, ExitGame }; //選択肢の種類
    private TitleSelect _currentSelect = default; //現在の選択
    private float _oldinput; //前回の選択

    protected override void Awake() {
        base.Awake();
    }

    private void Start() {
        _startText = DictView["Text_Start"].GetComponent<Text>();
        _deleteText = DictView["Text_DeleteData"].GetComponent<Text>();
        _deleteHintText = DictView["Text_DeleteHint"].GetComponent<Text>();
        _exitText = DictView["Text_Exit"].GetComponent<Text>();
        _starIcon = DictView["Image_Star"].GetComponent<Image>();

        //初期色、位置設定
        _startText.color = Color.red;
        _deleteText.color = Color.white;
        _exitText.color = Color.white;
        _starIcon.rectTransform.localPosition = new Vector2(-_startText.rectTransform.rect.width / 1.8f, _startText.transform.localPosition.y);
    }

    private void Update()
    {
        //カーソルマークをゆっくり回転させる
        _starIcon.transform.Rotate(Vector3.forward);

        UpdateCursor();
        Submit();
    }

    /// <summary>
    /// 選択更新
    /// </summary>
    private void UpdateCursor()
    {
        float input;
        //入力がある場合、数値を記録し、判断に入り
        if (GameInputManager.Instance.GetUISelectInput(out input))
        {
            //前回の選択を記録
            TitleSelect oldSelect = _currentSelect;
            //連続入力防止
            if (input > 0.8f && _oldinput < 0.8f)
            {
                _currentSelect = (TitleSelect)Mathf.Max((int)--_currentSelect, 0);
                _deleteHintText.enabled = false;
            }
            else if (input < -0.8f && _oldinput > -0.8f)
            {
                _currentSelect = (TitleSelect)Mathf.Min((int)++_currentSelect, 2);
                _deleteHintText.enabled = false;
            }

            //同じものを選択した時、以下の変更設定は不要
            if (_currentSelect == oldSelect) { return; }
            AudioManager.Instance.Play("UI", "UISelect", false); //カーソルの移動音

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

        _oldinput = input; //このフレームの入力を記録する（一回スティックを戻す必要がある）
    }

    /// <summary>
    /// 確認ボタンを押す時、対応のメソッドを実行する
    /// </summary>
    private void Submit()
    {
        if (GameInputManager.Instance.GetUISubmitInput())
        {
            switch (_currentSelect)
            {
                case TitleSelect.GameStart:
                    AudioManager.Instance.Play("UI", "UISubmit", false);
                    GameStart();
                    break;
                case TitleSelect.DeleteData:
                    AudioManager.Instance.Play("UI", "UIDeleteData", false);
                    DeleteData();
                    break;
                case TitleSelect.ExitGame:
                    ExitGame();
                    break;
            }
        }
    }

    /// <summary>
    /// 指定のシーンに移行する
    /// </summary>
    private void GameStart()
    {
        EventCenter.FadeNotify(SceneType.StageSelect);
    }

    /// <summary>
    /// クリアデータを削除する
    /// </summary>
    private void DeleteData()
    {
        _deleteHintText.enabled = true;
        DataManager.DeleteSaveFile("savedata.json");
    }

    private void ExitGame()
    {
        //ゲーム終了
        Application.Quit();
    }
}