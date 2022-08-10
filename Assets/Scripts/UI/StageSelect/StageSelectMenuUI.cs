using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEnumList;

public class StageSelectMenuUI : MonoBehaviour
{
    [SerializeField] private Text _returnText;
    [SerializeField] private Text _titleText;
    [SerializeField] private Image _starIcon;

    private enum StageSelectMenuUISelect { Return, Title };
    private StageSelectMenuUISelect _currentSelect;
    private Animator _animator;
    private bool _enabled;
    private float _oldInput;

    private void OnEnable()
    {
        EventCenter.AddUIListener(Notify);
    }

    private void OnDisable()
    {
        EventCenter.RemoveUIListener(Notify);
    }

    private void Awake()
    {
        TryGetComponent(out _animator);
    }

    private void Update()
    {
        if (GameInputManager.Instance.GetUIMenuInput())
        {
            EventCenter.UINotify("Menu");
        }

        if (_enabled == false) { return; }
        _starIcon.transform.Rotate(Vector3.forward);

        UpdateCursor();
        Submit();
    }

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
            AudioManager.Instance.Play("UI", "Select", false);

            switch (_currentSelect)
            {
                case StageSelectMenuUISelect.Return:
                    _returnText.color = Color.red;
                    _titleText.color = Color.white;
                    _starIcon.rectTransform.localPosition = new Vector2(_returnText.rectTransform.localPosition.x - _returnText.rectTransform.rect.width / 1.8f, _returnText.rectTransform.localPosition.y);
                    break;
                case StageSelectMenuUISelect.Title:
                    _returnText.color = Color.white;
                    _titleText.color = Color.red;
                    _starIcon.rectTransform.localPosition = new Vector2(_titleText.rectTransform.localPosition.x - _titleText.rectTransform.rect.width / 1.8f, _titleText.rectTransform.localPosition.y);
                    break;
            }
        }

        _oldInput = input;
    }

    private void Submit()
    {
        if(GameInputManager.Instance.GetUISubmitInput()){
            switch(_currentSelect){
                case StageSelectMenuUISelect.Return:
                    Return();
                    break;
                case StageSelectMenuUISelect.Title:
                    Title();
                    break;
            }
        }
    }

    private void Return()
    {
        AudioManager.Instance.Play("UI", "Cancel", false);
        GameManager.Pause = false;
        _animator.Play("CloseMenuUI", 0);
    }

    private void Title()
    {
        AudioManager.Instance.Play("UI", "Submit", false);
        EventCenter.FadeNotify(SceneType.Title);
    }

    public void MenuUIAnimationEnd()
    {
        _enabled = !_enabled;
    }


    public void Notify(string uiName)
    {
        if (uiName != "Menu") { return; }
        if (GameManager.Pause) { return; }

        AudioManager.Instance.Play("UI", "OpenMenu", false);
        GameManager.Pause = true;
        _currentSelect = StageSelectMenuUISelect.Return;
        _returnText.color = Color.red;
        _titleText.color = Color.white;
        _animator.Play("OpenMenuUI", 0);
    }
}
