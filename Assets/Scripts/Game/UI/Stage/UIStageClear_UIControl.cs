using System;
using UnityEngine;
using UnityEngine.UI;
using GameEnumList;

public class UIStageClear_UIControl : UIControl {
    //色、位置変更など必要なオブジェクト
    [SerializeField] private Text _againText;
    [SerializeField] private Text _nextStageText;
    [SerializeField] private Text _stageSelectText;
    [SerializeField] private Text _clearTimeText;
    [SerializeField] private Text _getItemText;
    [SerializeField] private Image _highLightImage;
    [SerializeField] private Image _starImage;
    [SerializeField] private ParticleSystem _effectParticle;
    [SerializeField] private ParticleSystem _selectParticle;

    private enum StageClearUISelect { Again, NextStage, StageSelect }; //選択肢の種類
    private StageClearUISelect _currentSelect; //現在の選択
    private Animator _animator;
    private bool _enabled;
    private float _oldInput; //前フレームの入力

    private bool _isLastStage => (GameManager.CurrentScene + 1) == SceneType.NULLSCENE; //次のステージの存在確認

    private void OnEnable()
    {
        //EventCenterに登録
        EventCenter.AddUIListener(OnNotify);
    }

    private void OnDisable()
    {
        //登録を外す
        EventCenter.RemoveUIListener(OnNotify);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start() {
        TryGetComponent(out _animator);
        _againText = DictView["Text_Again"].GetComponent<Text>();
        _nextStageText = DictView["Text_NextStage"].GetComponent<Text>();
        _stageSelectText = DictView["Text_StageSelect"].GetComponent<Text>();
        _clearTimeText = DictView["Text_ClearTime"].GetComponent<Text>();
        _getItemText = DictView["Text_GetItem"].GetComponent<Text>();
        _highLightImage = DictView["Image_HighLight"].GetComponent<Image>();
        _starImage = DictView["Image_Star"].GetComponent<Image>();
        _effectParticle = DictView["Particle_Clear"].GetComponent<ParticleSystem>();
        _selectParticle = DictView["Particle_Select"].GetComponent<ParticleSystem>();

        //初期設定
        _currentSelect = StageClearUISelect.Again;
        _againText.color = Color.red;
        _nextStageText.color = _isLastStage ? Color.gray : Color.white;
        _stageSelectText.color = Color.white;
        _starImage.rectTransform.localPosition = new Vector2(_againText.rectTransform.localPosition.x - _againText.rectTransform.rect.width / 1.8f, _againText.rectTransform.localPosition.y);
        _highLightImage.rectTransform.localPosition = _againText.rectTransform.localPosition;
        _highLightImage.rectTransform.sizeDelta = _againText.rectTransform.rect.size;
        _selectParticle.gameObject.transform.position = _againText.gameObject.transform.position;
        var sh = _selectParticle.shape;
        sh.scale = new Vector3(1.8f, 1, 1);
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
            StageClearUISelect oldSelect = _currentSelect;
            //連続入力防止
            //最終ステージの場合、nextStageは灰色になる
            if (input > 0.8f && _oldInput < 0.8f)
            {
                if (_isLastStage)
                    _currentSelect = StageClearUISelect.Again;
                else
                    _currentSelect = (StageClearUISelect)Mathf.Max((int)--_currentSelect, 0);
            }
            else if (input < -0.8f && _oldInput > -0.8f)
            {
                if (_isLastStage)
                    _currentSelect = StageClearUISelect.StageSelect;
                else
                    _currentSelect = (StageClearUISelect)Mathf.Min((int)++_currentSelect, 2);
            }

            if (_currentSelect == oldSelect) { return; }
            AudioManager.Instance.Play("UI", "UISelect", false);

            var sh = _selectParticle.shape;
            switch (_currentSelect)
            {
                case StageClearUISelect.Again:
                    if (_isLastStage)
                    {
                        _againText.color = Color.red;
                        _nextStageText.color = Color.gray;
                        _stageSelectText.color = Color.white;
                    }
                    else
                    {
                        _againText.color = Color.red;
                        _nextStageText.color = Color.white;
                    }
                    _starImage.rectTransform.localPosition = new Vector2(_againText.rectTransform.localPosition.x - _againText.rectTransform.rect.width / 1.8f, _againText.rectTransform.localPosition.y);
                    _highLightImage.rectTransform.localPosition = _againText.rectTransform.localPosition;
                    _highLightImage.rectTransform.sizeDelta = _againText.rectTransform.rect.size;
                    _selectParticle.gameObject.transform.position = _againText.gameObject.transform.position;
                    sh.scale = new Vector3(1.8f, 1, 1);
                    break;
                case StageClearUISelect.NextStage:
                    _againText.color = Color.white;
                    _nextStageText.color = Color.red;
                    _stageSelectText.color = Color.white;
                    _starImage.rectTransform.localPosition = new Vector2(_nextStageText.rectTransform.localPosition.x - _nextStageText.rectTransform.rect.width / 1.8f, _nextStageText.rectTransform.localPosition.y);
                    _highLightImage.rectTransform.localPosition = _nextStageText.rectTransform.localPosition;
                    _highLightImage.rectTransform.sizeDelta = _nextStageText.rectTransform.rect.size;
                    _selectParticle.gameObject.transform.position = _nextStageText.gameObject.transform.position;
                    sh.scale = new Vector3(3.6f, 1, 1);
                    break;
                case StageClearUISelect.StageSelect:
                    if (_isLastStage)
                    {
                        _againText.color = Color.white;
                        _nextStageText.color = Color.gray;
                        _stageSelectText.color = Color.red;
                    }
                    else
                    {
                        _nextStageText.color = Color.white;
                        _stageSelectText.color = Color.red;
                    }
                    _starImage.rectTransform.localPosition = new Vector2(_stageSelectText.rectTransform.localPosition.x - _stageSelectText.rectTransform.rect.width / 1.8f, _stageSelectText.rectTransform.localPosition.y);
                    _highLightImage.rectTransform.localPosition = _stageSelectText.rectTransform.localPosition;
                    _highLightImage.rectTransform.sizeDelta = _stageSelectText.rectTransform.rect.size;
                    _selectParticle.gameObject.transform.position = _stageSelectText.gameObject.transform.position;
                    sh.scale = new Vector3(4, 1, 1);
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
            AudioManager.Instance.Play("UI", "UISubmit", false);
            switch (_currentSelect)
            {
                case StageClearUISelect.Again:
                    Again();
                    break;
                case StageClearUISelect.NextStage:
                    NextStage();
                    break;
                case StageClearUISelect.StageSelect:
                    StageSelect();
                    break;
            }
        }
    }

    /// <summary>
    /// 現在のステージを再ロードする
    /// </summary>
    private void Again()
    {
        EventCenter.FadeNotify(GameManager.CurrentScene);
    }

    /// <summary>
    /// 次のステージに移行する
    /// </summary>
    private void NextStage()
    {
        if (_isLastStage)
        {
            EventCenter.FadeNotify(SceneType.StageSelect);
        }
        else
        {
            EventCenter.FadeNotify(GameManager.CurrentScene + 1);
        }
    }

    /// <summary>
    /// セレクト画面に移行する
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
        _clearTimeText.text = s;
        _getItemText.text = info.SecretItemCount.ToString() + " / " + info.SecretItemMaxCount.ToString();
    }

    /// <summary>
    /// UI動画確認
    /// </summary>
    public void ClearUIAnimationEnd()
    {
        _enabled = !_enabled;
    }

    /// <summary>
    /// エフェクトPlay
    /// </summary>
    public void PlayClearParticle()
    {
        _effectParticle.Play();
    }

    /// <summary>
    /// EventCenterから呼び出す関数
    /// </summary>
    /// <param name="uiName">ui名</param>
    public void OnNotify(string uiName)
    {
        if (uiName != "ClearUI") { return; }
        if (GameManager.Pause) { return; }

        GameManager.Pause = true;
        AudioManager.Instance.Play("BackGround", "BGMClear", false);
        StageDataManager.Instance.SaveStageClearData(); //クリアデータをセーブする
        _animator.Play("OpenStageClear", 0); //UIアニメション
        UpdateStageInfoUI();
        _selectParticle.Play();
    }
}