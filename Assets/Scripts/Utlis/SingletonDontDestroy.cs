using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//싱클턴 : 정적 변수로 바꿔주는것
public class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance = null;
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
