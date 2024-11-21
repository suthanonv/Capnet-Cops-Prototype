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
        slider.maxValue = PreparationPharse.instance.PhaseTransitionTime.SecondSum() - PreparationPharse.instance.StartClockTIme.SecondSum();

        float value = PreparationPharse.instance.CurrentClockTime.SecondSum() - PreparationPharse.instance.StartClockTIme.SecondSum();
        if (value > slider.maxValue) value = slider.maxValue;

        slider.value = value;
    }
}
