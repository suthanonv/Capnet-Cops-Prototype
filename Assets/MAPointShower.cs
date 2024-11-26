using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAPointShower : MonoBehaviour
{
    [SerializeField] GameObject MAPShower;

    private void OnEnable()
    {
        MAPShower.SetActive(true);
    }

    private void OnDisable()
    {
        MAPShower.SetActive(false);
    }
}
