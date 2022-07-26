using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENEINDEX
{
    TITLE = 0,
    STAGESELECT = 1,
    STAGE1_1 = 2,
    STAGE1_2 = 3,
    STAGE1_3 = 4,
    STAGE1_4 = 5,
    STAGE1_5 = 6,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] public static SCENEINDEX _ActiveSceneIndex;

    public static GameManager _Instance;

    private void Awake()
    {
        if(_Instance != null)
        {
            Destroy(this);
            return;
        }
        _Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //シーン番号を取得する
        GameManager._ActiveSceneIndex = (SCENEINDEX)SceneManager.GetActiveScene().buildIndex;
        BGMAudioSetting();

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    /// <summary>
    /// 初期BGM設定
    /// </summary>
    void BGMAudioSetting()
    {
        switch (GameManager._ActiveSceneIndex)
        {
            case SCENEINDEX.TITLE:
                AudioManager.PlayTitleBGMAudio();
                break;
            case SCENEINDEX.STAGESELECT:
                AudioManager.PlaySelectBGMAudio();
                break;
            default:
                AudioManager.PlayStageBGMAudio();
                break;
        }
    }

    void OnActiveSceneChanged(Scene preScene, Scene nextScene)
    {
        //シーン番号を取得する
        GameManager._ActiveSceneIndex = (SCENEINDEX)SceneManager.GetActiveScene().buildIndex;
        BGMAudioSetting();
    }
}
