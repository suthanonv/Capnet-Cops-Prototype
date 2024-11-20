using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Interact : MonoBehaviour
{

    #region member fields
    [SerializeField]
    AudioClip click, pop;
    [SerializeField]
    LayerMask interactMask;

    //Debug purposes only
    [SerializeField]
    bool debug;
    Path Lastpath;

    Camera mainCam;
    public Tile currentTile { get; set; }

    Character CharacterDebug;


    int CurrentMove = 0;
    public void SetAvalibeMoveStep()
    {
        Debug.Log(PreparationPharse.instance.PhaseTransitionTime.SecondSum() - PreparationPharse.instance.CurrentClockTime.SecondSum());
        int MOvePoint = Mathf.RoundToInt((PreparationPharse.instance.PhaseTransitionTime.SecondSum() - PreparationPharse.instance.CurrentClockTime.SecondSum()) / PreparationPharse.instance.MovementCost.SecondSum());
        Debug.Log(MOvePoint);

        CurrentMove = MOvePoint;
    }


    public Character selectedCharacter
    {
        get { return CharacterDebug; }
        set
        {
            if (CharacterDebug != null)
            {

                PathIllustrator pathDraw = GameObject.FindWithTag("Pathfinder").GetComponent<PathIllustrator>();

                pathDraw.ClearPaht();
                if (value == null && TurnBaseSystem.instance.ActionEnd)
                {
                    CharacterDebug.gameObject.GetComponent<EntityTurnBehaviour>().DeSelect();
                }
                else
                {
                    CharacterDebug.gameObject.GetComponent<EntityTurnBehaviour>().Select();

                }

            }



            CharacterDebug = value;

        }
    }
    Pathfinder pathfinder;
    #endregion

    [SerializeField] PathIllustrator PathIllustrator;
    public bool EnableInteracting = true;

    bool Attacking;

    private void Start()
    {
        mainCam = gameObject.GetComponent<Camera>();

        if (pathfinder == null)
            pathfinder = GameObject.Find("Pathfinder").GetComponent<Pathfinder>();
        selectedCharacter = null;

    }

    private void Update()
    {
        Clear();

        if (Input.GetKeyDown(KeyCode.Escape) && selectedCharacter != null)
        {
            PlayerActionUI.instance.EndPhase();
        }
        MouseUpdate();
    }

    private void MouseUpdate()
    {
        if (!Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 200f, interactMask))
            return;
        if (hit.transform.gameObject.TryGetComponent<Tile>(out Tile tile))
        {
            if (tile.InteractAble)
                currentTile = tile;
            else return;

        }
        InspectTile();
    }

    private void InspectTile()
    {
        if (EnableInteracting == false || (TurnBaseSystem.instance.currentTurn == Turn.Enemies && TurnBaseSystem.instance.OnBattlePhase)) return;

        if (currentTile.Occupied && currentTile.occupyingCharacter != null)
        {
            if (currentTile.occupyingCharacter.TryGetComponent<EntityTeam>(out EntityTeam teamSide))
            {
                if (teamSide.EntityTeamSide == Team.Human)
                    InspectCharacter();
                else
                    NavigateToTile();
            }
        }
        else
        {
            NavigateToTile();
        }

    }








    private void InspectCharacter()
    {

        if (currentTile.occupyingCharacter.TryGetComponent<Character>(out Character charrecter))
        {
            if (charrecter.Moving)
                return;


            currentTile.Highlight();

            if (Input.GetMouseButtonDown(0))
            {
                if (charrecter.gameObject.TryGetComponent<EntityTurnBehaviour>(out EntityTurnBehaviour turn))
                {
                    if (turn.InterActacle())
                    {
                        ShowMoveingRange.instance.CloseMovingRangeVisual();
                        turn.onTurn();
                    }
                }


                //    SelectCharacter(charrecter);
            }
        }
        else
        {
            return;
        }
    }

    private void Clear()
    {
        if (currentTile == null || currentTile.Occupied == false)
            return;

        //currentTile.ModifyCost(currentTile.terrainCost-1);//Reverses to previous cost and color after being highlighted
        currentTile.ClearHighlight();
        currentTile = null;
    }

    public void SelectCharacter(Character charecter)
    {


        PathIllustrator pathDraw = GameObject.FindWithTag("Pathfinder").GetComponent<PathIllustrator>();

        // pathDraw.ClearPaht();


        if (selectedCharacter != null)
        {
            if (selectedCharacter.GetComponent<Character>().Moving == false)
            {
                selectedCharacter.GetComponent<EntityTurnBehaviour>().DeSelect();
            }
        }


        selectedCharacter = charecter;

        if (charecter == null) TurnBaseSystem.instance.ActionEnd = true;



        if (!TurnBaseSystem.instance.OnBattlePhase)
        {
            selectedCharacter.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint = 99;
            selectedCharacter.GetComponent<EntityTurnBehaviour>().Status.AvalibleMoveStep = CurrentMove;
        }
        else
        {
            selectedCharacter.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint = selectedCharacter.GetComponent<EntityTurnBehaviour>().Status.ActionPoint;

            selectedCharacter.GetComponent<EntityTurnBehaviour>().Status.moveData.BaseAttackRange = selectedCharacter.GetComponent<EntityTurnBehaviour>().Status.moveData.AttackRange;
        }

        ShowMoveingRange.instance.CloseMovingRangeVisual();
        ShowMoveingRange.instance.ShowCharacterMoveRange(selectedCharacter.characterTile, selectedCharacter.GetComponent<EntityTurnBehaviour>().Status, selectedCharacter.GetComponent<EntityTeam>());

        GetComponent<AudioSource>().PlayOneShot(pop);
        charecter.gameObject.GetComponent<EntityTurnBehaviour>().Select();
    }

    public void NavigateToTile()
    {


        if (selectedCharacter == null || selectedCharacter.Moving == true && (!selectedCharacter.TryGetComponent<EntityTurnBehaviour>(out EntityTurnBehaviour turn)) || (selectedCharacter.anim.GetBool("Walking")))
            return;

        ShowMoveingRange.instance.ShowCharacterMoveRange(selectedCharacter.characterTile, selectedCharacter.GetComponent<EntityTurnBehaviour>().Status, selectedCharacter.GetComponent<EntityTeam>());

        if (RetrievePath(out Path newPath))
        {
            DebugPath = newPath;


            if (CameraBehaviouerControll.instance.isMoving == false)
                selectedCharacter.Character_LookAt(currentTile);

            if (currentTile != null)
            {
                if (Input.GetMouseButtonDown(0) && currentTile == newPath.tiles[newPath.tiles.Length - 1])
                {
                    if (newPath.tiles[newPath.tiles.Length - 1].occupyingCharacter == null)
                    {
                        CurrentMove -= newPath.tiles.Length - 1;
                        newPath.tiles[newPath.tiles.Length - 1].InteractAble = false;
                    }
                    else
                    {
                        CurrentMove -= newPath.tiles.Length;

                    }

                    GetComponent<AudioSource>().PlayOneShot(click);
                    PathIllustrator.ClearPaht();
                    ShowMoveingRange.instance.CloseMovingRangeVisual();
                    selectedCharacter.StartMove(newPath);

                }


            }
        }
    }

    Path DebugPath = new Path();

    bool RetrievePath(out Path path)
    {
        selectedCharacter.GetComponent<EntityTurnBehaviour>().Status.moveData.BaseAttackRange = selectedCharacter.GetComponent<EntityTurnBehaviour>().Status.moveData.AttackRange;

        if (!TurnBaseSystem.instance.OnBattlePhase)
        {
            selectedCharacter.GetComponent<EntityTurnBehaviour>().Status.AvalibleMoveStep = Mathf.RoundToInt((PreparationPharse.instance.PhaseTransitionTime.SecondSum() - PreparationPharse.instance.CurrentClockTime.SecondSum()) / PreparationPharse.instance.MovementCost.SecondSum());
        }
        if (currentTile.occupyingCharacter != null)
        {
            if (currentTile.occupyingCharacter.GetComponent<EntityTeam>().TypeOfTarget == Target.Pod)
            {
                selectedCharacter.GetComponent<EntityTurnBehaviour>().Status.moveData.BaseAttackRange = 1;
            }
        }


        if (TurnBaseSystem.instance.OnBattlePhase)
            Attacking = selectedCharacter.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint > 0;
        else
            Attacking = true;

        path = pathfinder.FindPath(selectedCharacter.characterTile, currentTile, selectedCharacter.GetComponent<EntityTurnBehaviour>().Status, selectedCharacter.GetComponent<EntityTeam>(), Attacking);




        if (path == null)
            return false;

        //Debug only
        if (debug)
        {
            ClearLastPath();
            DebugNewPath(path);
            Lastpath = path;
        }
        return true;
    }

    //Debug only
    void ClearLastPath()
    {
        if (Lastpath == null)
            return;

        foreach (Tile tile in Lastpath.tiles)
        {
            //tile.ClearText();
        }
    }

    //Debug only
    void DebugNewPath(Path path)
    {
        foreach (Tile t in path.tiles)
        {
            //t.DebugCostText();
        }
    }
}