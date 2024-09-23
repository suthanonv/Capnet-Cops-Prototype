using System.Collections.Generic;

public class SampleTroopTurn : EntityTurnBehaviour
{
    Character character;

    private void Start()
    {
        character = this.gameObject.GetComponent<Character>();
        TurnBaseSystem.instance.turnSystems.Add(this);

    }

    public override void onTurn()
    {
        base.onTurn();

        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            CameraBehaviouerControll.instance.LookAtTarget(this.transform.GetChild(0));
            CameraBehaviouerControll.instance.LookAtTarget(null);

            PlayerActionUI.instance.Troops = this;
            SelectingCharacter();
        }
        else
        {
            OpenUi();
        }
    }


    void SelectingCharacter()
    {
        OpenUi();
        PlayerActionUI.instance.EnableUI = true;
        TurnBaseSystem.instance.PlayerInteractScript.Attacking = false;
        TurnBaseSystem.instance.PlayerInteractScript.SelectCharacter(character);

    }


    bool IsPreviosBattlePhase;
    public void OpenUi()
    {
        List<PlayerActionUiButton> ButtonToUse = new List<PlayerActionUiButton>();

        IsPreviosBattlePhase = TurnBaseSystem.instance.OnBattlePhase;
        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            ButtonToUse.Add(PlayerActionUiButton.Walk);
            ButtonToUse.Add(PlayerActionUiButton.Attack);
            ButtonToUse.Add(PlayerActionUiButton.EndTurn);
        }
        else
        {
            ButtonToUse.Add(PlayerActionUiButton.Walk);
            ButtonToUse.Add(PlayerActionUiButton.EndTurn);
        }

        PlayerActionUiLayOut.instance.ArrangementUiButton(ButtonToUse);
        PlayerActionUiLayOut.instance.EditingActionButtonName("Attacking");
    }



    public override void DoingAction(int TypeOfAction)
    {

    }

    public override void OnActionEnd()
    {
        if (TurnBaseSystem.instance.OnBattlePhase && IsPreviosBattlePhase)
        {
            if (Status.AvalibleActionPoint > 0 || Status.AvalibleMoveStep > 0)
            {
                SelectingCharacter();
            }
            else
            {
                PlayerActionUI.instance.Troops = null;
                PlayerActionUI.instance.EnableUI = false;
                TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter = null;
                TurnBaseSystem.instance.ActionEnd = true;
            }
        }
        else if (TurnBaseSystem.instance.OnBattlePhase == false)
        {
            SelectingCharacter();
        }

    }

    public override void AttackingButton()
    {
        PlayerActionUI.instance.enabled = false;
        if (Status.AvalibleActionPoint > 0)
            TurnBaseSystem.instance.PlayerInteractScript.Attacking = true;
        else
        {
            TurnBaseSystem.instance.PlayerInteractScript.Attacking = false;
        }
    }

    public override void WalkingButton()
    {
        PlayerActionUI.instance.enabled = false;
        TurnBaseSystem.instance.PlayerInteractScript.Attacking = false;
    }
}
