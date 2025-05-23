using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EnemySpawnPoint : MonoBehaviour
{
    public static EnemySpawnPoint Instance;

    [SerializeField] LayerMask TileLayer;

    Vector3 Size = new Vector3(0, 10, 10);


    Tile CenterTile;


    Pathfinder pathfinder;


    [SerializeField] Character Base;


    [Header("ALl Wave")]
    [SerializeField] List<Wave> AllWaveComponent = new List<Wave>();

    EntityTeam en;



    [Header("Pod Component")]
    [SerializeField] GameObject Pod_Prefab;
    [SerializeField] public List<PodSpawningRange> podSpawningRanges = new List<PodSpawningRange>();
    


    private void Awake()
    {
        Instance = this;
    }




    void Start()
    {
        
        CenterTile = Base.characterTile;
        en = this.GetComponent<EntityTeam>();
    }


    List<GameObject> Pods = new List<GameObject>();

    public void SpawningPod()
    {

        foreach (var pod in podSpawningRanges)
        {
            PodSpawn(pod);
        }
    }


    List<Tile> PodTile = new List<Tile>();

    public void PodSpawn(PodSpawningRange podRange)
    {
        List<Tile> SpawnAbleTile = new List<Tile>();

        HashSet<Tile> moveRange = ShowMoveingRange.instance.CalculatePathfindingRange(CenterTile, podRange.SpawningDistance, en);

        // Calculate the attack range based on the movement range
        HashSet<Tile> attackRange = ShowMoveingRange.instance.CalculateAttackRange(moveRange, podRange.SpawningInRange + podRange.SpawningDistance, en);


        attackRange.ExceptWith(moveRange);

        List<int> IndexOfTilToSpawnEnemy = new List<int>();




        GameObject HeighestPod = null;
        float PreviosHeight = 0;

        bool addedNum = false;

        while (!addedNum)
        {
            int newIndex = UnityEngine.Random.Range(0, attackRange.Count() - 1);

            if (!IndexOfTilToSpawnEnemy.Contains(newIndex) && attackRange.ToList()[newIndex].Occupied == false && !PodTile.Contains(attackRange.ToList()[newIndex]))
            {
                IndexOfTilToSpawnEnemy.Add(newIndex);

                float RandomHeigh = UnityEngine.Random.Range(100, 151);



                GameObject PodNew = Instantiate(Pod_Prefab, attackRange.ToList()[newIndex].transform.position + new Vector3(0, RandomHeigh, 0), Quaternion.identity);
                PodNew.gameObject.GetComponent<Character>().characterTile = attackRange.ToList()[newIndex];
                PodTile.Add(attackRange.ToList()[newIndex]);
                Pods.Add(PodNew);
                addedNum = true;

                if (RandomHeigh > PreviosHeight)
                {
                    PreviosHeight = RandomHeigh;
                    HeighestPod = PodNew;
                }
            }
        }

        HeighestPod.GetComponent<CollectingPod>().IsHighestPod = true;

    }


    public void SpawningWave(int CurrentWave)
    {
        int WaveIndex = CurrentWave;
        if (CurrentWave >= AllWaveComponent.Count) WaveIndex = AllWaveComponent.Count - 1;

        foreach (EnemyInWave i in AllWaveComponent[WaveIndex].AllEnemyInWave)
        {
            SpawningEnemy(i);
        }
    }

    public void SpawningEnemy(EnemyInWave Enemy)
    {
        List<Tile> SpawnAbleTile = new List<Tile>();

        HashSet<Tile> moveRange = ShowMoveingRange.instance.CalculatePathfindingRange(CenterTile, Enemy.SpawningDistance, en);

        // Calculate the attack range based on the movement range
        HashSet<Tile> attackRange = ShowMoveingRange.instance.CalculateAttackRange(moveRange, Enemy.SpawningInRange + Enemy.SpawningDistance, en);


        attackRange.ExceptWith(moveRange);

        List<int> IndexOfTilToSpawnEnemy = new List<int>();





        for (int i = 0; i < Enemy.AmountToSpawn; i++)
        {
            bool addedNum = false;

            while (!addedNum)
            {
                int newIndex = UnityEngine.Random.Range(0, attackRange.Count() - 1);

                if (!IndexOfTilToSpawnEnemy.Contains(newIndex) && attackRange.ToList()[newIndex].Occupied == false)
                {
                    IndexOfTilToSpawnEnemy.Add(newIndex);

                    attackRange.ToList()[newIndex].SetSpawnEnemy(Enemy.EnemyPrefab);

                    //                    Instantiate(Enemy, attackRange.ToList()[newIndex].transform.position + new Vector3(0, 0.17f, 0), Quaternion.identity);

                    addedNum = true;
                }
            }
        }
    }




    public List<Tile> NeighborTiles(Tile origin)
    {
        const float HEXAGONAL_OFFSET = 1.75f;
        List<Tile> tiles = new List<Tile>();
        Vector3 direction = Vector3.forward * (origin.GetComponent<MeshFilter>().sharedMesh.bounds.extents.x * HEXAGONAL_OFFSET);
        float rayLength = 4f;
        float rayHeightOffset = 1f;



        for (int i = 0; i < 6; i++)
        {


            direction = Quaternion.Euler(0f, 60f, 0f) * direction;




            Debug.Log(direction);

            Vector3 aboveTilePos = (origin.transform.position + direction).With(y: origin.transform.position.y + rayHeightOffset);

            if (Physics.Raycast(aboveTilePos, Vector3.down, out RaycastHit hit, rayLength, TileLayer))
            {
                Tile hitTile = hit.transform.GetComponent<Tile>();
                if (hitTile != null)
                {
                    if (!hitTile.Occupied)
                    {
                        tiles.Add(hitTile);
                        hitTile.Highlight();
                    }
                }
            }

            Debug.DrawRay(aboveTilePos, Vector3.down * rayLength, Color.blue);
        }

        // Additionally add connected tiles such as ladders
        if (origin.connectedTile != null)
            tiles.Add(origin.connectedTile);

        Debug.Log($"Spawning TIle : {tiles.Count}");

        return tiles;
    }
}
