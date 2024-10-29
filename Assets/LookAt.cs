using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    void Start()
    {
        transform.LookAt(transform.parent);
    }
}
