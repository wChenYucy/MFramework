using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorSingleton<T> : MonoBehaviour where T : BehaviorSingleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject go = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
                    go.name = "ScriptsHolder";
                    instance = go.AddComponent<T>();
                }

                instance.Init();
            }

            return instance;
        }
    }
    
    protected virtual void Init(){}
}
