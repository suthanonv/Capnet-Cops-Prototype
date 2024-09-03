using TMPro;
using UnityEngine;
public class PlayerActionUI : MonoBehaviour
{
    bool enable = false;




    public bool EnableUI
    {
        get { return enable; }
        set
        {
            enable = value;
            PlayerActionHolder.SetActive(enable);
        }
    }




    public static PlayerActionUI instance;

    [SerializeField] GameObject PlayerActionHolder;
    [SerializeField] TextMeshProUGUI MpText;
    [SerializeField] TextMeshProUGUI ApText;


    public EntityStat TroopsStat { get; set; }


    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
        if (TroopsStat != null)
        {
            MpText.text = "MP: " + TroopsStat.AvalibleMoveStep.ToString();
            ApText.text = "AP: " + TroopsStat.AvalibleActionPoint.ToString();


        }

    }
    public void WalkButton()
    {
        TurnBaseSystem.instance.PlayerInteractScript.Attacking = false;
    }
    public void AttackButton()
    {
        TurnBaseSystem.instance.PlayerInteractScript.Attacking = true;
    }


    public void EndPhase()
    {
        EnableUI = false;
        TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter = null;
        TurnBaseSystem.instance.ActionEnd = true;
    }


}
