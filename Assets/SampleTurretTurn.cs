using System.Collections.Generic;
using UnityEngine;

public class SampleTurretTurn : EntityTurnBehaviour
{
    private Pathfinder pathfinder;
    private Character Char;
    private EntityTurnBehaviour turnBehaviour;
    private AnimationControll animC;

    private Animator anim;
    private void Start()
    {

        TurnBaseSystem.instance.turnSystems.Add(this);

        if (pathfinder == null)
            pathfinder = GameObject.Find("Pathfinder").GetComponent<Pathfinder>();

        Char = this.GetComponent<Character>();

        turnBehaviour = this.GetComponent<EntityTurnBehaviour>();

        anim = this.transform.GetChild(0).GetComponent<Animator>();
        animC = this.transform.GetChild(0).GetComponent<AnimationControll>();
    }




    public override void onTurn()
    {

        Debug.Log("Start Turn");

        HashSet<Tile> moveRange = CalculatePathfindingRange(Char.characterTile, turnBehaviour.Status.AvalibleMoveStep, this.GetComponent<EntityTeam>());

        // Calculate the attack range based on the movement range
        HashSet<Tile> attackRange = CalculateAttackRange(moveRange, turnBehaviour.Status.moveData.AttackRange, this.GetComponent<EntityTeam>());


        foreach (Tile tile in attackRange)
        {

            if (tile.occupyingCharacter != null)
            {


                if (tile.occupyingCharacter.TryGetComponent<EntityTeam>(out EntityTeam teamCheck))
                {
                    if ((teamCheck.EntityTeamSide == Team.Enemy))
                    {
                        animC.Target = tile.occupyingCharacter.GetComponent<EnemyHealth>();

                        anim.SetTrigger("Attacking");
                        break;
                    }
                }
            }
        }


    }
    public override void OnActionEnd()
    {
        TurnBaseSystem.instance.ActionEnd = true;
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
