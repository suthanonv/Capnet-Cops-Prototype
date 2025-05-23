using System.Collections.Generic;
using UnityEngine;

public class ShowMoveingRange : MonoBehaviour
{
    private Pathfinder pathfinder;
    public static ShowMoveingRange instance;

    private HashSet<Tile> currentTileRange = new HashSet<Tile>();
    private HashSet<Tile> currentAttackRange = new HashSet<Tile>();
    private Tile previousCenter = null;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (pathfinder == null)
            pathfinder = GameObject.Find("Pathfinder").GetComponent<Pathfinder>();
    }

    public void ShowCharacterMoveRange(Tile centerTile, EntityStat moveData, EntityTeam entityTeam)
    {
        // If the center tile is the same as before, do nothing

        if (centerTile == previousCenter) return;


        previousCenter = centerTile;

        // Set the new center tile

        // Clear the previous tile range



        // Calculate the movement range
        HashSet<Tile> moveRange = CalculatePathfindingRange(centerTile, moveData.AvalibleMoveStep, entityTeam);




        // Calculate the attack range based on the movement range
        HashSet<Tile> attackRange = CalculateAttackRange(moveRange, moveData.moveData.AttackRange, entityTeam);

        HashSet<Tile> PodMoveRange = CalculateAttackRange(moveRange, 1, entityTeam);



        // Mark all tiles in movement range

        currentTileRange = moveRange;



        foreach (Tile tile in moveRange)
        {


            if (tile.occupyingCharacter != null && (TurnBaseSystem.instance.PlayerInteractScript.Attacking || TurnBaseSystem.instance.OnBattlePhase == false))
            {
                if (tile.occupyingCharacter.TryGetComponent<EntityTeam>(out EntityTeam teamCheck))
                {
                    if ((teamCheck.EntityTeamSide == Team.Enemy) && centerTile.occupyingCharacter.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint > 0)
                    {
                        tile.ShowRangeVisual = true;
                        tile.IsInAttackRange = true;
                    }
                }
            }
            else if (TurnBaseSystem.instance.PlayerInteractScript.Attacking == false || TurnBaseSystem.instance.OnBattlePhase == false)
            {
                tile.ShowRangeVisual = true;
            }

        }







        if (entityTeam.gameObject.TryGetComponent<EntityTurnBehaviour>(out EntityTurnBehaviour turnStatus))
        {
            if (turnStatus.Status.AvalibleActionPoint <= 0) return;
        }


        currentAttackRange = attackRange;

        foreach (Tile tile in attackRange)
        {

            if (tile.occupyingCharacter != null && (TurnBaseSystem.instance.PlayerInteractScript.Attacking || TurnBaseSystem.instance.OnBattlePhase == false))
            {
                if (tile.occupyingCharacter.TryGetComponent<EntityTeam>(out EntityTeam teamCheck))
                {
                    if ((teamCheck.EntityTeamSide == Team.Enemy && teamCheck.TypeOfTarget != Target.Pod) && centerTile.occupyingCharacter.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint > 0)
                    {

                        tile.IsInAttackRange = true;
                        tile.ShowRangeVisual = true;
                    }
                }
            }
        }




        foreach (Tile tile in PodMoveRange)
        {

            if (tile.occupyingCharacter != null && (TurnBaseSystem.instance.PlayerInteractScript.Attacking || TurnBaseSystem.instance.OnBattlePhase == false))
            {
                if (tile.occupyingCharacter.TryGetComponent<EntityTeam>(out EntityTeam teamCheck))
                {
                    if ((teamCheck.EntityTeamSide == Team.Enemy) && centerTile.occupyingCharacter.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint > 0)
                    {
                        tile.IsInAttackRange = true;
                        tile.ShowRangeVisual = true;
                    }
                }
            }
        }

    }

    public HashSet<Tile> CalculatePathfindingRange(Tile centerTile, int range, EntityTeam entityTeam)
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

    public HashSet<Tile> CalculateAttackRange(HashSet<Tile> moveRange, int attackRange, EntityTeam entityTeam)
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

    public void CloseMovingRangeVisual()
    {
        previousCenter = null;

        foreach (Tile tile in currentTileRange)
        {
            tile.ShowRangeVisual = false;

        }

        foreach (Tile tile in currentAttackRange)
        {
            tile.IsInAttackRange = false;
            tile.ShowRangeVisual = false;
        }

        // Clear the current tile ranges
        currentTileRange.Clear();
        currentAttackRange.Clear();
    }
}
