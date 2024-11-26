using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerActionUiButton
{
    Walk = 0,
    Attack = 1,
    Build = 2,
    SKill = 3,
    EndTurn = 4,
    MP_AP = 5

}

public class PlayerActionUI : MonoBehaviour
{
    bool enable = false;

    public GameObject cam;

    public GameObject behemoth;


    public bool EnableUI
    {
        get { return enable; }
        set
        {
            enable = value;
            PlayerActionHolder.SetActive(enable);
            PlayerActionUiLayOut.instance.EnableUI = enable;



            if (TurnBaseSystem.instance.OnBattlePhase)
            {

                MpText.gameObject.SetActive(enable);
                MpText.gameObject.SetActive(enable);
            }
            else
            {
                MpText.gameObject.SetActive(false);
                MpText.gameObject.SetActive(false);
            }
        }
    }




    public static PlayerActionUI instance;

    [SerializeField] GameObject PlayerActionHolder;
    [SerializeField] TextMeshProUGUI MpText;
    [SerializeField] TextMeshProUGUI ApText;


    public EntityTurnBehaviour Troops { get; set; }


    private void Awake()
    {
        instance = this;
    }



    private void Update()
    {

        if (Troops != null)
        {
            if (Troops.Status.AvalibleMoveStep <= 20 || Troops.Status.AvalibleActionPoint <= 20)
            {
                MpText.text = ": " + Troops.Status.AvalibleMoveStep.ToString();
                ApText.text = ": " + Troops.Status.AvalibleActionPoint.ToString();
            }

        }



    }
    public void WalkButton()
    {
        if (TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter == null) return;
        TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter.GetComponent<EntityTurnBehaviour>().WalkingButton();

    }
    public void AttackButton()
    {
        if (TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter == null) return;

        TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter.GetComponent<EntityTurnBehaviour>().AttackingButton();
    }
    public UnityEvent EndPhaseEvent = new UnityEvent();
    public void EndPhase()
    {
        EndPhaseEvent.Invoke();
        CameraBehaviouerControll.instance.ResetTransform();
        EnableUI = false;
        TurnBaseSystem.instance.ActionEnd = true;
        TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter = null;
        TurnBaseSystem.instance.ActionEnd = true;
        TurnBaseSystem.instance.PlayerInteractScript.enabled = true;
        ShowMoveingRange.instance.CloseMovingRangeVisual();


        TurnBaseSystem.instance.EndPharseButton.SetActive(true);


    }




    private void FixedUpdate()
    {

    }

    public bool IsMouseOnUI;

    public void OnMouseEnter()
    {
        IsMouseOnUI = true;
        TurnBaseSystem.instance.PlayerInteractScript.EnableInteracting = false;


    }

    public void OnMouseExit()
    {
        IsMouseOnUI = false;


        TurnBaseSystem.instance.PlayerInteractScript.EnableInteracting = true;

    }


}
