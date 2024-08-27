using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;


[System.Serializable]
public class DelegateList<T> 
{
    public List<T> List = new List<T>();

    public UnityEvent OnRemove = new UnityEvent();

    public UnityEvent OnAdd = new UnityEvent();


    public void Remove(T ObjectToRemove)
    {
        List.Remove(ObjectToRemove);
        OnRemove.Invoke();
    }


    public void Add(T ObjectToRemove)
    {
        List.Add(ObjectToRemove);
        OnAdd.Invoke();
    }
}
