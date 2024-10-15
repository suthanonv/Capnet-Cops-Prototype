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
    [Header("Spawning Range")]
    [SerializeField] int EnemySpawnDistance = 6;
    [SerializeField] int EnemySpawningRange = 3;

    [Header("ALl Wave")]
    [SerializeField] List<Wave> AllWaveComponent = new List<Wave>();

    EntityTeam en;











    private void Awake()
    {
        Instance = this;
    }




    void Start()
    {
        CenterTile = Base.characterTile;
        en = this.GetComponent<EntityTeam>();
    }







    public void SpawningWave(int CurrentWave)
    {
        if (CurrentWave > AllWaveComponent.Count) CurrentWave = AllWaveComponent.Count - 1;

        foreach (EnemyInWave i in AllWaveComponent[CurrentWave].AllEnemyInWave)
        {
            SpawningEnemy(i.AmountToSpawn, i.EnemyPrefab);
        }
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

        return tiles;
    }
}
