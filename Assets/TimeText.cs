using TMPro;
using UnityEngine;
public class TimeText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Time_Text;


    // Update is called once per frame
    void Update()
    {

        Clock clock = PreparationPharse.instance.CurrentClockTime;
        if (clock == null) return;

        if (clock.Hour <= 9 && clock.Min <= 9)
        {
            Time_Text.text = $"0{clock.Hour}:0{clock.Min}";
        }
        else if (clock.Hour <= 9 & clock.Min > 9)
        {
            Time_Text.text = $"0{clock.Hour}:{clock.Min}";
        }
        else if (clock.Hour > 9 & clock.Min < 9)
        {
            Time_Text.text = $"{clock.Hour}:0{clock.Min}";
        }
        else if (clock.Hour > 9 & clock.Min > 9)
        {
            Time_Text.text = $"{clock.Hour}:{clock.Min}";
        }
    }
}
