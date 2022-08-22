public abstract class Singleton<T> where T : new()
{
    private static T _instance;
    private static object mutex = new object();

    public static T Instance{
        get{
            if(_instance == null){
                //同時複数訪問防止、唯一性確保
                lock(mutex){
                    if(_instance == null){
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}


