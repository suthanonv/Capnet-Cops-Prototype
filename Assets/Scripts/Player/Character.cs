using JetBrains.Annotations;
using System;
using System.Collections;
using System.IO;
using System.Linq;
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
        bool IsMoveRangeMoreThanMaxRange = false;

        CanAttack = true;

        const float MIN_DISTANCE = 0.05f;
        const float TERRAIN_PENALTY = 0.5f;

        Path moveingPath = new Path();

        int currentStep = 0;
        int pathLength = path.tiles.Length;
        Tile currentTile = path.tiles[0];
        float animationTime = 0f;

        if(pathLength > movedata.MaxMove)
        {
            IsMoveRangeMoreThanMaxRange = true;
            pathLength = movedata.MaxMove+1;
        }


        if (path.tiles[pathLength - 1].occupyingCharacter != null)
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




        if (path.tiles[pathLength - 1].occupyingCharacter == null)
        {
            FinalizePosition(moveingPath.tiles[moveingPath.tiles.Length - 1]);
        }
        else
        {
            if (path.tiles[pathLength - 1].occupyingCharacter.GetComponent<EntityTeam>().EntityTeamSide != this.GetComponent<EntityTeam>().EntityTeamSide)
            {
                Attack();
            }
            FinalizePosition(moveingPath.tiles[moveingPath.tiles.Length - 1]);
        }


        if(IsMoveRangeMoreThanMaxRange)
        {
            Path RamaingPath = new Path();

            RamaingPath.tiles = path.tiles.Skip(pathLength - 1).Take(path.tiles.Length - 1).ToArray();

            foreach(Tile i in RamaingPath.tiles)
            {
               if(i.occupyingCharacter)
                {
                   if(i.TryGetComponent<EntityTeam>(out EntityTeam team))
                    {
                        if (team.EntityTeamSide != this.GetComponent<EntityTeam>().EntityTeamSide)
                        {
                            Attack();
                            break;
                        }
                    }
                }
            }
        }
    }


    bool CanAttack = false;
    public void Attack()
    {
        if(CanAttack)
        {
            CanAttack = false;
            Debug.Log(this.gameObject.name + "Attack");
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