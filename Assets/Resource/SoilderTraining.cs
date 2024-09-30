using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoilderTraining : MonoBehaviour
{

    [SerializeField] private GameObject resourceManagement;

    [SerializeField] private GameObject soilder;
    [SerializeField] private Transform spawnPos;
    [SerializeField] public int timeToComplete;
    [SerializeField] public float currentProgress;
    private bool isTraining;
    [SerializeField] public Slider progressBar;
    
    private void Awake()
    {
        if (resourceManagement == null)
        {
            resourceManagement = GameObject.Find("ResourceManagement");
        }
    }
    
    public void Start()
    {
        timeToComplete *= 60;
        progressBar.maxValue = timeToComplete;
        progressBar.value = 0;
    }
    
    public void Update()
    {
        if (isTraining)
        {
            currentProgress += Time.deltaTime;
            progressBar.value = currentProgress;
        }

        if (currentProgress >= timeToComplete)
        {
            Debug.Log("Training complete");
            isTraining = false;
            currentProgress = 0;
            OnTrainingComplete();
        }
    }

    public void OnTrainingComplete()
    {
        Debug.Log("Training complete");
        progressBar.value = 0;
        Instantiate(soilder, spawnPos.position, Quaternion.identity);
    }

    public void OnTrainingStart()
    {
        if (resourceManagement.GetComponent<ResourceManagement>().humanResource > 0)
        {
            resourceManagement.GetComponent<ResourceManagement>().DecreaseResource(1, 2);
            isTraining = true;
        }
    }
    
}
