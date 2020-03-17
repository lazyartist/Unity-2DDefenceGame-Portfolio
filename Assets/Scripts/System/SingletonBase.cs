using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T: MonoBehaviour
{
    static private T _inst;
    static public T Inst
    {
        get{
            if (_inst == null)
            {
                GameObject go = new GameObject();
                _inst = go.AddComponent<T>();
                _inst.name = typeof(T).Name;
                Debug.Log("SingletonBase Create " + _inst);
            }
            return _inst;
        }
        private set { }
    }

    virtual protected void Awake()
    {
        if (_inst == null)
        {
            _inst = this.GetComponent<T>();
            Debug.Log("SingletonBase Awake " + _inst + " " + Time.time);
        } 
        else
        {
            Debug.Log("Destroy Singleton " + _inst + ", " + this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
