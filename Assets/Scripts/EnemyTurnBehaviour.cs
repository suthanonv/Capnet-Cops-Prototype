using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnBehaviour : EntityTurnBehaviour
{
    private Character enemyChar;
    private Tile destinationTile;
    public AudioSource audioSource;
    public AudioClip monsterRoar;
    [Range(0.5f, 1.5f)] public float pitchMin = 0.8f;
    [Range(0.5f, 1.5f)] public float pitchMax = 1.2f;

    [SerializeField] List<Target> targets = new List<Target>();


    protected override void Start()
    {

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = monsterRoar;
        audioSource.playOnAwake = false;
        base.Start();
        Status.moveData.BaseAttackRange = Status.moveData.AttackRange;
        enemyChar = GetComponent<Character>();
        this.GetComponent<Character>().Paused = true;
        // Register this entity's turn behavior with the TurnBaseSystem
        TurnBaseSystem.instance.GetComponent<TurnBaseSystem>().enemiesTurnSystems.Add(this);
    }

    public bool Spawned = false;
    public override void onTurn()
    {
        Character Human = TurnBaseSystem.instance.GetHumenNearestChar(enemyChar, targets);

        if (Human == null)
        {
            OnActionEnd();
            return;
        }

        destinationTile = Human.characterTile;

        this.gameObject.GetComponent<Character>().Character_LookAt(destinationTile);
        CameraBehaviouerControll.instance.LookAtTarget(this.gameObject.transform);
        this.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Play");

        if (!Spawned)
        {
            playRoarSound(true);
            return;
        }
        else
        {
            playRoarSound();
        }

        base.onTurn();

        StartCoroutine(FindAndMove());
    }

    IEnumerator FindAndMove()
    {
        destinationTile = TurnBaseSystem.instance.GetHumenNearestChar(enemyChar, targets).characterTile;




        Path newPath = null;
        int maxAttempts = 10;  // Safeguard to prevent infinite loops
        int attempts = 0;

        // Attempt to retrieve a valid path in a loop
        while (newPath == null && attempts < maxAttempts)
        {
            attempts++;
            if (RetrievePath(out newPath))
            {

                enemyChar.StartMove(newPath);
                yield break;  // Exit the coroutine once a valid path is found and movement starts
            }

            // Wait until the next frame before attempting again
            yield return null;
        }

        if (newPath == null)
        {
            // Handle the case where no valid path was found after max attempts
            OnActionEnd();
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
        CameraBehaviouerControll.instance.ResetTransform();
    }

    public void playRoarSound(bool forcePlay = false)
    {
        if (forcePlay || Random.value <= 0.3f) // 30% chance or forced play
        {
            // Randomize the pitch within the specified range
            audioSource.pitch = Random.Range(pitchMin, pitchMax);

            // Play the roar sound
            audioSource.Play();
        }
    }

}