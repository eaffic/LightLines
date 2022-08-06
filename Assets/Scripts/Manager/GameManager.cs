using UnityEngine;
using GameEnumList;

public class GameManager : Singleton<GameManager>
{
    public static SceneType CurrentScene;

    private static bool _openMenu;
    public static bool OpenMenu { get => _openMenu; set => _openMenu = value; }

    private static bool _stageClear;
    public static bool StageClear { get => _stageClear; set => _stageClear = value; }

    private StageInfo _currentStageInfo;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update() {

    }
}