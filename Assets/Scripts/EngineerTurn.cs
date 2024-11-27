using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EngineerTurn : EntityTurnBehaviour
{
    Character character;

    [SerializeField] int BuildingRange;



    [SerializeField] GameObject Turret;
    public AudioClip SetupTurret;
    AudioSource audioSource;


    [SerializeField] private GameObject resourceManagement;
    [SerializeField] private GameObject cost;

    Animator anim;

    [SerializeField] GameObject HoloGrameTurret;

    public override void ResetState()
    {

        base.ResetState();
        BuildingMode = false;
        offVisual();
    }

    public override void Onwalking()
    {
        offVisual();
    }

    public override bool InterActacle()
    {
        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            if (Status.AvalibleActionPoint > 0 || Status.AvalibleMoveStep > 0) return true;
            else return false;
        }


        return true;
    }

    GameObject VisualTurret;
    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        character = this.gameObject.GetComponent<Character>();
        TurnBaseSystem.instance.playerTurnSystems.Add(this);

        if (resourceManagement == null)
        {
            resourceManagement = GameObject.Find("ResourceManagement");
        }

        if (cost == null)
        {
            cost = GameObject.Find("Cost");
        }

        PlayerActionUI.instance.EndPhaseEvent.AddListener(offVisual);

        anim = this.transform.GetChild(1).GetComponent<Animator>();
        VisualTurret = Instantiate(HoloGrameTurret, this.transform.position, Quaternion.identity);
        VisualTurret.SetActive(false);
    }
    public override void onTurn()
    {
        TurnBaseSystem.instance.PlayerInteractScript.Walking = false;




        OffAction = false;
        BuildingMode = false;
        TurnBaseSystem.instance.ActionEnd = false;
        PlayerActionUI.instance.Troops = this;
        CameraBehaviouerControll.instance.LookAtTarget(this.transform);
        CameraBehaviouerControll.instance.LookAtTarget(null);

        SelectingCharacter();

    }


    public void OpenUI()
    {

        PlayerActionUI.instance.EnableUI = true;

        IsPreviosBattlePhase = TurnBaseSystem.instance.OnBattlePhase;
        List<PlayerActionUiButton> ButtonToUse = new List<PlayerActionUiButton>();

        TurnBaseSystem.instance.EndPharseButton.SetActive(true);

        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            ButtonToUse.Add(PlayerActionUiButton.Walk);
            ButtonToUse.Add(PlayerActionUiButton.Attack);

            ButtonToUse.Add(PlayerActionUiButton.EndTurn);
            ButtonToUse.Add(PlayerActionUiButton.MP_AP);

        }
        else
        {
            ButtonToUse.Add(PlayerActionUiButton.Walk);
            ButtonToUse.Add(PlayerActionUiButton.Attack);
            PlayerActionUiLayOut.instance.EditingActionButtonName("Building");
            ButtonToUse.Add(PlayerActionUiButton.EndTurn);
        }

        PlayerActionUiLayOut.instance.ArrangementUiButton(ButtonToUse);
    }



    bool BuildingMode = false;

    public override void WalkingButton()
    {

        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            TurnBaseSystem.instance.PlayerInteractScript.Attacking = false;
        }


        BuildingMode = false;
        offVisual();
        this.GetComponent<Character>().WalkAble = true;

        TurnBaseSystem.instance.PlayerInteractScript.Walking = true;
        TurnBaseSystem.instance.PlayerInteractScript.ClearIlustatePath();
        ShowMoveingRange.instance.ShowCharacterMoveRange(this.GetComponent<Character>().characterTile, Status, this.GetComponent<EntityTeam>());
    }





    public override void AttackingButton()
    {
        if (TurnBaseSystem.instance.OnBattlePhase && Status.AvalibleActionPoint == 0) return;

        ShowMoveingRange.instance.CloseMovingRangeVisual();
        TurnBaseSystem.instance.PlayerInteractScript.ClearIlustatePath();
        if (TurnBaseSystem.instance.OnBattlePhase == false && resourceManagement.GetComponent<ResourceManagement>().scrap >= cost.GetComponent<Cost>().turret)
        {
            showedVisual = false;
            BuildingMode = true;
            TurnBaseSystem.instance.PlayerInteractScript.Walking = false;

        }

        if (TurnBaseSystem.instance.OnBattlePhase)
        {

            TurnBaseSystem.instance.PlayerInteractScript.Walking = true;
            TurnBaseSystem.instance.PlayerInteractScript.Attacking = true;
            ShowMoveingRange.instance.ShowCharacterMoveRange(this.GetComponent<Character>().characterTile, Status, this.GetComponent<EntityTeam>());




        }

    }


    bool showedVisual;

    Tile previsoTile;

    List<Tile> ShowedTile = new List<Tile>();

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.T)) BuildingRange = 70;
        if (BuildingMode && resourceManagement.GetComponent<ResourceManagement>().scrap >= cost.GetComponent<Cost>().turret)
        {
            HashSet<Tile> BuidlAbleTile = ShowMoveingRange.instance.CalculatePathfindingRange(character.characterTile, BuildingRange, this.GetComponent<EntityTeam>());
            BuidlAbleTile.Remove(this.GetComponent<Character>().characterTile);
            ShowedTile = BuidlAbleTile.ToList();
            if (!showedVisual)
            {
                showedVisual = true;
                foreach (Tile tile in BuidlAbleTile)
                {
                    tile.ShowRangeVisual = true;
                }
            }
            if (BuidlAbleTile.Contains(TurnBaseSystem.instance.PlayerInteractScript.currentTile))
            {
                if (previsoTile != TurnBaseSystem.instance.PlayerInteractScript.currentTile)
                {
                    if (previsoTile != null)
                    {
                        previsoTile.ClearHighlight();
                    }
                    previsoTile = TurnBaseSystem.instance.PlayerInteractScript.currentTile;
                    VisualTurret.transform.position = TurnBaseSystem.instance.PlayerInteractScript.currentTile.transform.position + new Vector3(0, 0.17f, 0);
                    VisualTurret.SetActive(true);
                    TurnBaseSystem.instance.PlayerInteractScript.currentTile.Highlight();
                }
                if (Input.GetMouseButtonDown(0))
                {
                    this.GetComponent<Character>().WalkAble = false;
                    resourceManagement.GetComponent<ResourceManagement>().DecreaseResource(cost.GetComponent<Cost>().turret, 1);

                    TurnBaseSystem.instance.PlayerInteractScript.currentTile.ClearHighlight();




                    Instantiate(Turret, TurnBaseSystem.instance.PlayerInteractScript.currentTile.transform.position + new Vector3(0, 0.17f, 0), Quaternion.identity);
                    audioSource.PlayOneShot(SetupTurret, 0.7f);
                    TurnBaseSystem.instance.PlayerInteractScript.currentTile.ShowRangeVisual = false;

                    Status.AvalibleActionPoint--;

                    if (resourceManagement.GetComponent<ResourceManagement>().scrap < cost.GetComponent<Cost>().turret)
                    {
                        BuildingMode = false;
                        offVisual();
                    }

                    OnActionEnd();
                }
            }
            else
            {
                if (previsoTile != null)
                {
                    previsoTile.ClearHighlight();
                    VisualTurret.SetActive(false);
                }

            }
        }
    }







    public void offVisual()
    {
        foreach (Tile i in ShowedTile)
        {
            i.ShowRangeVisual = false;
        }

        ShowedTile = new List<Tile>();
    }




    bool IsPreviosBattlePhase = false;



    public override void OnActionEnd()
    {

        if (OffAction || TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter != null || TurnBaseSystem.instance.currentTurn == Turn.Enemies && TurnBaseSystem.instance.OnBattlePhase) { DeSelect(); return; }
        ShowMoveingRange.instance.CloseMovingRangeVisual();

        this.GetComponent<Character>().WalkAble = true;


        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            if (Status.AvalibleActionPoint > 0 || Status.AvalibleMoveStep > 0)
            {
                SelectingCharacter();
            }
            else
            {
                Invoke("OffACtion", 0.1f);
            }
        }
        else if (IsPreviosBattlePhase == true && !TurnBaseSystem.instance.OnBattlePhase)
        {
            return;
        }
        else
        {

            if (BuildingMode == false)
                SelectingCharacter();
        }

    }

    bool OffAction = false;
    public override void OffACtion()
    {
        OffAction = true;
        anim.SetTrigger("deselect");
        CameraBehaviouerControll.instance.ResetTransform();
        PlayerActionUI.instance.Troops = null;
        PlayerActionUI.instance.EnableUI = false;
        TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter = null;
        TurnBaseSystem.instance.ActionEnd = true;

    }


    void SelectingCharacter()
    {
        OpenUI();
        Invoke("EnableWalk", 0.1f);



        if (!TurnBaseSystem.instance.OnBattlePhase)
        {
            Status.AvalibleMoveStep = TurnBaseSystem.instance.PlayerInteractScript.CurrentMove;
        }
        else
        {

            Status.moveData.BaseAttackRange = Status.moveData.AttackRange;
        }

        TurnBaseSystem.instance.PlayerInteractScript.enabled = true;

        PlayerActionUiLayOut.instance.EnableUI = true;
        PlayerActionUI.instance.EnableUI = true;
        PlayerActionUI.instance.Troops = this;
        TurnBaseSystem.instance.PlayerInteractScript.SelectCharacter(character);

        TurnBaseSystem.instance.EndPharseButton.SetActive(true);
    }

    void EnableWalk()
    {
        this.GetComponent<Character>().WalkAble = true;
    }


    private void OnDestroy()
    {
        PlayerActionUI.instance.EndPhaseEvent.RemoveListener(offVisual);

    }
}
