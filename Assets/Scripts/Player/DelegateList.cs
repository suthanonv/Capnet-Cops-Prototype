using System.Collections.Generic;
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

        List.RemoveAll(item => item == null);

        OnRemove.Invoke();
    }


    public void Add(T ObjectToRemove)
    {

        List.Add(ObjectToRemove);
        OnAdd.Invoke();
    }
}
