using UnityEngine;
using UnityEngine.UI;

public class Exploring : MonoBehaviour
{
    public static Exploring Instance;


    [SerializeField] private GameObject resourceManagement;

    [SerializeField] public int timeToComplete;
    [SerializeField] public float currentProgress;
    [SerializeField] public int minResourceGet;
    [SerializeField] public int maxResourceGet;


    [SerializeField] Clock ExploreingCost;
    private bool isExploring;

    [SerializeField] public Slider progressBar;

    private void Awake()
    {
        Instance = this;
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

    public void OnExploringStart()
    {
        if (PreparationPharse.instance.CurrentClockTime.SecondSum() + ExploreingCost.SecondSum() <= PreparationPharse.instance.PhaseTransitionTime.SecondSum())
        {
            PreparationPharse.instance.AddingTimeToCurrentTime(ExploreingCost);
            OnExploringComplete();
        }
    }

    public void OnExploringComplete()
    {
        //  progressBar.value = 0;
        resourceManagement.GetComponent<ResourceManagement>().IncreaseResource(Random.Range(minResourceGet, maxResourceGet), 1);
    }

    public void Update()
    {
        if (isExploring)
        {
            currentProgress += Time.deltaTime;
            progressBar.value = currentProgress;
        }

        if (currentProgress >= timeToComplete)
        {
            isExploring = false;
            currentProgress = 0;
            OnExploringComplete();
        }
    }

}
