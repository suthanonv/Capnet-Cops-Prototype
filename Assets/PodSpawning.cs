using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PodSpawning : MonoBehaviour
{
    public static PodSpawning Instance;

    [SerializeField] LayerMask TileLayer;

    Vector3 Size = new Vector3(0, 10, 10);


    Tile CenterTile;


    Pathfinder pathfinder;


    [SerializeField] Character Base;
    [Header("Spawning Range")]
    [SerializeField] int EnemySpawnDistance = 6;
    [SerializeField] int EnemySpawningRange = 3;

    [Header("Prefab")]
    [SerializeField] GameObject Prefab;

    EntityTeam en;











    private void Awake()
    {

        Instance = this;
    }




    void Start()
    {
        Debug.Log($"CenterTile {CenterTile == null} : En {en == null} : ShowMoveingRange {ShowMoveingRange.instance == null}");

        CenterTile = Base.characterTile;
        en = base.GetComponent<EntityTeam>();
    }





    bool isFirst = true;

    public void Spawningod()
    {



        SpawningEnemy(2, Prefab);

    }

    public void SpawningEnemy(int AmountToSpawn, GameObject Enemy)
    {
        List<Tile> SpawnAbleTile = new List<Tile>();
        HashSet<Tile> moveRange = ShowMoveingRange.instance.CalculatePathfindingRange(CenterTile, EnemySpawnDistance, en);

        // Calculate the attack range based on the movement range
        HashSet<Tile> attackRange = ShowMoveingRange.instance.CalculateAttackRange(moveRange, EnemySpawningRange + EnemySpawnDistance, en);


        attackRange.ExceptWith(moveRange);

        List<int> IndexOfTilToSpawnEnemy = new List<int>();





        for (int i = 0; i < AmountToSpawn; i++)
        {
            bool addedNum = false;

            while (!addedNum)
            {
                int newIndex = Random.Range(0, attackRange.Count() - 1);

                if (!IndexOfTilToSpawnEnemy.Contains(newIndex) && attackRange.ToList()[newIndex].Occupied == false)
                {
                    IndexOfTilToSpawnEnemy.Add(newIndex);

                    Instantiate(Enemy, attackRange.ToList()[newIndex].transform.position + new Vector3(0, 0.17f, 0), Quaternion.identity);

                    addedNum = true;
                }
            }
        }
    }





}
