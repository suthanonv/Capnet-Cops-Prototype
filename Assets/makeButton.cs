using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class makeButton : MonoBehaviour
{
    public UnityEvent unityEvent;
    public GameObject button;

    private void Start()
    {
        button = this.gameObject;
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            unityEvent.Invoke();
        }
    }
}
