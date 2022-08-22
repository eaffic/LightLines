using UnityEngine;

/// <summary>
/// Unityシングルトーン
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;

                //現在のシーンの中に存在していない場合、新しいオブジェクトを作る
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = (T)obj.AddComponent(typeof(T)); //ソースコードを追加
                    obj.hideFlags = HideFlags.DontSave;
                    obj.name = typeof(T).Name;
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
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

    /// <summary>
    /// 削除
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}