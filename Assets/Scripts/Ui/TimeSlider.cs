using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    public int MaxValue = 100;
    public int CurrentValue;

    private void Update()
    {
        SlideBarMan();
    }

    void SlideBarMan()
    {
        slider.maxValue = MaxValue;
        slider.value = CurrentValue;
    }
}
