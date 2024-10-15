using UnityEngine;

public class Biomass : MonoBehaviour
{

    private GameObject resourceManagement;
    [SerializeField] private int minResourceGet;
    [SerializeField] private int maxResourceGet;

    private void Awake()
    {
        if (resourceManagement == null)
        {
            resourceManagement = GameObject.Find("ResourceManagement");
        }
    }

    public void OnDie()
    {
        resourceManagement.GetComponent<ResourceManagement>().IncreaseResource(Random.Range(minResourceGet, maxResourceGet), 0);
        resourceManagement.GetComponent<ResourceManagement>().IncreaseResource(Random.Range(minResourceGet, maxResourceGet), 1);
    }

}
