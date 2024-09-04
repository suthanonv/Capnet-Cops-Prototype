using System.Collections;
using UnityEngine;

public class EnemyTurnBehaviour : EntityTurnBehaviour
{
    private Character enemyChar;
    private Tile destinationTile;


    private void Start()
    {

        enemyChar = GetComponent<Character>();

        // Register this entity's turn behavior with the TurnBaseSystem
        TurnBaseSystem.instance.GetComponent<TurnBaseSystem>().turnSystems.Add(this);
    }

    public override void onTurn()
    {
        base.onTurn();
        Character Human = TurnBaseSystem.instance.GetHumenNearestChar(enemyChar);


        if (Human == null) OnActionEnd();

        destinationTile = Human.characterTile;



        StartCoroutine(FindAndMove());
    }

    IEnumerator FindAndMove()
    {
        destinationTile = TurnBaseSystem.instance.GetHumenNearestChar(enemyChar).characterTile;




        Path newPath = null;
        int maxAttempts = 10;  // Safeguard to prevent infinite loops
        int attempts = 0;

        // Attempt to retrieve a valid path in a loop
        while (newPath == null && attempts < maxAttempts)
        {
            attempts++;
            if (RetrievePath(out newPath))
            {
                Debug.Log(this.gameObject.name + ": " + newPath.tiles[newPath.tiles.Length - 1].Occupied + ": " + newPath.tiles.Length);

                enemyChar.StartMove(newPath);
                yield break;  // Exit the coroutine once a valid path is found and movement starts
            }

            // Wait until the next frame before attempting again
            yield return null;
        }

        if (newPath == null)
        {
            // Handle the case where no valid path was found after max attempts
            Debug.LogWarning("No valid path found after maximum attempts.");
        }
    }


    public override void DoingAction(int TypeOfAction)
    {
        // Implement the action logic here if needed
    }

    private bool RetrievePath(out Path path)
    {
        // Find the path from the enemy's current tile to the destination tile
        path = TurnBaseSystem.instance.EnemyPathFindingScript.FindPath(enemyChar.characterTile, destinationTile, enemyChar.movedata);

        // Return true if a valid path is found, false otherwise
        return path != null;
    }

    public override void OnActionEnd()
    {
        TurnBaseSystem.instance.ActionEnd = true;
    }
}