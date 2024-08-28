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

    private void Awake()
    {
        FindTileAtStart();
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

    IEnumerator MoveAlongPath(Path path)
    {
        CanAttack = true;

        const float MIN_DISTANCE = 0.05f;
        const float TERRAIN_PENALTY = 0.5f;

        Path moveingPath = new Path();
        int pathLength = path.tiles.Length;
        int moveLimit = movedata.MaxMove;

        // Determine if the destination has an enemy character
        bool destinationOccupied = path.tiles[path.tiles.Length - 1].occupyingCharacter != null;

        // Calculate required movement to reach attack range
        int requiredSteps = Mathf.Max(0, pathLength - movedata.AttackRange);

        if (destinationOccupied)
        {
            // Case 1: Destination has an enemy character, calculate the number of steps required to reach attack range

            pathLength = Mathf.Min(requiredSteps, moveLimit);
        }
        else
        {
            // Case 2: Destination is not occupied, move up to movement limit or to attack range
            pathLength = Mathf.Min(moveLimit, requiredSteps + movedata.AttackRange);
        }

        // Copy the appropriate number of steps into the moving path
        moveingPath.tiles = new Tile[pathLength];
        Array.Copy(path.tiles, moveingPath.tiles, pathLength);

        int currentStep = 0;
        Tile currentTile = moveingPath.tiles[0];
        float animationTime = 0f;

        while (currentStep < moveingPath.tiles.Length)
        {
            yield return null;

            Vector3 nextTilePosition = moveingPath.tiles[currentStep].transform.position;

            float movementTime = animationTime / (movedata.MoveSpeed + moveingPath.tiles[currentStep].terrainCost * TERRAIN_PENALTY);
            MoveAndRotate(currentTile.transform.position, nextTilePosition, movementTime);
            animationTime += Time.deltaTime;

            if (Vector3.Distance(transform.position, nextTilePosition) > MIN_DISTANCE)
                continue;

            // Min dist has been reached, look to next step in path
            currentTile = moveingPath.tiles[currentStep];
            currentStep++;
            animationTime = 0f;
        }

        TurnBaseSystem.instance.ActionEnd = true;

        if (destinationOccupied)
        {
            // Attack if the final tile is within attack range and has an enemy character
            if (path.tiles[path.tiles.Length - 1].occupyingCharacter.GetComponent<EntityTeam>().EntityTeamSide != this.GetComponent<EntityTeam>().EntityTeamSide)
            {
                Attack(path.tiles[path.tiles.Length - 1].occupyingCharacter.GetComponent<Health>());
            }
        }

        FinalizePosition(moveingPath.tiles[moveingPath.tiles.Length - 1]);
    }

    bool CanAttack = false;
    public void Attack(Health target)
    {
        if (CanAttack)
        {
            CanAttack = false;
            target.TakeDamage(50);
        }
    }

    public void StartMove(Path _path)
    {
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