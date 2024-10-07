using TMPro;
using UnityEngine;

public enum PlayerActionUiButton
{
    Walk = 0,
    Attack = 1,
    Build = 2,
    SKill = 3,
    EndTurn = 4


}

public class PlayerActionUI : MonoBehaviour
{
    bool enable = false;

    public GameObject cam;


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

    private void Start()
    {
        cam = GameObject.Find("CameraHolder");
    }


    private void Update()
    {
        if (Troops != null)
        {
            MpText.text = "MP: " + Troops.Status.AvalibleMoveStep.ToString();
            ApText.text = "AP: " + Troops.Status.AvalibleActionPoint.ToString();
        }

    }
    public void WalkButton()
    {
        Troops.WalkingButton();
    }
    public void AttackButton()
    {
        Troops.AttackingButton();
    }

    public void EndPhase()
    {
        CameraBehaviouerControll.instance.ResetTransform();
        EnableUI = false;
        TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter = null;
        TurnBaseSystem.instance.ActionEnd = true;
    }

    public void OnMouseEnter()
    {
        cam.GetComponent<Interact>().enabled = false;
    }

    public void OnMouseExit()
    {
        cam.GetComponent<Interact>().enabled = true;
    }


}
