using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Interact : MonoBehaviour
{
    public bool NeedMouseInteraction = false;

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
    public Character selectedCharacter
    {
        get { return CharacterDebug; }
        set
        {
            if (CharacterDebug != null)
            {
                PathIllustrator pathDraw = GameObject.FindWithTag("Pathfinder").GetComponent<PathIllustrator>();

                pathDraw.ClearPaht();

                ShowMoveingRange.instance.CloseMovingRangeVisual();
            }

            CharacterDebug = value;

        }
    }
    Pathfinder pathfinder;
    #endregion


    public bool Attacking { get; set; }

    private void Start()
    {
        mainCam = gameObject.GetComponent<Camera>();

        if (pathfinder == null)
            pathfinder = GameObject.Find("Pathfinder").GetComponent<Pathfinder>();
    }

    private void Update()
    {
        Clear();
        MouseUpdate();
    }

    private void MouseUpdate()
    {
        if (!Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 200f, interactMask))
            return;

        currentTile = hit.transform.GetComponent<Tile>();
        InspectTile();
    }

    private void InspectTile()
    {
        if (NeedMouseInteraction)
        {

            if (currentTile.Occupied)
                InspectCharacter();
            else
                NavigateToTile();
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
                    turn.onTurn();
                }


                SelectCharacter(charrecter);
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

        pathDraw.ClearPaht();

        ShowMoveingRange.instance.CloseMovingRangeVisual();




        selectedCharacter = charecter;

        if (charecter == null) TurnBaseSystem.instance.ActionEnd = true;


        if (selectedCharacter.TryGetComponent<EntityTurnBehaviour>(out EntityTurnBehaviour entity))
        {
            if (TurnBaseSystem.instance.OnBattlePhase)
                ShowMoveingRange.instance.ShowCharacterMoveRange(selectedCharacter.characterTile, entity.Status, selectedCharacter.GetComponent<EntityTeam>());


        }
        GetComponent<AudioSource>().PlayOneShot(pop);
    }

    public void NavigateToTile()
    {


        if (selectedCharacter == null || selectedCharacter.Moving == true && (!selectedCharacter.TryGetComponent<EntityTurnBehaviour>(out EntityTurnBehaviour turn)))
            return;

        if (RetrievePath(out Path newPath))
        {
            if (Input.GetMouseButtonDown(0) && currentTile == newPath.tiles[newPath.tiles.Length - 1])
            {

                GetComponent<AudioSource>().PlayOneShot(click);

                selectedCharacter.StartMove(newPath);

            }


        }
    }



    bool RetrievePath(out Path path)
    {
        if (!TurnBaseSystem.instance.OnBattlePhase)
        {
            selectedCharacter.GetComponent<EntityTurnBehaviour>().Status.AvalibleMoveStep = Mathf.RoundToInt((PreparationPharse.instance.PhaseTransitionTime.SecondSum() - PreparationPharse.instance.CurrentClockTime.SecondSum()) / PreparationPharse.instance.MovementCost.SecondSum());
        }


        ShowMoveingRange.instance.ShowCharacterMoveRange(selectedCharacter.characterTile, selectedCharacter.GetComponent<EntityTurnBehaviour>().Status, selectedCharacter.GetComponent<EntityTeam>());

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