using System;
using UnityEngine;
using UnityEngine.UI;
using GameEnumList;

/// <summary>
/// UI ステージ内のメニュー画面
/// </summary>
public class UIStageMenu_UIControl : UIControl {
    //色、位置変更など必要なオブジェクト
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _returnText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Text _stageSelectText;
    [SerializeField] private Text _clearTimeText;
    [SerializeField] private Text _getItemText;
    [SerializeField] private Image _highLightImage;
    [SerializeField] private Image _starImage;
    [SerializeField] private ParticleSystem _selectParticle;

    private enum StageMenuUISelect { Return, Restart, StageSelect }; //選択肢の種類
    private StageMenuUISelect _currentSelect; //現在の選択
    private Animator _animator;
    private bool _enabled; //起動確認
    private float _oldInput; //前フレームの入力

    private void OnEnable()
    {
        EventCenter.AddUIListener(OnNotify);
    }

    private void OnDisable()
    {
        EventCenter.RemoveUIListener(OnNotify);
    }

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out _animator);
    }

    private void Start() {
        _titleText = DictView["Text_Title"].GetComponent<Text>();
        _returnText = DictView["Text_Return"].GetComponent<Text>();
        _restartText = DictView["Text_Restart"].GetComponent<Text>();
        _stageSelectText = DictView["Text_StageSelect"].GetComponent<Text>();
        _clearTimeText = DictView["Text_ClearTime"].GetComponent<Text>();
        _getItemText = DictView["Text_GetItem"].GetComponent<Text>();
        _highLightImage = DictView["Image_HighLight"].GetComponent<Image>();
        _starImage = DictView["Image_Star"].GetComponent<Image>();
        _selectParticle = DictView["Particle_Select"].GetComponent<ParticleSystem>();

        //初期設定
        _currentSelect = StageMenuUISelect.Return;
        _starImage.rectTransform.localPosition = new Vector2(_returnText.rectTransform.localPosition.x - _returnText.rectTransform.rect.width / 1.8f, _returnText.rectTransform.localPosition.y);
        _returnText.color = Color.red;
        _restartText.color = Color.white;
        _stageSelectText.color = Color.white;
        _highLightImage.rectTransform.localPosition = _returnText.rectTransform.localPosition;
        _highLightImage.rectTransform.sizeDelta = _returnText.rectTransform.rect.size;
        _selectParticle.gameObject.transform.position = _returnText.gameObject.transform.position;
        
        var sh = _selectParticle.shape;
        sh.scale = new Vector3(2.4f, 1, 1);
    }

    private void Update()
    {
        if (_enabled == false) { return; }
        _starImage.transform.Rotate(Vector3.forward);

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
            StageMenuUISelect oldSelect = _currentSelect;
            //連続入力防止
            if (input > 0.8f && _oldInput < 0.8f)
            {
                _currentSelect = (StageMenuUISelect)Mathf.Max((int)--_currentSelect, 0);
            }
            else if (input < -0.8f && _oldInput > -0.8f)
            {
                _currentSelect = (StageMenuUISelect)Mathf.Min((int)++_currentSelect, 2);
            }

            if (_currentSelect == oldSelect) { return; }
            AudioManager.Instance.Play("UI", "UISelect", false);

            //選択に応じて設定変更
            var sh = _selectParticle.shape;
            switch (_currentSelect)
            {
                case StageMenuUISelect.Return:
                    _returnText.color = Color.red;
                    _restartText.color = Color.white;
                    _starImage.rectTransform.localPosition = new Vector2(_returnText.rectTransform.localPosition.x - _returnText.rectTransform.rect.width / 1.8f, _returnText.rectTransform.localPosition.y);
                    _highLightImage.rectTransform.localPosition = _returnText.rectTransform.localPosition;
                    _highLightImage.rectTransform.sizeDelta = _returnText.rectTransform.rect.size;
                    _selectParticle.gameObject.transform.position = _returnText.gameObject.transform.position;
                    sh.scale = new Vector3(2.4f, 1, 1);
                    break;
                case StageMenuUISelect.Restart:
                    _returnText.color = Color.white;
                    _restartText.color = Color.red;
                    _stageSelectText.color = Color.white;
                    _starImage.rectTransform.localPosition = new Vector2(_restartText.rectTransform.localPosition.x - _restartText.rectTransform.rect.width / 1.8f, _restartText.rectTransform.localPosition.y);
                    _highLightImage.rectTransform.localPosition = _restartText.rectTransform.localPosition;
                    _highLightImage.rectTransform.sizeDelta = _restartText.rectTransform.rect.size;
                    _selectParticle.gameObject.transform.position = _restartText.gameObject.transform.position;
                    sh.scale = new Vector3(2.6f, 1, 1);
                    break;
                case StageMenuUISelect.StageSelect:
                    _restartText.color = Color.white;
                    _stageSelectText.color = Color.red;
                    _starImage.rectTransform.localPosition = new Vector2(_stageSelectText.rectTransform.localPosition.x - _stageSelectText.rectTransform.rect.width / 1.8f, _stageSelectText.rectTransform.localPosition.y);
                    _highLightImage.rectTransform.localPosition = _stageSelectText.rectTransform.localPosition;
                    _highLightImage.rectTransform.sizeDelta = _stageSelectText.rectTransform.rect.size;
                    _selectParticle.gameObject.transform.position = _stageSelectText.gameObject.transform.position;
                    sh.scale = new Vector3(4f, 1, 1);
                    break;
            }
        }

        _oldInput = input; //前フレームの入力記録
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
                case StageMenuUISelect.Return:
                    AudioManager.Instance.Play("UI", "UICancel", false);
                    Return();
                    break;
                case StageMenuUISelect.Restart:
                    AudioManager.Instance.Play("UI", "UISubmit", false);
                    Restart();
                    break;
                case StageMenuUISelect.StageSelect:
                    AudioManager.Instance.Play("UI", "UISubmit", false);
                    StageSelect();
                    break;
            }
        }
    }

    /// <summary>
    /// メニューを閉じる
    /// </summary>
    private void Return()
    {
        GameManager.Pause = false;
        _animator.Play("CloseStageMenu", 0);
        _selectParticle.Stop();
    }

    /// <summary>
    /// 現在のシーンを再ロードする
    /// </summary>
    private void Restart()
    {
        EventCenter.FadeNotify(GameManager.CurrentScene);
    }

    /// <summary>
    /// ステージ選択画面に戻る
    /// </summary>
    private void StageSelect()
    {
        EventCenter.FadeNotify(SceneType.StageSelect);
    }

    /// <summary>
    /// ステージの情報パネル更新
    /// </summary>
    private void UpdateStageInfoUI()
    {
        StageInfo info = StageDataManager.Instance.GetCurrentStageInfo();
        string s = String.Format("{0:00}:{1:00}:{2:00}",
            (int)info.ClearTime / 60,
            (int)info.ClearTime % 60,
            (info.ClearTime - (int)info.ClearTime) * 100);
        _titleText.text = info.StageType.ToString();
        _clearTimeText.text = s;
        _getItemText.text = info.SecretItemCount.ToString() + " / " + info.SecretItemMaxCount.ToString();
    }

    /// <summary>
    /// UI動画確認
    /// </summary>
    public void MenuUIAnimationEnd()
    {
        _enabled = !_enabled;
    }

    /// <summary>
    /// EventCenterから呼び出す関数
    /// </summary>
    /// <param name="uiName">ui名</param>
    public void OnNotify(string uiName)
    {
        if (uiName != "Menu") { return; }
        if (GameManager.Pause) { return; }

        AudioManager.Instance.Play("UI", "UIOpenMenu", false);
        GameManager.Pause = true;
        _animator.Play("OpenStageMenu", 0);
        UpdateStageInfoUI();
        _selectParticle.Play();
    }
}