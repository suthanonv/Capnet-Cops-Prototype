using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EngineerTurn : EntityTurnBehaviour
{
    Character character;

    [SerializeField] int BuildingRange;



    [SerializeField] GameObject Turret;



    [SerializeField] private GameObject resourceManagement;
    [SerializeField] private GameObject cost;

    Animator anim;



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
    protected override void Start()
    {
        base.Start();

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

    }
    public override void onTurn()
    {

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
        offVisual();
        BuildingMode = false;
        SelectingCharacter();
    }





    public override void AttackingButton()
    {
        ShowMoveingRange.instance.CloseMovingRangeVisual();

        if (TurnBaseSystem.instance.OnBattlePhase == false)
        {
            showedVisual = false;
            BuildingMode = true;
            TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter = null;
        }

    }




    bool showedVisual;

    Tile previsoTile;

    List<Tile> ShowedTile = new List<Tile>();

    private void Update()
    {



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
                    TurnBaseSystem.instance.PlayerInteractScript.currentTile.Highlight();
                }
                if (Input.GetMouseButtonDown(0))
                {
                    this.GetComponent<Character>().WalkAble = false;
                    resourceManagement.GetComponent<ResourceManagement>().DecreaseResource(cost.GetComponent<Cost>().turret, 1);

                    TurnBaseSystem.instance.PlayerInteractScript.currentTile.ClearHighlight();




                    Instantiate(Turret, TurnBaseSystem.instance.PlayerInteractScript.currentTile.transform.position + new Vector3(0, 0.17f, 0), Quaternion.identity);

                    TurnBaseSystem.instance.PlayerInteractScript.currentTile.ShowRangeVisual = false;

                    Status.AvalibleActionPoint--;
                    OnActionEnd();
                }
            }
            else
            {
                if (previsoTile != null)
                {
                    previsoTile.ClearHighlight();
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

        if (TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter != null || TurnBaseSystem.instance.currentTurn == Turn.Enemies && TurnBaseSystem.instance.OnBattlePhase) { DeSelect(); return; }
        ShowMoveingRange.instance.CloseMovingRangeVisual();

        if (BuildingMode == false || resourceManagement.GetComponent<ResourceManagement>().scrap < cost.GetComponent<Cost>().turret)
        {
            BuildingMode = false;
            offVisual();
        }



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


    public override void OffACtion()
    {

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
