using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//싱클턴 : 정적 변수로 바꿔주는것
public class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    public static T Inst
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DoAwake();
    }

    protected virtual void DoAwake() 
    {
    
    }
}
