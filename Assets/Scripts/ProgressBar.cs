using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Slider biomassSlider;
    [SerializeField] Slider scrapsSlider;
    [SerializeField] public float maxFuelTank = 1000;
    public float currentMaximum;
    public float progress;
    public Image mask;
    public Image tankMax;
    public Image fuelingAmount;
    void Start()
    {
    }


    void Update()
    {
        GetCurrentFill();
        GetCurrentMax();
        if (biomassSlider.value != 0)
        {
            GetFuelingAmount();
        }
        else
        {
            fuelingAmount.fillAmount = 0;
        }
    }

    void GetCurrentFill()
    {
        float fillAmount = progress / maxFuelTank;
        mask.fillAmount = fillAmount;
    }
    void GetCurrentMax()
    {
        tankMax.fillAmount = currentMaximum / maxFuelTank;
    }
    void GetFuelingAmount()
    {
        float fillAmount = (progress + (int)biomassSlider.value) / maxFuelTank;
        fuelingAmount.fillAmount = fillAmount;
    }
}
