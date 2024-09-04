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
        Debug.Log("removing");

        List.Remove(ObjectToRemove);

        List.RemoveAll(item => item == null);

        OnRemove.Invoke();
    }


    public void Add(T ObjectToRemove)
    {
        Debug.Log("is Object Remove Null?: " + ObjectToRemove == null);

        List.Add(ObjectToRemove);
        OnAdd.Invoke();
    }
}
