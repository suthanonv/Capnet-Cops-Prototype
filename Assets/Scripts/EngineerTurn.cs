using System.Collections.Generic;
using UnityEngine;
public class EngineerTurn : EntityTurnBehaviour
{
    Character character;

    [SerializeField] int BuildingRange;



    [SerializeField] GameObject Turret;



    [SerializeField] private GameObject resourceManagement;
    [SerializeField] private GameObject cost;

    public override bool InterActacle()
    {
        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            if (Status.AvalibleActionPoint > 0 || Status.AvalibleMoveStep > 0) return true;
            else return false;
        }


        return true;
    }
    private void Start()
    {
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

    }
    public override void onTurn()
    {
        OpenUI();
        PlayerActionUI.instance.Troops = this;
        CameraBehaviouerControll.instance.LookAtTarget(this.transform.GetChild(0));
        CameraBehaviouerControll.instance.LookAtTarget(null);

        SelectingCharacter();

    }


    public void OpenUI()
    {
        IsPreviosBattlePhase = TurnBaseSystem.instance.OnBattlePhase;
        List<PlayerActionUiButton> ButtonToUse = new List<PlayerActionUiButton>();


        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            ButtonToUse.Add(PlayerActionUiButton.Walk);
            ButtonToUse.Add(PlayerActionUiButton.Attack);
            PlayerActionUiLayOut.instance.EditingActionButtonName("Healing");

            ButtonToUse.Add(PlayerActionUiButton.EndTurn);
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
        BuildingMode = false;
        SelectingCharacter();
    }





    public override void AttackingButton()
    {

        if (TurnBaseSystem.instance.OnBattlePhase == false)
        {
            showedVisual = false;
            BuildingMode = true;
            TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter = null;
        }

    }




    bool showedVisual;

    Tile previsoTile;

    private void Update()
    {



        if (BuildingMode && resourceManagement.GetComponent<ResourceManagement>().scrap >= cost.GetComponent<Cost>().turret)
        {
            HashSet<Tile> BuidlAbleTile = ShowMoveingRange.instance.CalculatePathfindingRange(character.characterTile, BuildingRange, this.GetComponent<EntityTeam>());
            BuidlAbleTile.Remove(this.GetComponent<Character>().characterTile);

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


                    foreach (Tile tile in BuidlAbleTile)
                    {
                        tile.ShowRangeVisual = false;
                    }

                    Instantiate(Turret, TurnBaseSystem.instance.PlayerInteractScript.currentTile.transform.position + new Vector3(0, 0.17f, 0), Quaternion.identity);



                    BuidlAbleTile.Clear();
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












    bool IsPreviosBattlePhase = false;



    public override void OnActionEnd()
    {

       



        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            if (Status.AvalibleActionPoint > 0 || Status.AvalibleMoveStep > 0)
            {
                SelectingCharacter();
            }
            else
            {

                Invoke("Delay", 0.1f);
            }
        }
        else if (IsPreviosBattlePhase == true && !TurnBaseSystem.instance.OnBattlePhase)
        {
            return;
        }
        else
        {
            SelectingCharacter();
        }

    }


    void Delay()
    {

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
        PlayerActionUiLayOut.instance.EnableUI = true;
        PlayerActionUI.instance.EnableUI = true;
        PlayerActionUI.instance.Troops = this;
        TurnBaseSystem.instance.PlayerInteractScript.Attacking = false;
        TurnBaseSystem.instance.PlayerInteractScript.SelectCharacter(character);
        
        TurnBaseSystem.instance.EndPharseButton.SetActive(true);
    }

    void EnableWalk()
    {
        this.GetComponent<Character>().WalkAble = true;
    }

}
