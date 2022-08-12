using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GameEnumList;

public class StageMenuUI : MonoBehaviour {
    //色、位置変更など必要なオブジェクト
    [SerializeField] private Text _returnText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Text _stageSelectText;
    [SerializeField] private Text _clearTimeText;
    [SerializeField] private Text _getItemText;
    [SerializeField] private Image _starIcon;

    private enum StageMenuUISelect { Return, Restart, StageSelect }; //選択肢の種類
    private StageMenuUISelect _currentSelect; //現在の選択
    private Animator _animator;
    private bool _enabled;
    private float _oldInput; //前フレームの入力

    private void Awake() {
        TryGetComponent(out _animator);
    }

    private void OnEnable() {
        //EventCenterに登録
        EventCenter.AddUIListener(OnNotify);
    }

    private void OnDisable() {
        //登録を外す
        EventCenter.RemoveUIListener(OnNotify);
    }

    private void Update() {
        //TODO UI管理システムの構築
        if(GameInputManager.Instance.GetUIMenuInput()){
            EventCenter.UINotify("Menu");
        }

        if (_enabled == false) { return; }
        _starIcon.transform.Rotate(Vector3.forward);

        UpdateCursor();
        Submit();
    }

    /// <summary>
    /// 選択更新
    /// </summary>
    private void UpdateCursor(){
        float input;
        if(GameInputManager.Instance.GetUISelectInput(out input)){
            StageMenuUISelect oldSelect = _currentSelect;
            //連続入力防止
            if(input > 0.8f && _oldInput < 0.8f){
                _currentSelect = (StageMenuUISelect)Mathf.Max((int)--_currentSelect, 0);
            }else if(input < -0.8f && _oldInput > -0.8f){
                _currentSelect = (StageMenuUISelect)Mathf.Min((int)++_currentSelect, 2);
            }

            if (_currentSelect == oldSelect) { return; }
            AudioManager.Instance.Play("UI", "Select", false);

            switch(_currentSelect){
                case StageMenuUISelect.Return:
                    _returnText.color = Color.red;
                    _restartText.color = Color.white;
                    _starIcon.rectTransform.localPosition = new Vector2(_returnText.rectTransform.localPosition.x - _returnText.rectTransform.rect.width / 1.8f, _returnText.rectTransform.localPosition.y);
                    break;
                case StageMenuUISelect.Restart:
                    _returnText.color = Color.white;
                    _restartText.color = Color.red;
                    _stageSelectText.color = Color.white;
                    _starIcon.rectTransform.localPosition = new Vector2(_restartText.rectTransform.localPosition.x - _restartText.rectTransform.rect.width / 1.8f, _restartText.rectTransform.localPosition.y);
                    break;
                case StageMenuUISelect.StageSelect:
                    _restartText.color = Color.white;
                    _stageSelectText.color = Color.red;
                    _starIcon.rectTransform.localPosition = new Vector2(_stageSelectText.rectTransform.localPosition.x - _stageSelectText.rectTransform.rect.width / 1.8f, _stageSelectText.rectTransform.localPosition.y);
                    break;
            }
        }

        _oldInput = input; //前フレームの入力記録
    }

    /// <summary>
    /// 確認ボタンを押す時、対応のメソッドを実行する
    /// </summary>
    private void Submit(){
        if (GameInputManager.Instance.GetUISubmitInput())
        {
            switch (_currentSelect)
            {
                case StageMenuUISelect.Return:
                    AudioManager.Instance.Play("UI", "Cancel", false);
                    Return();
                    break;
                case StageMenuUISelect.Restart:
                    AudioManager.Instance.Play("UI", "Submit", false);
                    Restart();
                    break;
                case StageMenuUISelect.StageSelect:
                    AudioManager.Instance.Play("UI", "Submit", false);
                    StageSelect();
                    break;
            }
        }
    }

    /// <summary>
    /// メニューを閉じる
    /// </summary>
    private void Return(){
        GameManager.Pause = false;
        _animator.Play("CloseMenuUI", 0);
    }

    /// <summary>
    /// 現在のシーンを再ロードする
    /// </summary>
    private void Restart(){
        EventCenter.FadeNotify(GameManager.CurrentScene);
    }

    /// <summary>
    /// ステージ選択画面に戻る
    /// </summary>
    private void StageSelect(){
        EventCenter.FadeNotify(SceneType.StageSelect);
    }

    /// <summary>
    /// ステージの情報パネル更新
    /// </summary>
    private void UpdateStageInfoUI(){
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
    public void MenuUIAnimationEnd(){
        _enabled = !_enabled;
    }

    /// <summary>
    /// EventCenterから呼び出す関数
    /// </summary>
    /// <param name="uiName">ui名</param>
    public void OnNotify(string uiName){
        if (uiName != "Menu") { return; }
        if (GameManager.Pause) { return; }

        AudioManager.Instance.Play("UI", "OpenMenu", false);
        GameManager.Pause = true;
        _currentSelect = StageMenuUISelect.Return;
        _starIcon.rectTransform.localPosition = new Vector2(_returnText.rectTransform.localPosition.x - _returnText.rectTransform.rect.width / 1.8f, _returnText.rectTransform.localPosition.y);
        _returnText.color = Color.red;
        _restartText.color = Color.white;
        _stageSelectText.color = Color.white;
        _animator.Play("OpenMenuUI", 0);
        UpdateStageInfoUI();
    }
}