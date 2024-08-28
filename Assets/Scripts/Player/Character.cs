using JetBrains.Annotations;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

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

        int currentStep = 0;
        int pathLength = path.tiles.Length;
        Tile currentTile = path.tiles[0];
        float animationTime = 0f;

        if (path.tiles.Length > movedata.MaxMove)
        {
             pathLength = movedata.MaxMove;

            // Check if the last tile in the path is occupied by an enemy character
            if (path.tiles[path.tiles.Length - 1].occupyingCharacter != null)
            {
                // Calculate the difference between attack range and movement range
                int attackRangeDifference = movedata.AttackRange - movedata.MaxMove;

                // Ensure the difference is non-negative
                if (attackRangeDifference < 0)
                    attackRangeDifference = 0;

                // Adjust path length to bring the character into attack range
                pathLength += attackRangeDifference;
            }
            else
            {
                // Move as far as possible within movement range
                pathLength = movedata.MaxMove;
            }

            // Limit the pathLength to the actual number of tiles in the path
            pathLength = Mathf.Min(pathLength, path.tiles.Length);
        }



        if (path.tiles[path.tiles.Length -1].occupyingCharacter != null)
        {
            moveingPath.tiles = new Tile[pathLength - 1];
            Array.Copy(path.tiles, moveingPath.tiles, pathLength - 1);
        }
        else
        {
            moveingPath.tiles = new Tile[pathLength];
            Array.Copy(path.tiles, moveingPath.tiles, pathLength);
        }

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




        if (path.tiles[path.tiles.Length - 1].occupyingCharacter == null)
        {
            FinalizePosition(moveingPath.tiles[moveingPath.tiles.Length - 1]);
        }
        else
        {
            if (path.tiles[path.tiles.Length - 1].occupyingCharacter.GetComponent<EntityTeam>().EntityTeamSide != this.GetComponent<EntityTeam>().EntityTeamSide)
            {
                Attack(path.tiles[path.tiles.Length - 1].occupyingCharacter.GetComponent<Health>());
            }
            FinalizePosition(moveingPath.tiles[moveingPath.tiles.Length - 1]);
        }

    }


    bool CanAttack = false;
    public void Attack(Health Target)
    {
        if(CanAttack)
        {
            
            CanAttack = false;
            Target.TakeDamage(50);
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