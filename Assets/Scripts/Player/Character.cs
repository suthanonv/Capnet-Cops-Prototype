using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region member fields
    public bool Moving { get; private set; } = false;

    public CharacterMoveData movedata;
    public Tile characterTile;
    [SerializeField]
    LayerMask GroundLayerMask;
    #endregion

    public bool IsObstacle;
    Animator anim;



    private void Awake()
    {
       
    }

    private void Start()
    {
        FindTileAtStart();
        anim = this.transform.GetChild(0).GetComponent<Animator>();
    }

    /// <summary>
    /// If no starting tile has been manually assigned, we find one beneath us
    /// </summary>
    void FindTileAtStart()
    {
        if (characterTile != null)
        {
            FinalizePosition(characterTile);
            return;
        }

        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 50f, GroundLayerMask))
        {
            FinalizePosition(hit.transform.GetComponent<Tile>());
            return;
        }

        Debug.Log("Unable to find a start position");
    }

    protected IEnumerator MoveAlongPath(Path path)
    {

        CanAttack = true;

        EntityStat Stat = this.GetComponent<EntityTurnBehaviour>().Status;


        const float MIN_DISTANCE = 0.05f;
        const float TERRAIN_PENALTY = 0.5f;

        int pathLength = path.tiles.Length;
        Path movingPath = new Path();
        int totalTiles = path.tiles.Length;
        int moveLimit = Stat.AvalibleMoveStep + 1;

        // Determine if the destination has an enemy character
        bool destinationOccupied = totalTiles > 0 && path.tiles[totalTiles - 1].occupyingCharacter != null;

        // Calculate required movement to reach attack range
        int requiredSteps = Mathf.Max(0, totalTiles - movedata.AttackRange);

        // Adjust the path length based on whether the destination is occupied
        if (destinationOccupied)
        {
            // Case 1: Destination has an enemy character
            pathLength = Mathf.Min(requiredSteps, moveLimit);
        }
        else
        {
            // Case 2: Destination is not occupied
            pathLength = Mathf.Min(moveLimit, requiredSteps + movedata.AttackRange);
        }

        // Ensure we don't exceed the length of the tiles array
        pathLength = Mathf.Min(pathLength, totalTiles);



        // Copy the appropriate number of steps into the moving path
        movingPath.tiles = new Tile[pathLength];
        Array.Copy(path.tiles, movingPath.tiles, pathLength);

        int currentStep = 0;


        if (movingPath.tiles.Length <= 0)
        {
            FinalizePosition(path.tiles[0]);
            if (destinationOccupied)
            {
                // Attack if the final tile is within attack range and has an enemy character
                var finalCharacter = path.tiles[totalTiles - 1].occupyingCharacter;
                if (finalCharacter != null && finalCharacter.GetComponent<EntityTeam>().EntityTeamSide != this.GetComponent<EntityTeam>().EntityTeamSide)
                {
                    Attack(finalCharacter.GetComponent<Health>());
                }
                else
                {
                    this.GetComponent<EntityTurnBehaviour>().OnActionEnd();
                }
            }

            // Finalize position at the end of the movement


            yield break;
        }


        Tile currentTile = movingPath.tiles[currentStep];

        float animationTime = 0f;

        while (currentStep < movingPath.tiles.Length)
        {
            anim.SetBool("Walking", true);
            yield return null;

            Vector3 nextTilePosition = movingPath.tiles[currentStep].transform.position;

            float movementTime = animationTime / (movedata.MoveSpeed + movingPath.tiles[currentStep].terrainCost * TERRAIN_PENALTY);
            MoveAndRotate(currentTile.transform.position, nextTilePosition, movementTime);
            animationTime += Time.deltaTime;

            if (Vector3.Distance(transform.position, nextTilePosition) > MIN_DISTANCE)
                continue;

            if (currentStep != 0)
            {
                if (TurnBaseSystem.instance.OnBattlePhase) this.GetComponent<EntityTurnBehaviour>().Status.AvalibleMoveStep -= 1;
                else PreparationPharse.instance.AddingTimeToCurrentTime(PreparationPharse.instance.MovementCost);
            }
            currentTile = movingPath.tiles[currentStep];
            currentStep++;
            animationTime = 0f;
        }

        anim.SetBool("Walking", false);

        FinalizePosition(movingPath.tiles[movingPath.tiles.Length - 1]);

        if (destinationOccupied)
        {
            // Attack if the final tile is within attack range and has an enemy character
            var finalCharacter = path.tiles[totalTiles - 1].occupyingCharacter;
            if (finalCharacter != null && finalCharacter.GetComponent<EntityTeam>().EntityTeamSide != this.GetComponent<EntityTeam>().EntityTeamSide)
            {
                Attack(finalCharacter.GetComponent<Health>());
            }
            else
            {

                {
                    this.GetComponent<EntityTurnBehaviour>().OnActionEnd();
                }
            }
        }
        else
        {
            this.GetComponent<EntityTurnBehaviour>().OnActionEnd();
        }

        // Finalize position at the end of the movement

    }



    bool CanAttack = false;
    public void Attack(Health target)
    {

        if (CanAttack)
        {
            Vector3 origin = this.transform.position;
            Vector3 destination = target.transform.position;

            transform.rotation = Quaternion.LookRotation(origin.DirectionTo(destination).Flat(), Vector3.up);
            this.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint -= 1;
            anim.gameObject.GetComponent<AnimationControll>().Target = target;
            anim.SetTrigger("Attacking");
            CanAttack = false;
        }
    }

    public virtual void StartMove(Path _path)
    {
        PlayerActionUI.instance.EnableUI = false;

        TurnBaseSystem.instance.PlayerInteractScript.enabled = false;
        if (IsObstacle) return;
        ShowMoveingRange.instance.CloseMovingRangeVisual();

        Moving = true;

        characterTile.Occupied = false;
        characterTile.occupyingCharacter = null;

        StartCoroutine(MoveAlongPath(_path));
    }

    int count = 0;

    void FinalizePosition(Tile tile)
    {
        TurnBaseSystem.instance.PlayerInteractScript.enabled = true;

        transform.position = tile.transform.position;
        characterTile = tile;
        Moving = false;
        tile.Occupied = true;
        tile.occupyingCharacter = this.gameObject;

        if (count > 0)
        {
            PathIllustrator pathDraw = GameObject.FindWithTag("Pathfinder").GetComponent<PathIllustrator>();
            pathDraw.ClearPaht();
        }

        count++;
    }

    void MoveAndRotate(Vector3 origin, Vector3 destination, float duration)
    {
        transform.position = Vector3.Lerp(origin, destination, duration);
        transform.rotation = Quaternion.LookRotation(origin.DirectionTo(destination).Flat(), Vector3.up);
    }
}