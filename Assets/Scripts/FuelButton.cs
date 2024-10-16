using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuelButton : MonoBehaviour
{
    [SerializeField] Slider biomassSlider;
    [SerializeField] Slider scrapsSlider;
    [SerializeField] ProgressBar progressBar;
    [SerializeField] float biomass;
    [SerializeField] float scraps;
    [SerializeField] Button fuelButton;
    [SerializeField] TMP_Text biomasssAmount;
    [SerializeField] TMP_Text scrapsAmount;
    [SerializeField] Button addBiomass;
    [SerializeField] Button addScraps;
    void Start()
    {
        progressBar.currentMaximum = 500;
        progressBar.progress = 0f;
        fuelButton.onClick.AddListener(Fuel);
        addBiomass.onClick.AddListener(AddBiomass);
        addScraps.onClick.AddListener(AddScraps);
    }
    void Update()
    {
        if (progressBar.currentMaximum - progressBar.progress <= biomass)
        {
            biomassSlider.maxValue = progressBar.currentMaximum - progressBar.progress;
        }
        else 
        {
            biomassSlider.maxValue = biomass;
        }
        if (progressBar.maxFuelTank - progressBar.currentMaximum <= scraps)
        {
            scrapsSlider.maxValue = progressBar.maxFuelTank - progressBar.currentMaximum;
        }
        else
        {
            scrapsSlider.maxValue = scraps;
        }
        biomasssAmount.text = $"{biomass}";
        scrapsAmount.text = $"{scraps}";
        if (biomassSlider.value != 0)
        {
            biomassSlider.value = (int)biomassSlider.value;
            biomasssAmount.text = $"{biomass} - ({biomassSlider.value})";
        }
        if (scrapsSlider.value != 0)
        {
            scrapsSlider.value = (int)scrapsSlider.value;
            scrapsAmount.text = $"{scraps} - ({scrapsSlider.value})";
        }
    }
    void Fuel()
    {
        progressBar.progress += biomassSlider.value;
        progressBar.currentMaximum += scrapsSlider.value;
        biomass -= biomassSlider.value;
        scraps -= scrapsSlider.value;
        biomassSlider.value = 0;
        scrapsSlider.value = 0;
    }

    void AddBiomass()
    {
        biomass += 100;
    }

    void AddScraps()
    {
        scraps += 100;
    }
}
