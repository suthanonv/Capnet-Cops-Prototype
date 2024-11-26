using System;
using System.Collections;
using System.Linq;
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
    public Animator anim;
    public AudioClip shot1;
    public AudioClip shot2;
    public AudioClip shot3;
    public AudioClip shot4;
    AudioSource audioSource;


    bool walkAble = true;
    public bool WalkAble { get { return walkAble; } set { walkAble = value; } }
    public bool isAttacking;

    [SerializeField] private bool FindLater = false;
    private void Awake()
    {

    }

    private void Start()
    {
        if (FindLater == false)
            FindTileAtStart();
        anim = this.transform.GetChild(1).GetComponent<Animator>();
        Entityteam = this.GetComponent<EntityTeam>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// If no starting tile has been manually assigned, we find one beneath us
    /// </summary>
    public void FindTileAtStart()
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


    EntityTeam Entityteam;


    [SerializeField] bool paused;

    public bool Paused { get { return paused; } set { paused = value; } }

    protected IEnumerator MoveAlongPath(Path path)
    {
        this.gameObject.GetComponent<EntityTurnBehaviour>().Onwalking();

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


        int AttackRanges = movedata.AttackRange;


        if (path.tiles[path.tiles.Length - 1].occupyingCharacter != null)
        {
            if (path.tiles[path.tiles.Length - 1].occupyingCharacter.GetComponent<EntityTeam>().TypeOfTarget == Target.Pod)
            {
                AttackRanges = 1;
            }
        }



        // Calculate required movement to reach attack range
        int requiredSteps = Mathf.Max(0, totalTiles - AttackRanges);

        // Adjust the path length based on whether the destination is occupied
        if (destinationOccupied)
        {
            // Case 1: Destination has an enemy character
            pathLength = Mathf.Min(requiredSteps, moveLimit);
        }
        else
        {
            // Case 2: Destination is not occupied
            pathLength = Mathf.Min(moveLimit, requiredSteps + AttackRanges);
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

        if (this.GetComponent<EntityTeam>().EntityTeamSide == Team.Human)
        {
            Path clonedPath = new Path();

            // Clone the tiles array
            clonedPath.tiles = (Tile[])path.tiles.Clone();
            Stat.IsWalking = true;
            if (this.gameObject.TryGetComponent<PathIllustrator>(out PathIllustrator instance))
            {
                instance.IllustratePath(clonedPath, Stat);
            }
        }


        while (currentStep < movingPath.tiles.Length)
        {
            anim.SetBool("Walking", true);
            Stat.IsWalking = anim.GetBool("Walking");
            yield return null;


            while (Paused)
            {
                anim.SetBool("Walking", false);
                yield return null;
                if (!Paused)
                {
                    anim.SetBool("Walking", true);
                }
            }




            Vector3 nextTilePosition = movingPath.tiles[currentStep].transform.position;

            float movementTime = animationTime / (AnimeSpeedAdjust.Instance.GetAnimSpeed(Entityteam) + movingPath.tiles[currentStep].terrainCost * TERRAIN_PENALTY);
            MoveAndRotate(currentTile.transform.position, nextTilePosition, movementTime);
            animationTime += Time.deltaTime;

            if (Vector3.Distance(transform.position, nextTilePosition) > MIN_DISTANCE)
                continue;

            if (currentStep != 0)
            {
                if (TurnBaseSystem.instance.OnBattlePhase)
                {
                    this.GetComponent<EntityTurnBehaviour>().Status.AvalibleMoveStep -= 1;
                }
                else PreparationPharse.instance.AddingTimeToCurrentTime(PreparationPharse.instance.MovementCost);
            }
            currentTile = movingPath.tiles[currentStep];

            if (this.GetComponent<EntityTeam>().EntityTeamSide == Team.Human)
            {
                Path clonedPath = new Path();

                // Clone the tiles array
                clonedPath.tiles = (Tile[])movingPath.tiles.Clone();
                clonedPath.tiles = clonedPath.tiles.Skip(currentStep).Take(movingPath.tiles.Count()).ToArray();
                if (this.gameObject.TryGetComponent<PathIllustrator>(out PathIllustrator instance))
                {
                    instance.IllustratePath(clonedPath, Stat);
                }
            }

            currentStep++;
            animationTime = 0f;
        }

        anim.SetBool("Walking", false);
        Stat.IsWalking = anim.GetBool("Walking");

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
            CanAttack = false;

            if (this.gameObject.TryGetComponent<PathIllustrator>(out PathIllustrator instance))
            {
                instance.ClearPaht();
            }
            Vector3 origin = this.transform.position;
            Vector3 destination = target.transform.position;

            transform.rotation = Quaternion.LookRotation(origin.DirectionTo(destination).Flat(), Vector3.up);
            anim.gameObject.GetComponent<AnimationControll>().Target = target;
            if (this.GetComponent<EntityTeam>().EntityTeamSide == Team.Human)
            {
                anim.SetBool("Select", false);
            }
            isAttacking = true;

            anim.SetTrigger("Attacking");
            playAudio();
        }

    }

    public virtual void StartMove(Path _path)
    {
        TurnBaseSystem.instance.PlayerInteractScript.enabled = false;

        TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter = null;
        TurnBaseSystem.instance.EndPharseButton.SetActive(false);
        if (WalkAble == false) return;
        PlayerActionUI.instance.EnableUI = false;

        if (this.transform.GetComponent<EntityTeam>().EntityTeamSide == Team.Human)
        {

            TurnBaseSystem.instance.PlayerInteractScript.enabled = true;
        }
        if (IsObstacle) return;
        ShowMoveingRange.instance.CloseMovingRangeVisual();

        Moving = true;

        characterTile.Occupied = false;
        characterTile.occupyingCharacter = null;
        StartCoroutine(MoveAlongPath(_path));
    }

    int count = 0;

    public void FinalizePosition(Tile tile)
    {
        TurnBaseSystem.instance.PlayerInteractScript.enabled = true;
        movedata.TargetObj = Target.None;
        transform.position = tile.transform.position;
        characterTile = tile;
        tile.InteractAble = true;
        Moving = false;
        tile.Occupied = true;
        tile.occupyingCharacter = this.gameObject;

        if (count > 0)
        {
            if (this.gameObject.TryGetComponent<PathIllustrator>(out PathIllustrator instance))
            {
                instance.ClearPaht();
            }
        }

        count++;
    }

    void MoveAndRotate(Vector3 origin, Vector3 destination, float duration)
    {
        transform.position = Vector3.Lerp(origin, destination, duration);
        transform.rotation = Quaternion.LookRotation(origin.DirectionTo(destination).Flat(), Vector3.up);
    }

    public void Character_LookAt(Tile Destination)
    {
        Vector3 Tile_Position = new Vector3(Destination.transform.position.x, this.transform.position.y, Destination.transform.position.z);
        transform.LookAt(Tile_Position);
    }

    public void playAudio()
    {
        AudioClip[] shotSounds = { shot1, shot2, shot3, shot4 };
        AudioClip selectedShotSound = shotSounds[UnityEngine.Random.Range(0, shotSounds.Length)];

        audioSource.PlayOneShot(selectedShotSound, 0.7f);
        
    }
}