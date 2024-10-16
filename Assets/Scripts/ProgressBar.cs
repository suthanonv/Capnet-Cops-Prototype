using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Slider biomassSlider;
    [SerializeField] Slider scrapsSlider;
    [SerializeField] public float maxFuelTank = 500;
    public float currentMaximum { get { return BaseHealth.Instance.MaxHealth; } set { BaseHealth.Instance.MaxHealth = value; } }


    float Progess_Holder;
    public float progress { get { return Progess_Holder; } set { Progess_Holder = value; if (Progess_Holder >= maxFuelTank) SceneManager.LoadScene("Win"); } }
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
