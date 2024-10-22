using UnityEngine;

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


    public void StartEnemyWave()
    {
        Debug.Log(CurrentWave);
        EnemySpawnPoint.Instance.SpawningWave(CurrentWave);
    }



}
