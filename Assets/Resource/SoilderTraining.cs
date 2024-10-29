using TMPro;
using UnityEngine;

public class SoilderTraining : MonoBehaviour
{
    public static SoilderTraining Instance;




    [SerializeField] private GameObject resourceManagement;

    [SerializeField] private GameObject soilder;
    [SerializeField] private Tile spawnPos;
    [SerializeField] public Clock timeToComplete;
    [SerializeField] public float currentProgress;
    [SerializeField] private int price;
    [SerializeField] public TextMeshProUGUI priceTxt;

    [SerializeField] private GameObject window;

    private void Awake()
    {
        SoilderTraining.Instance = this;
        if (resourceManagement == null)
        {
            resourceManagement = GameObject.Find("ResourceManagement");
        }
    }

    public void Start()
    {
        priceTxt.SetText(price.ToString());
        CloseWindow();
    }

    public void Update()
    {

    }

    public void OnTrainingComplete()
    {
        Debug.Log("Training complete");
        Instantiate(soilder, spawnPos.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
    }

    public void OnTrainingStart()
    {
        if (PreparationPharse.instance.CurrentClockTime.SecondSum() + timeToComplete.SecondSum() <= PreparationPharse.instance.PhaseTransitionTime.SecondSum())
        {


            if (resourceManagement.GetComponent<ResourceManagement>().humanResource > 0 && spawnPos.occupyingCharacter == null)
            {
                PreparationPharse.instance.AddingTimeToCurrentTime(timeToComplete);
                resourceManagement.GetComponent<ResourceManagement>().DecreaseResource(price, 2);
                OnTrainingComplete();
            }
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
