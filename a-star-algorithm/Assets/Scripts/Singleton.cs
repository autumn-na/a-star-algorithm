using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            _instance = FindObjectOfType(typeof(T)) as T;

            if (_instance == null)
            {
                _instance = new GameObject(typeof(T).ToString(), typeof(T)).AddComponent<T>();
            }

            return _instance;
        }
    }
}