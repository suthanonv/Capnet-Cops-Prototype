using UnityEngine;

public class PreparationPharse : MonoBehaviour
{
    public static PreparationPharse instance;



    [SerializeField] Clock StartClockTIme = new Clock();
    public Clock CurrentClockTime { get; set; }

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




    public void AddingTimeToCurrentTime(Clock TimeToAdd)
    {
        CurrentClockTime.Hour += TimeToAdd.Hour;
        CurrentClockTime.Min += TimeToAdd.Min;
        CurrentClockTime.Second += TimeToAdd.Second;
        CurrentClockTime.ReSizeTime();


        float CurrenClockTimeSecond = CurrentClockTime.Hour * 3600 + CurrentClockTime.Min * 60 + CurrentClockTime.Second;
        float PhaseTransTime = PhaseTransitionTime.Hour * 3600 + PhaseTransitionTime.Min * 60 + PhaseTransitionTime.Second;




        if (CurrenClockTimeSecond >= PhaseTransTime)
        {
            if (TurnBaseSystem.instance.OnBattlePhase == false)
            {
                EnemyWaveSpawn.instance.CurrentWave++;
                EnemyWaveSpawn.instance.StartEnemyWave();
                TurnBaseSystem.instance.OnBattlePhase = true;
            }


        }

    }
}
