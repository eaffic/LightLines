using UnityEngine;

/// <summary>
/// シングルトーン
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get { return _instance; }
    }

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        else{
            _instance = (T)this;
        }
    }

    /// <summary>
    /// 初期化確認
    /// </summary>
    /// <value></value>
    public static bool IsInitialized
    {
        get { return _instance != null; }
    }

    protected virtual void OnDestroy() {
        if(_instance == this)
        {
            _instance = null;
        }
    }
}