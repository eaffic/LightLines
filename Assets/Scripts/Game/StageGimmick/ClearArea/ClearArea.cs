using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// クリア魔法陣
/// </summary>
public class ClearArea : MonoBehaviour
{
    [SerializeField] private GameObject _magicCircle; //魔法陣
    [SerializeField] private PlayableDirector _timeline; //魔法陣動画

    private bool _isOpen;

    private void OnEnable() {
        EventCenter.AddStageClearListener(Notify);
    }

    private void OnDisable() {
        EventCenter.RemoveStageClearListener(Notify);
    }

    void Start()
    {
        TryGetComponent(out _timeline);
    }

#region 接触処理
    private void OnTriggerEnter(Collider other)
    {
        if (_isOpen)
        {
            if (other.gameObject.tag == "Player")
            {
                EventCenter.UINotify("ClearUI");
            }
        }
    }
    #endregion

    /// <summary>
    /// EventCenterから呼び出す
    /// </summary>
    /// <param name="check"></param>
    public void Notify(bool check)
    {
        //状態切り替え時のみ実行
        if(_isOpen != check)
        {
            _isOpen = check;
            if(_isOpen)
            {
                _timeline.enabled = true;
                AudioManager.Instance.Play("ClearArea", "ClearAreaOpen", false);
            }
            else{
                _timeline.Stop();
                _timeline.enabled = false;
                _magicCircle.SetActive(false);
                AudioManager.Instance.Play("ClearArea", "ClearAreaClose", false);
            }
        }
    }
}
