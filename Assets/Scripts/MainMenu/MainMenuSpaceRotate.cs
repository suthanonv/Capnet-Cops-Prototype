using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSpaceRotate : MonoBehaviour
{
    [SerializeField] float degreesPerSecond = 30f;
    [SerializeField] Vector3 axis = Vector3.forward;
    void Update()
    {
        transform.Rotate(axis.normalized * degreesPerSecond * Time.deltaTime);
    }
}
