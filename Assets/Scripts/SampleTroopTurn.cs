using System.Collections.Generic;

public class SampleTroopTurn : EntityTurnBehaviour
{
    Character character;

    private void Start()
    {
        character = this.gameObject.GetComponent<Character>();
        TurnBaseSystem.instance.playerTurnSystems.Add(this);

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
        List<PlayerActionUiButton> ButtonToUse = new List<PlayerActionUiButton>();

        IsPreviosBattlePhase = TurnBaseSystem.instance.OnBattlePhase;
        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            ButtonToUse.Add(PlayerActionUiButton.Walk);
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



    public override void OnActionEnd()
    {

        if (TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter != null) return;
        ShowMoveingRange.instance.CloseMovingRangeVisual();

        CameraBehaviouerControll.instance.ResetTransform();

        if (!TurnBaseSystem.instance.OnBattlePhase)
        {
            TurnBaseSystem.instance.EndPharseButton.SetActive(true);
        }


        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            if (Status.AvalibleActionPoint > 0 || Status.AvalibleMoveStep > 0)
            {
                SelectingCharacter();
            }
            else
            {
                CameraBehaviouerControll.instance.ResetTransform();
                PlayerActionUI.instance.Troops = null;
                PlayerActionUI.instance.EnableUI = false;
                TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter = null;
                TurnBaseSystem.instance.ActionEnd = true;
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

    public override void AttackingButton()
    {

    }

    public override void WalkingButton()
    {
        PlayerActionUI.instance.enabled = false;
        PlayerActionUI.instance.enabled = false;


    }
}
