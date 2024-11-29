using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuCamDetection : MonoBehaviour
{
    public UnityEvent unityEvent;
    public UnityEvent returnAnim;
    private bool tf = false;
    private bool triggerred;

    private void OnTriggerEnter(Collider other)
    {
        triggerred = true;
        Debug.Log("Entered Trigger");
        if (other.gameObject.name == "Main Camera")
        {
            unityEvent.Invoke();
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && triggerred)
        {
            tf = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        triggerred = false;
        tf = false;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay");
        if (tf)
        {
            Debug.Log("Pressed Escape Recently: Attempting to play Return animation");
            returnAnim.Invoke();
        }
    }


}
