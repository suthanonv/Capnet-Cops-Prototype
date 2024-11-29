using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuCamDetection : MonoBehaviour
{
    public UnityEvent unityEvent;
    public UnityEvent returnAnim;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Entered Collider");
        if(collision.gameObject.name == "Main Camera")
        {
            unityEvent.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Main Camera")
        {
            unityEvent.Invoke();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay");
        if (Input.GetKeyDown(KeyCode.Escape) && collision.gameObject.name == "Main Camera")
        {
            Debug.Log("Pressed Escape: Attempting to play Return animation");
            returnAnim.Invoke();
        }
    }


}
