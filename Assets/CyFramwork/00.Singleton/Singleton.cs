/// <summary>
/// 普通类单例基类
/// 线程不安全
/// </summary>
/// <typeparam name="T">类型</typeparam>
public class Singleton<T> where T : Singleton<T>, new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }

            return instance;
        }
    }
    
    private Singleton(){}
}