using System.Collections.Generic;
using UnityEngine;

public class SampleTurretTurn : EntityTurnBehaviour
{
    private Pathfinder pathfinder;
    private Character Char;
    private EntityTurnBehaviour turnBehaviour;
    private AnimationControll animC;

    private Animator anim;




    [SerializeField] AttackingRadius attackingRadius;



    private void Start()
    {


        if (pathfinder == null)
            pathfinder = GameObject.Find("Pathfinder").GetComponent<Pathfinder>();

        Char = this.GetComponent<Character>();

        turnBehaviour = this.GetComponent<EntityTurnBehaviour>();
        TurnBaseSystem.instance.TurretTurn.Add(this);
        anim = this.transform.GetChild(0).GetComponent<Animator>();
        animC = this.transform.GetChild(0).GetComponent<AnimationControll>();
    }




    public override void onTurn()
    {

       if(attackingRadius.EnemyToAttack == null) return;

        animC.Target = attackingRadius.EnemyToAttack.GetComponent<EnemyHealth>();
        if (animC.Target == null) return;
        transform.rotation = Quaternion.LookRotation(Char.characterTile.transform.position.DirectionTo(attackingRadius.EnemyToAttack.transform.position).Flat(), Vector3.up);
        anim.SetTrigger("Attacking");





    }



    private HashSet<Tile> CalculatePathfindingRange(Tile centerTile, int range, EntityTeam entityTeam)
    {
        HashSet<Tile> tilesInRange = new HashSet<Tile>();
        Queue<Tile> tilesToProcess = new Queue<Tile>();

        // Start with the center tile
        tilesToProcess.Enqueue(centerTile);
        tilesInRange.Add(centerTile);

        for (int i = 0; i < range; i++)
        {
            int count = tilesToProcess.Count;

            for (int j = 0; j < count; j++)
            {
                Tile tile = tilesToProcess.Dequeue();
                List<Tile> neighborTiles = pathfinder.NeighborTiles(tile, entityTeam, centerTile, true, false);

                foreach (Tile neighbor in neighborTiles)
                {
                    // Add the tile to range if not already in range and it's not occupied
                    if (!tilesInRange.Contains(neighbor) && !neighbor.Occupied)
                    {
                        tilesInRange.Add(neighbor);
                        tilesToProcess.Enqueue(neighbor);
                    }
                }
            }
        }

        return tilesInRange;
    }

    private HashSet<Tile> CalculateAttackRange(HashSet<Tile> moveRange, int attackRange, EntityTeam entityTeam)
    {
        HashSet<Tile> tilesInRange = new HashSet<Tile>();
        Queue<Tile> tilesToProcess = new Queue<Tile>();

        // Process each tile in the movement range to calculate the attack range
        foreach (Tile moveTile in moveRange)
        {
            tilesToProcess.Enqueue(moveTile);
        }

        for (int i = 0; i < attackRange; i++)
        {
            int count = tilesToProcess.Count;

            for (int j = 0; j < count; j++)
            {
                Tile tile = tilesToProcess.Dequeue();
                List<Tile> neighborTiles = pathfinder.NeighborTiles(tile, entityTeam, tile, true, true);

                foreach (Tile neighbor in neighborTiles)
                {
                    // Add the tile to attack range if not already in range and it's not in the movement range
                    if (!tilesInRange.Contains(neighbor) && !moveRange.Contains(neighbor))
                    {
                        tilesInRange.Add(neighbor);
                        tilesToProcess.Enqueue(neighbor);
                    }
                }
            }
        }

        return tilesInRange;
    }

}
