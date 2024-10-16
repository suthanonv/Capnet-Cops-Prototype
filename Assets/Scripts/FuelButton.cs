using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FuelButton : MonoBehaviour
{
    [SerializeField] Slider biomassSlider;
    [SerializeField] Slider scrapsSlider;
    [SerializeField] ProgressBar progressBar;

    [SerializeField] Button fuelButton;
    [SerializeField] TMP_Text biomasssAmount;
    [SerializeField] TMP_Text scrapsAmount;
    [SerializeField] Button addBiomass;
    [SerializeField] Button addScraps;
    void Start()
    {
        progressBar.currentMaximum = BaseHealth.Instance.MaxHealth;
        progressBar.progress = 0f;
        fuelButton.onClick.AddListener(Fuel);

    }
    void Update()
    {
        if (progressBar.currentMaximum - progressBar.progress <= ResourceManagement.Instance.bioMass)
        {
            biomassSlider.maxValue = progressBar.currentMaximum - progressBar.progress;
        }
        else
        {
            biomassSlider.maxValue = ResourceManagement.Instance.bioMass;
        }
        if (progressBar.maxFuelTank - progressBar.currentMaximum <= ResourceManagement.Instance.scrap)
        {
            scrapsSlider.maxValue = progressBar.maxFuelTank - progressBar.currentMaximum;
        }
        else
        {
            scrapsSlider.maxValue = ResourceManagement.Instance.scrap;
        }
        biomasssAmount.text = $"{ResourceManagement.Instance.bioMass}";
        scrapsAmount.text = $"{ResourceManagement.Instance.scrap}";
        if (biomassSlider.value != 0)
        {
            biomassSlider.value = (int)biomassSlider.value;
            biomasssAmount.text = $"{ResourceManagement.Instance.bioMass} - ({biomassSlider.value})";
        }
        if (scrapsSlider.value != 0)
        {
            scrapsSlider.value = (int)scrapsSlider.value;
            scrapsAmount.text = $"{ResourceManagement.Instance.scrap} - ({scrapsSlider.value})";
        }
    }
    void Fuel()
    {
        progressBar.progress += biomassSlider.value;
        progressBar.currentMaximum += scrapsSlider.value;
        ResourceManagement.Instance.bioMass -= (int)biomassSlider.value;
        ResourceManagement.Instance.scrap -= (int)scrapsSlider.value;
        biomassSlider.value = 0;
        scrapsSlider.value = 0;
    }

}
