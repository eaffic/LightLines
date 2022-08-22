using System;
using UnityEngine;
using UnityEngine.UI;
using GameEnumList;

public class UIStageInformation_UIControl : UIControl {
    //色、位置変更など必要なオブジェクト
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _clearTimeText;
    [SerializeField] private Text _getItemText;
    [SerializeField] private Text _startText;
    [SerializeField] private Text _cancelText;
    [SerializeField] private Image _starIcon;
    [SerializeField] private Image _stageImage;
    //[SerializeField] private Sprite[] _stageImageList; //ステージ内容を簡単表示用のプレビュー画像
    //TODO プレビュー画像ではなく、動画表示をする

    private enum StageSelectUISelect { Start, Cancel };
    private StageSelectUISelect _currentSelect;
    private SceneType _targetScene;
    private bool _enabled;
    private float _oldInput;

    private Animator _animator;

    private void OnEnable()
    {
        //起動時、EventCenterに登録する
        EventCenter.AddStageInfomationListener(OnNotify);
    }

    private void OnDisable()
    {
        //終了時、登録を外す
        EventCenter.AddStageInfomationListener(OnNotify);
    }

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out _animator);
    }

    private void Start() {
        _titleText = DictView["Text_Title"].GetComponent<Text>();
        _clearTimeText = DictView["Text_ClearTime"].GetComponent<Text>();
        _getItemText = DictView["Text_GetItme"].GetComponent<Text>();
        _startText = DictView["Text_Start"].GetComponent<Text>();
        _cancelText = DictView["Text_Cancel"].GetComponent<Text>();
        _starIcon = DictView["Image_Star"].GetComponent<Image>();
        _stageImage = DictView["Image_Stage"].GetComponent<Image>();

        _currentSelect = StageSelectUISelect.Start;
        _startText.color = Color.red;
        _cancelText.color = Color.white;
        _starIcon.rectTransform.localPosition = new Vector2(_startText.rectTransform.localPosition.x - _startText.rectTransform.rect.width / 1.8f, _startText.transform.localPosition.y);
    }

    private void Update()
    {
        //TODO UI管理システムの構築
        if (_enabled == false) { return; }
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
        if (GameInputManager.Instance.GetUISelectInput(out input))
        {
            StageSelectUISelect oldSelect = _currentSelect;
            //連続入力防止
            if (input > 0.8f && _oldInput < 0.8f)
            {
                _currentSelect = (StageSelectUISelect)Mathf.Max((int)--_currentSelect, 0);
            }
            else if (input < -0.8f && _oldInput > -0.8f)
            {
                _currentSelect = (StageSelectUISelect)Mathf.Min((int)++_currentSelect, 1);
            }

            if (_currentSelect == oldSelect) { return; }
            AudioManager.Instance.Play("UI", "UISelect", false);

            switch (_currentSelect)
            {
                case StageSelectUISelect.Start:
                    _startText.color = Color.red;
                    _cancelText.color = Color.white;
                    _starIcon.rectTransform.localPosition = new Vector2(_startText.rectTransform.localPosition.x - _startText.rectTransform.rect.width / 1.8f, _startText.transform.localPosition.y);
                    break;
                case StageSelectUISelect.Cancel:
                    _startText.color = Color.white;
                    _cancelText.color = Color.red;
                    _starIcon.rectTransform.localPosition = new Vector2(_cancelText.rectTransform.localPosition.x - _cancelText.rectTransform.rect.width / 1.8f, _cancelText.transform.localPosition.y);
                    break;
            }
        }

        _oldInput = input;
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
                case StageSelectUISelect.Start:
                    AudioManager.Instance.Play("UI", "UISubmit", false);
                    ToNewStage();
                    break;
                case StageSelectUISelect.Cancel:
                    AudioManager.Instance.Play("UI", "UICancel", false);
                    Cancel();
                    break;
            }
        }
    }

    /// <summary>
    /// 目標ステージに移行
    /// </summary>
    private void ToNewStage()
    {
        EventCenter.FadeNotify(_targetScene);
    }

    /// <summary>
    /// ゲームに戻る
    /// </summary>
    private void Cancel()
    {
        _animator.Play("CloseMenu", 0);
        GameManager.Pause = false;
    }

    /// <summary>
    /// ステージ選択の情報パネル更新
    /// </summary>
    private void UpdateStageInfomation()
    {
        StageInfo info = DataManager.GetStageInfo(_targetScene);
        _stageImage.sprite = Resources.Load<Sprite>("Textures/UI/Stage/" + _targetScene.ToString());
        _titleText.text = _targetScene.ToString();
        string s = String.Format("{0:00}:{1:00}:{2:00}",
            (int)info.ClearTime / 60,
            (int)info.ClearTime % 60,
            (info.ClearTime - (int)info.ClearTime) * 100);
        _clearTimeText.text = s;
        _getItemText.text = info.SecretItemCount.ToString() + " / " + info.SecretItemMaxCount.ToString();
    }

    /// <summary>
    /// EventCenterから呼び出すの関数
    /// </summary>
    /// <param name="scene"></param>
    public void OnNotify(SceneType scene)
    {
        //メニュー表示中、または閉じるの動画中の場合キャンセル
        if (_enabled) { return; }

        _animator.Play("OpenMenu", 0);

        AudioManager.Instance.Play("UI", "UIOpenMenu", false);
        GameManager.Pause = true;
        _targetScene = scene;
        UpdateStageInfomation();
    }

    /// <summary>
    /// UI動画の確認
    /// </summary>
    public void UIAnimationEnd()
    {
        _enabled = !_enabled;
    }
}