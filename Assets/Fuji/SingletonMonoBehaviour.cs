using UnityEngine;
using System;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    public static T Instance
    {
        private set;
        get;
    }
    //インスタンスが一つであると保証
    protected void Awake()
    {
        if(Instance == null)
        {
            Instance = this.GetComponent<T>();
        }else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
}