using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// 单例模板类,不继承自monobehavior的单例类可继承该类
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingLeton<T> where T : class, new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }
}