using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Interact : MonoBehaviour
{
    [SerializeField] bool NeedMouseInteraction = false; 

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
    Tile currentTile;
    Character selectedCharacter;
    Pathfinder pathfinder;
    #endregion




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







    Character PreviosCharacter = null;

    private void InspectCharacter()
    {

        if (currentTile.occupyingCharacter.TryGetComponent<Character>(out Character charrecter))
        {
            if (charrecter.Moving)
                return;


            currentTile.Highlight();

            if (Input.GetMouseButtonDown(0))
                SelectCharacter(charrecter);
        }
        else
        {
            return;
        }
    }

    private void Clear()
    {
        if (currentTile == null  || currentTile.Occupied == false)
            return;

        //currentTile.ModifyCost(currentTile.terrainCost-1);//Reverses to previous cost and color after being highlighted
        currentTile.ClearHighlight();
        currentTile = null;
    }

    public void SelectCharacter(Character charecter)
    {
        if(charecter != PreviosCharacter)
        {
            PathIllustrator pathDraw = GameObject.FindWithTag("Pathfinder").GetComponent<PathIllustrator>();

            pathDraw.ClearPaht();

            ShowMoveingRange.instance.CloseMovingRangeVisual();


        }
        
        selectedCharacter = charecter;

        ShowMoveingRange.instance.ShowCharacterMoveRange(selectedCharacter.characterTile, selectedCharacter.movedata, selectedCharacter.GetComponent<EntityTeam>());
        GetComponent<AudioSource>().PlayOneShot(pop);
    }

    public void NavigateToTile()
    {
        if (selectedCharacter == null || selectedCharacter.Moving == true)
            return;

        if (RetrievePath(out Path newPath ))
        {
            if (Input.GetMouseButtonDown(0) && currentTile == newPath.tiles[newPath.tiles.Length - 1])
            {

                GetComponent<AudioSource>().PlayOneShot(click);

                selectedCharacter.StartMove(newPath);
                selectedCharacter = null;
            }

            if(Input.GetMouseButton(1))
            {
                selectedCharacter = null;


                PathIllustrator pathDraw = GameObject.FindWithTag("Pathfinder").GetComponent<PathIllustrator>();

                pathDraw.ClearPaht();

                ShowMoveingRange.instance.CloseMovingRangeVisual();



            }
        }
    }

    bool RetrievePath(out Path path)
    {
      
        ShowMoveingRange.instance.ShowCharacterMoveRange(selectedCharacter.characterTile, selectedCharacter.movedata, selectedCharacter.GetComponent<EntityTeam>());
        path = pathfinder.FindPath(selectedCharacter.characterTile, currentTile , selectedCharacter.movedata , selectedCharacter.GetComponent<EntityTeam>());



        if (path == null )
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