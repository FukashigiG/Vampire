using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SM_DontDestOnLand<T> : MonoBehaviour where T : MonoBehaviour
{
    protected abstract bool dontDestroyOnLoad { get; }

    static T instance_;

    public static T Instance
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = FindFirstObjectByType<T>();
            }

            return instance_ ?? new GameObject(typeof(T).FullName).AddComponent<T>();
        }
    }

    protected virtual void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}