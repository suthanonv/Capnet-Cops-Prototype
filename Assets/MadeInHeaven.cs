using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadeInHeaven : MonoBehaviour
{
    public void Pucci()
    {
        if (Time.timeScale <= 1)
        {
            Time.timeScale = 10f;
        }
        else if(Time.timeScale > 1)
        {
            Time.timeScale = 1f;
        }
    }
}
