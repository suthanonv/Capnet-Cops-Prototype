using UnityEngine;
using UnityEngine.UIElements;

public class PreparationPharse : MonoBehaviour
{
    public static PreparationPharse instance;

    [Header("Light")]
    [SerializeField] Light directionalLight;

    [SerializeField] float dayIntensity;
    [SerializeField] float nightIntensity;
    [SerializeField] float nightTemperature;
    [SerializeField] float dayTemperature;


    Clock currentTime = new Clock();
    [Header("Clock")]

    public Clock StartClockTIme = new Clock();

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



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetToStartTime();
        EnemySpawnPoint.Instance.SpawningPod();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            CurrentClockTime.Hour = StartClockTIme.Hour;
            CurrentClockTime.Min = StartClockTIme.Min;
            CurrentClockTime.Second = StartClockTIme.Second;
        }
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
        CurrentClockTime.Hour = StartClockTIme.Hour;
        CurrentClockTime.Min = StartClockTIme.Min;
        CurrentClockTime.Second = StartClockTIme.Second;
        ChangeDirectionLight();
        EnemyWaveSpawn.instance.CurrentWave++;
        EnemySpawnPoint.Instance.SpawningWave(EnemyWaveSpawn.instance.CurrentWave);
        TurnBaseSystem.instance.PlayerInteractScript.SetAvalibeMoveStep();

    }


    public void SetToAttackTime()
    {
        CurrentClockTime.Hour = PhaseTransitionTime.Hour;
        CurrentClockTime.Min = PhaseTransitionTime.Min;
        CurrentClockTime.Second = PhaseTransitionTime.Second;
        ChangeDirectionLight();
        EnemyWaveSpawn.instance.StartEnemyWave();
    }



    public void AddingTimeToCurrentTime(Clock TimeToAdd)
    {
        CurrentClockTime.Hour += TimeToAdd.Hour;
        CurrentClockTime.Min += TimeToAdd.Min;
        CurrentClockTime.Second += TimeToAdd.Second;
        CurrentClockTime.ReSizeTime();
        ChangeDirectionLight();
        if (currentTime.SecondSum() >= PhaseTransitionTime.SecondSum())
        {
            if (TurnBaseSystem.instance.OnBattlePhase == false)
            {
                StartEnemyWave();
            }
        }
    }



    void ChangeDirectionLight()
    {
        CurrentClockTime.ReSizeTime();

        // Calculate hours elapsed from start time
        int hoursElapsed = CurrentClockTime.Hour - StartClockTIme.Hour;

        // Ensure `hoursElapsed` is within the day-to-night range (assuming a 12-hour cycle from 8 AM to 8 PM)
        hoursElapsed = Mathf.Clamp(hoursElapsed, 0, 12);

        // Adjust intensity and temperature based on hours elapsed

        directionalLight.intensity = dayIntensity + (hoursElapsed * -(1f / 3f)); // Smooth transition to night intensity
        directionalLight.colorTemperature = dayTemperature + (hoursElapsed * 1015f); // Smooth transition to night temperature


        if (currentTime.SecondSum() >= PhaseTransitionTime.SecondSum())
        {
            directionalLight.intensity = nightIntensity;
            directionalLight.colorTemperature = nightTemperature;
        }

    }




    public void StartEnemyWave()
    {

        EnemyWaveSpawn.instance.StartEnemyWave();
    }


}
