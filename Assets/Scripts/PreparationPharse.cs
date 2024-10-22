using UnityEngine;

public class PreparationPharse : MonoBehaviour
{
    public static PreparationPharse instance;



    [SerializeField] Clock StartClockTIme = new Clock();

    Clock currentTime;
    public Clock CurrentClockTime
    {
        get { return currentTime; }
        set
        {
            currentTime = value;
            if (currentTime.SecondSum() >= PhaseTransitionTime.SecondSum())
            {
                if (TurnBaseSystem.instance.OnBattlePhase == false)
                {
                    StartEnemyWave();
                }
            }
        }
    }

    public Clock PhaseTransitionTime = new Clock();



    public Clock MovementCost;

    public Clock ClockMoveSpeed;

    private void Start()
    {
        SetToStartTime();

    }


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (onCD == false)
        {
            onCD = true;

            Invoke("IncreaseTIme", 0.5f);
        }

    }
    bool onCD = false;
    void IncreaseTIme()
    {
        onCD = false;
        AddingTimeToCurrentTime(ClockMoveSpeed);
    }


    public void SetToStartTime()
    {
        CurrentClockTime = StartClockTIme;
    }


    public void SetToAttackTime()
    {
        CurrentClockTime = PhaseTransitionTime;

    }



    public void AddingTimeToCurrentTime(Clock TimeToAdd)
    {
        CurrentClockTime.Hour += TimeToAdd.Hour;
        CurrentClockTime.Min += TimeToAdd.Min;
        CurrentClockTime.Second += TimeToAdd.Second;
        CurrentClockTime.ReSizeTime();
    }






    public void StartEnemyWave()
    {

        EnemyWaveSpawn.instance.CurrentWave++;
        EnemyWaveSpawn.instance.StartEnemyWave();
    }


}
