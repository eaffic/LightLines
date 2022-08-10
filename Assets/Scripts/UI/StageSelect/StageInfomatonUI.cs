using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEnumList;

public class StageInfomatonUI : MonoBehaviour
{
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _clearTimeText;
    [SerializeField] private Text _getItemText;
    [SerializeField] private Text _startText;
    [SerializeField] private Text _cancelText;
    [SerializeField] private Image _starIcon;
    [SerializeField] private Image _stageImage;
    [SerializeField] private Sprite[] _stageImageList;

    private enum StageSelectUISelect { Start, Cancel };
    private StageSelectUISelect _currentSelect;
    private SceneType _targetScene;
    private bool _enabled;
    private float _oldInput;

    private Animator _animator;

    private void OnEnable()
    {
        EventCenter.AddStageInfomationListener(UpdateInfomation);
    }

    private void OnDisable()
    {
        EventCenter.AddStageInfomationListener(UpdateInfomation);
    }

    private void Awake()
    {
        TryGetComponent(out _animator);

        _currentSelect = StageSelectUISelect.Start;
        _starIcon.rectTransform.localPosition = new Vector2(_startText.rectTransform.localPosition.x - _startText.rectTransform.rect.width / 1.8f, _startText.transform.localPosition.y);

        _startText.color = Color.red;
        _cancelText.color = Color.white;
    }

    private void Update()
    {
        if (_enabled == false) { return; }

        _starIcon.transform.Rotate(Vector3.forward);

        UpdateCursor();
        Submit();
    }

    private void UpdateCursor(){
        float input;
        if (GameInputManager.Instance.GetUISelectInput(out input))
        {
            StageSelectUISelect oldSelect = _currentSelect;
            //連続入力防止
            if (input > 0.8f && _oldInput < 0.8f)
            {
                _currentSelect = (StageSelectUISelect)Mathf.Max((int)--_currentSelect, 0);
            }
            else if(input < -0.8f && _oldInput > -0.8f)
            {
                _currentSelect = (StageSelectUISelect)Mathf.Min((int)++_currentSelect, 1);
            }

            if (_currentSelect == oldSelect) { return; }
            AudioManager.Instance.Play("UI", "Select", false);

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

    private void Submit(){
        if (GameInputManager.Instance.GetUISubmitInput())
        {
            switch (_currentSelect)
            {
                case StageSelectUISelect.Start:
                    AudioManager.Instance.Play("UI", "Submit", false);
                    ToNewStage();
                    break;
                case StageSelectUISelect.Cancel:
                    AudioManager.Instance.Play("UI", "Cancel", false);
                    Cancel();
                    break;
            }
        }
    }

    private void ToNewStage()
    {
        EventCenter.FadeNotify(_targetScene);
    }

    private void Cancel()
    {
        _animator.Play("CloseMenu", 0);
        GameManager.Pause = false;
    }

    public void UpdateInfomation(SceneType scene)
    {
        //メニュー表示中、または閉じるの動画中の場合キャンセル
        if (_enabled) { return; }

        _animator.Play("OpenMenu", 0);
        
        AudioManager.Instance.Play("UI", "OpenMenu", false);
        GameManager.Pause = true;
        _targetScene = scene;
        StageInfo info = DataManager.GetStageInfo(scene);
        _stageImage.sprite = Array.Find<Sprite>(_stageImageList, sprite => sprite.name == scene.ToString());
        _titleText.text = scene.ToString();
        string s = String.Format("{0:00}:{1:00}:{2:00}",
            (int)info.ClearTime / 60,
            (int)info.ClearTime % 60,
            (info.ClearTime - (int)info.ClearTime) * 100);
        _clearTimeText.text = s;
        _getItemText.text = info.SecretItemCount.ToString() + " / " + info.SecretItemMaxCount.ToString();
    }

    public void UIAnimationEnd(){
        _enabled = !_enabled;
    }
}