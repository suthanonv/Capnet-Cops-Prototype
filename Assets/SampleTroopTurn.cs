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
        CameraBehaviouerControll.instance.LookAtTarget(this.transform.GetChild(0));
        CameraBehaviouerControll.instance.LookAtTarget(null);

        PlayerActionUI.instance.Troops = this;
        SelectingCharacter();
    }


    void SelectingCharacter()
    {
        PlayerActionUI.instance.EnableUI = true;
        TurnBaseSystem.instance.PlayerInteractScript.Attacking = false;
        TurnBaseSystem.instance.PlayerInteractScript.SelectCharacter(character);

    }


    public override void DoingAction(int TypeOfAction)
    {

    }

    public override void OnActionEnd()
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

    public override void AttackingButton()
    {
        if (Status.AvalibleActionPoint > 0)
            TurnBaseSystem.instance.PlayerInteractScript.Attacking = true;
        else
        {
            TurnBaseSystem.instance.PlayerInteractScript.Attacking = false;
        }
    }

    public override void WalkingButton()
    {
        TurnBaseSystem.instance.PlayerInteractScript.Attacking = false;
    }
}
