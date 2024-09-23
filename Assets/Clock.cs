using UnityEngine;

[System.Serializable]
public class Clock
{
    public int Hour;
    public int Min;
    public int Second;



    public void ReSizeTime()
    {

        Min += Mathf.RoundToInt(Second / 60);
        Second %= 60;

        Hour += Mathf.RoundToInt(Min / 60);
        Min %= 60;


    }

    public int SecondSum()
    {
        return (Min * 60 + Hour * 3600 + Second);
    }

    public bool IsMoreThanOrEqual(Clock otherToCheck)
    {
        this.ReSizeTime();
        otherToCheck.ReSizeTime();

        if (this.Hour >= otherToCheck.Hour && this.Min >= otherToCheck.Min && this.Second >= otherToCheck.Second) return true;

        return false;
    }




}
