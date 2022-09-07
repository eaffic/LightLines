using UnityEngine;
using UnityEngine.UI;
using GameEnumList;

public class UIStageSelectMenu_UIControl : UIControl {
    //色、位置変更など必要なオブジェクト
    [SerializeField] private Text _returnText;
    [SerializeField] private Text _titleText;
    [SerializeField] private Image _highLightImage;
    [SerializeField] private Image _starImage;
    [SerializeField] private ParticleSystem _selectParticle;

    private enum StageSelectMenuUISelect { Return, Title }; //選択肢の種類
    private StageSelectMenuUISelect _currentSelect; //現在の選択
    private Animator _animator;
    private bool _enabled; //起動確認
    private float _oldInput; //前回の選択

    private void OnEnable()
    {
        //起動時、EventCenterに登録する
        EventCenter.AddUIListener(OnNotify);
    }

    private void OnDisable()
    {
        //終了時、登録を外す
        EventCenter.RemoveUIListener(OnNotify);
    }

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out _animator);
    }

    private void Start() {
        _returnText = DictView["Text_Return"].GetComponent<Text>();
        _titleText = DictView["Text_Title"].GetComponent<Text>();
        _highLightImage = DictView["Image_HighLight"].GetComponent<Image>();
        _starImage = DictView["Image_Star"].GetComponent<Image>();
        _selectParticle = DictView["Particle_Select"].GetComponent<ParticleSystem>();

        //初期設定
        _currentSelect = StageSelectMenuUISelect.Return;
        _returnText.color = Color.red;
        _titleText.color = Color.white;
        _highLightImage.rectTransform.localPosition = _returnText.rectTransform.localPosition;
        _highLightImage.rectTransform.sizeDelta = _returnText.rectTransform.rect.size;
        _selectParticle.gameObject.transform.position = _returnText.gameObject.transform.position;
        var sh = _selectParticle.shape;
        sh.scale = new Vector3(2.4f, 1, 1);
    }

    private void Update()
    {
        //入力があると、特定のUIを呼び出す
        //TODO UI管理システムの構築
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
            StageSelectMenuUISelect oldSelect = _currentSelect;
            //連続入力防止
            if (input > 0.8f && _oldInput < 0.8f)
            {
                _currentSelect = (StageSelectMenuUISelect)Mathf.Max((int)--_currentSelect, 0);
            }
            else if (input < -0.8f && _oldInput > -0.8f)
            {
                _currentSelect = (StageSelectMenuUISelect)Mathf.Min((int)++_currentSelect, 1);
            }

            if (_currentSelect == oldSelect) { return; }
            AudioManager.Instance.Play("UI", "UISelect", false);

            var sh = _selectParticle.shape;
            //テキストの色変更
            switch (_currentSelect)
            {
                case StageSelectMenuUISelect.Return:
                    _returnText.color = Color.red;
                    _titleText.color = Color.white;
                    _starImage.rectTransform.localPosition = new Vector2(_returnText.rectTransform.localPosition.x - _returnText.rectTransform.rect.width / 1.8f, _returnText.rectTransform.localPosition.y);

                    _highLightImage.rectTransform.localPosition = _returnText.rectTransform.localPosition;
                    _highLightImage.rectTransform.sizeDelta = _returnText.rectTransform.rect.size;

                    _selectParticle.gameObject.transform.position = _returnText.gameObject.transform.position;
                    sh.scale = new Vector3(2.4f, 1, 1);
                    break;
                case StageSelectMenuUISelect.Title:
                    _returnText.color = Color.white;
                    _titleText.color = Color.red;
                    _starImage.rectTransform.localPosition = new Vector2(_titleText.rectTransform.localPosition.x - _titleText.rectTransform.rect.width / 1.8f, _titleText.rectTransform.localPosition.y);
                    
                    _highLightImage.rectTransform.localPosition = _titleText.rectTransform.localPosition;
                    _highLightImage.rectTransform.sizeDelta = _titleText.rectTransform.rect.size;

                    _selectParticle.gameObject.transform.position = _titleText.gameObject.transform.position;
                    sh.scale = new Vector3(1.5f, 1, 1);
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
                case StageSelectMenuUISelect.Return:
                    AudioManager.Instance.Play("UI", "UICancel", false);
                    Return();
                    break;
                case StageSelectMenuUISelect.Title:
                    AudioManager.Instance.Play("UI", "UISubmit", false);
                    Title();
                    break;
            }
        }
    }

    /// <summary>
    /// ゲーム画面に戻る
    /// </summary>
    private void Return()
    {
        GameManager.Pause = false;
        _animator.Play("CloseMenuUI", 0);
        _selectParticle.Stop();
    }

    /// <summary>
    /// タイトルに移行する
    /// </summary>
    private void Title()
    {
        EventCenter.FadeNotify(SceneType.Title);
    }

    /// <summary>
    /// アニメションの終了確認
    /// </summary>
    public void MenuUIAnimationEnd()
    {
        _enabled = !_enabled;
    }


    public void OnNotify(string uiName)
    {
        if (uiName != "Menu") { return; }
        if (GameManager.Pause) { return; }

        AudioManager.Instance.Play("UI", "UIOpenMenu", false);
        GameManager.Pause = true;
        _selectParticle.Play();
        _animator.Play("OpenMenuUI");
    }
}