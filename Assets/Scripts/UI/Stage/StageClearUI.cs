using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEnumList;

public class StageClearUI : MonoBehaviour {
    [SerializeField] private Text _againText;
    [SerializeField] private Text _nextStageText;
    [SerializeField] private Text _stageSelectText;
    [SerializeField] private Text _clearTimeText;
    [SerializeField] private Text _getItemText;
    [SerializeField] private Image _starIcon;
    [SerializeField] private ParticleSystem _effectParticle;

    private enum StageClearUISelect { Again, NextStage, StageSelect };
    private StageClearUISelect _currentSelect;
    private Animator _animator;
    private bool _enabled;
    private float _oldInput;

    private bool _isLastStage => (GameManager.CurrentScene + 1) == SceneType.NULLSCENE;

    private void OnEnable() {
        EventCenter.AddUIListener(Notify);
    }

    private void OnDisable() {
        EventCenter.RemoveUIListener(Notify);
    }

    private void Awake() {
        TryGetComponent(out _animator);
    }

    private void Update() {
        if (_enabled == false) { return; }
        _starIcon.transform.Rotate(Vector3.forward);

        UpdateCursor();
        Submit();
    }

    private void UpdateCursor(){
        float input;
        if(GameInputManager.Instance.GetUISelectInput(out input)){
            StageClearUISelect oldSelect = _currentSelect;
            //連続入力防止
            //最終ステージの場合、nextStageは灰色になる
            if(input > 0.8f && _oldInput < 0.8f){
                if(_isLastStage)
                    _currentSelect = StageClearUISelect.Again;
                else
                    _currentSelect = (StageClearUISelect)Mathf.Max((int)--_currentSelect, 0);
            }
            else if(input < -0.8f && _oldInput > -0.8f){
                if(_isLastStage)
                    _currentSelect = StageClearUISelect.StageSelect;
                else
                    _currentSelect = (StageClearUISelect)Mathf.Min((int)++_currentSelect, 2);
            }

            if (_currentSelect == oldSelect) { return; }
            AudioManager.Instance.Play("UI", "Select", false);

            switch(_currentSelect){
                case StageClearUISelect.Again:
                    if(_isLastStage)
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
                    _starIcon.rectTransform.localPosition = new Vector2(_againText.rectTransform.localPosition.x - _againText.rectTransform.rect.width / 1.8f, _againText.rectTransform.localPosition.y);
                    break;
                case StageClearUISelect.NextStage:
                    _againText.color = Color.white;
                    _nextStageText.color = Color.red;
                    _stageSelectText.color = Color.white;
                    _starIcon.rectTransform.localPosition = new Vector2(_nextStageText.rectTransform.localPosition.x - _nextStageText.rectTransform.rect.width / 1.8f, _nextStageText.rectTransform.localPosition.y);
                    break;
                case StageClearUISelect.StageSelect:
                    if(_isLastStage){
                        _againText.color = Color.white;
                        _nextStageText.color = Color.gray;
                        _stageSelectText.color = Color.red;
                    }
                    else
                    {
                        _nextStageText.color = Color.white;
                        _stageSelectText.color = Color.red;
                    }
                    _starIcon.rectTransform.localPosition = new Vector2(_stageSelectText.rectTransform.localPosition.x - _stageSelectText.rectTransform.rect.width / 1.8f, _stageSelectText.rectTransform.localPosition.y);
                    break;
            }

        }

        _oldInput = input;
    }

    private void Submit(){
        if(GameInputManager.Instance.GetUISubmitInput()){
            AudioManager.Instance.Play("UI", "Submit", false);
            switch(_currentSelect){
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

    #region 選択
    private void Again(){
        EventCenter.FadeNotify(GameManager.CurrentScene);
    }

    private void NextStage(){
        if(_isLastStage){
            EventCenter.FadeNotify(SceneType.StageSelect);
        }else{
            EventCenter.FadeNotify(GameManager.CurrentScene + 1);
        }
    }

    private void StageSelect(){
        EventCenter.FadeNotify(SceneType.StageSelect);
    }
    #endregion

    private void UpdateStageInfoUI(){
        StageInfo info = StageDataManager.Instance.GetCurrentStageInfo();
        string s = String.Format("{0:00}:{1:00}:{2:00}",
            (int)info.ClearTime / 60,
            (int)info.ClearTime % 60,
            (info.ClearTime - (int)info.ClearTime) * 100);
        _clearTimeText.text = s;
        _getItemText.text = info.SecretItemCount.ToString() + " / " + info.SecretItemMaxCount.ToString();
    }

    public void ClearUIAnimationEnd(){
        _enabled = !_enabled;
    }

    public void PlayClearParticle(){
        _effectParticle.Play();
    }

    public void Notify(string uiName){
        if (uiName != "ClearUI") { return; }
        if (GameManager.Pause) { return; }

        GameManager.Pause = true;
        AudioManager.Instance.Play("BackGround", "Clear", false);
        StageDataManager.Instance.SaveStageClearData();
        _animator.Play("OpenClearUI", 0);
        _currentSelect = StageClearUISelect.Again;
        _againText.color = Color.red;
        _nextStageText.color = _isLastStage ? Color.gray : Color.white;
        _stageSelectText.color = Color.white;
        _starIcon.rectTransform.localPosition = new Vector2(_againText.rectTransform.localPosition.x - _againText.rectTransform.rect.width / 1.8f, _againText.rectTransform.localPosition.y);
        UpdateStageInfoUI();
    }
}