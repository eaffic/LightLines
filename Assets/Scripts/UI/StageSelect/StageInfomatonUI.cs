using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEnumList;

public class StageInfomatonUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _timeText;
    [SerializeField] private Text _itemText;
    [SerializeField] private Text _startText;
    [SerializeField] private Text _cancelText;
    [SerializeField] private Image _starIcon;
    [SerializeField] private Image _stageImage;
    [SerializeField] private List<Image> _stageImageList = new List<Image>();

    private enum StageUISelect { Start, Cancel };
    private StageUISelect _currentSelect;
    private SceneType _targetScene;
    private StageInfo _stageInfo;
    private bool _enabled;

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

        _currentSelect = StageUISelect.Start;
        _starIcon.rectTransform.localPosition = new Vector2(_startText.rectTransform.localPosition.x - _startText.rectTransform.rect.width / 1.8f, _startText.transform.localPosition.y);

        _startText.color = Color.red;
    }

    private void Update()
    {
        if (_enabled == false) { return; }

        _starIcon.transform.Rotate(Vector3.forward);

        float input;
        if (GameInputManager.Instance.GetUISelectInput(out input))
        {
            if (input > 0)
            {
                _currentSelect = (StageUISelect)Mathf.Max((int)--_currentSelect, 0);
            }
            else
            {
                _currentSelect = (StageUISelect)Mathf.Min((int)++_currentSelect, 1);
            }

            switch (_currentSelect)
            {
                case StageUISelect.Start:
                    _startText.color = Color.red;
                    _cancelText.color = Color.white;
                    _starIcon.rectTransform.localPosition = new Vector2(_startText.rectTransform.localPosition.x - _startText.rectTransform.rect.width / 1.8f, _startText.transform.localPosition.y);
                    break;
                case StageUISelect.Cancel:
                    _startText.color = Color.white;
                    _cancelText.color = Color.red;
                    _starIcon.rectTransform.localPosition = new Vector2(_cancelText.rectTransform.localPosition.x - _cancelText.rectTransform.rect.width / 1.8f, _cancelText.transform.localPosition.y);
                    break;
            }
        }

        if(GameInputManager.Instance.GetUISubmitInput()){
            switch (_currentSelect){
                case StageUISelect.Start:
                    ToNewStage();
                    break;
                case StageUISelect.Cancel:
                    _animator.SetBool("Enable", false);
                    GameManager.OpenMenu = false;
                    break;
            }
        }
    }

    private void ToNewStage()
    {
        EventCenter.FadeNotify(_targetScene);
    }

    public void UpdateInfomation(SceneType scene)
    {
        GameManager.OpenMenu = true;
        _targetScene = scene;
        _stageInfo = DataManager.GetStageInfo(scene);
        //_stageImage.sprite = _sourceSprite;
        _titleText.text = scene.ToString();
        string s = String.Format("{0:00}:{1:00}:{2:00}",
            (int)_stageInfo.ClearTime / 60,
            (int)_stageInfo.ClearTime % 60,
            (_stageInfo.ClearTime - (int)_stageInfo.ClearTime) * 100);
        _timeText.text = s;
        _itemText.text = _stageInfo.SecretItemCount.ToString() + " / " + _stageInfo.SecretItemMaxCount.ToString();
        _animator.SetBool("Enable", true);
    }

    public void UIAnimationEnd(){
        _enabled = !_enabled;
    }
}