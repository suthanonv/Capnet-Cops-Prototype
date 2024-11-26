using System.Collections.Generic;
public class SampleTroopTurn : EntityTurnBehaviour
{
    Character character;

    protected override void Start()
    {
        base.Start();
        character = this.gameObject.GetComponent<Character>();
        TurnBaseSystem.instance.playerTurnSystems.Add(this);

    }


    public override void ResetState()
    {


        base.ResetState();
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
    public override void onTurn()
    {
        TurnBaseSystem.instance.PlayerInteractScript.Walking = false;
        OffAction = false;
        TurnBaseSystem.instance.ActionEnd = false;
        OpenUi();
        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            CameraBehaviouerControll.instance.LookAtTarget(this.transform);
            CameraBehaviouerControll.instance.LookAtTarget(null);

            PlayerActionUI.instance.Troops = this;
            SelectingCharacter();

        }
        else
        {
            SelectingCharacter();
        }
    }

    bool OffAction = false;
    void SelectingCharacter()
    {
        TurnBaseSystem.instance.PlayerInteractScript.enabled = true;

        PlayerActionUI.instance.EnableUI = true;
        PlayerActionUiLayOut.instance.EnableUI = true;
        OpenUi();
        TurnBaseSystem.instance.PlayerInteractScript.SelectCharacter(character); // make player can choosing a tile to moving
    }


    bool IsPreviosBattlePhase;
    public void OpenUi()
    {
        PlayerActionUI.instance.EnableUI = false;
        PlayerActionUI.instance.EnableUI = true;

        TurnBaseSystem.instance.EndPharseButton.SetActive(true);
        List<PlayerActionUiButton> ButtonToUse = new List<PlayerActionUiButton>();

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
            ButtonToUse.Add(PlayerActionUiButton.EndTurn);
        }

        PlayerActionUiLayOut.instance.ArrangementUiButton(ButtonToUse);
        PlayerActionUiLayOut.instance.EditingActionButtonName("Attacking");
    }



    public override void OnActionEnd()
    {

        if (OffAction || TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter != null || TurnBaseSystem.instance.currentTurn == Turn.Enemies && TurnBaseSystem.instance.OnBattlePhase)
        {
            DeSelect();
            return;
        }
        ShowMoveingRange.instance.CloseMovingRangeVisual();

        CameraBehaviouerControll.instance.ResetTransform();

        if (!TurnBaseSystem.instance.OnBattlePhase)
        {
            TurnBaseSystem.instance.EndPharseButton.SetActive(true);
        }
        else
        {
            TurnBaseSystem.instance.EndPharseButton.SetActive(false);
        }


        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            if (Status.AvalibleActionPoint > 0 || Status.AvalibleMoveStep > 0)
            {
                SelectingCharacter();
            }
            else
            {
                OffACtion();
            }
        }
        else if (IsPreviosBattlePhase == true && !TurnBaseSystem.instance.OnBattlePhase)
        {
            return;
        }

        else if (TurnBaseSystem.instance.OnBattlePhase == false)
        {
            SelectingCharacter();
        }
    }

    public override void OffACtion()
    {
        OffAction = true;
        this.DeSelect();
        CameraBehaviouerControll.instance.ResetTransform();
        PlayerActionUI.instance.Troops = null;
        PlayerActionUI.instance.EnableUI = false;
        TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter = null;
        TurnBaseSystem.instance.ActionEnd = true;
    }

    public override void AttackingButton()
    {
        if (TurnBaseSystem.instance.OnBattlePhase && Status.AvalibleActionPoint == 0) return;


        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            if (Status.AvalibleActionPoint > 0)
            {
                TurnBaseSystem.instance.PlayerInteractScript.Attacking = true;
            }
            else
            {
                TurnBaseSystem.instance.PlayerInteractScript.Attacking = false;

            }
        }
        TurnBaseSystem.instance.PlayerInteractScript.ClearIlustatePath();

        TurnBaseSystem.instance.PlayerInteractScript.Walking = true;
        ShowMoveingRange.instance.ShowCharacterMoveRange(this.GetComponent<Character>().characterTile, Status, this.GetComponent<EntityTeam>());
    }

    public override void WalkingButton()
    {
        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            TurnBaseSystem.instance.PlayerInteractScript.Attacking = false;
        }

        TurnBaseSystem.instance.PlayerInteractScript.ClearIlustatePath();
        TurnBaseSystem.instance.PlayerInteractScript.Walking = true;
        ShowMoveingRange.instance.ShowCharacterMoveRange(this.GetComponent<Character>().characterTile, Status, this.GetComponent<EntityTeam>());
    }
}
