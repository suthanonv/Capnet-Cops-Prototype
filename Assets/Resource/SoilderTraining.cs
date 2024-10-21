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
    [SerializeField] private int price;
    [SerializeField] public TextMeshProUGUI priceTxt;

    [SerializeField] private GameObject window;
    
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
        priceTxt.SetText(price.ToString());
        CloseWindow();
    }
    
    public void Update()
    {

        if (currentProgress >= timeToComplete)
        {
            Debug.Log("Training complete");
            currentProgress = 0;
            OnTrainingComplete();
        }
    }

    public void OnTrainingComplete()
    {
        Debug.Log("Training complete");
        Instantiate(soilder, spawnPos.position, Quaternion.identity);
    }

    public void OnTrainingStart()
    {
        if (resourceManagement.GetComponent<ResourceManagement>().humanResource > 0)
        {
            resourceManagement.GetComponent<ResourceManagement>().DecreaseResource(price, 2);
        }
    }

    public void ShowWindow()
    {
        window.SetActive(true);
    }

    public void CloseWindow()
    {
        window.SetActive(false);
    }
    
}
