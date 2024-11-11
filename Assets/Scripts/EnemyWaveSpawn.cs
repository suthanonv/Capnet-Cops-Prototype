using UnityEngine;
using UnityEngine.Events;
public class EnemyWaveSpawn : MonoBehaviour
{
    public static EnemyWaveSpawn instance;

    private void Awake()
    {
        instance = this;
    }


    [SerializeField] int Start_EnemyQuantity;
    [SerializeField] int EnemyMultiple_Scaling_PerWave;

    int currentWave = -1;
    public int CurrentWave { get { return currentWave; } set { currentWave = value; } }


    public UnityEvent SpawningEnemy = new UnityEvent();

    public void StartEnemyWave()
    {
        if (PreparationPharse.instance.CurrentClockTime.SecondSum() >= PreparationPharse.instance.PhaseTransitionTime.SecondSum())
            SpawningEnemy.Invoke();
    }



}
