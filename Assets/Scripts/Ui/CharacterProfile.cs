using UnityEngine;
using UnityEngine.UI;

public class CharacterProfile : MonoBehaviour
{
    [SerializeField] GameObject Character;
    [SerializeField] Slider Slide;
    [SerializeField] Button Button;
    private void Start()
    {
        Slide.maxValue = Character.GetComponent<Health>().Maxhealth;
    }

    void Update()
    {
        Slide.value = Character.GetComponent<Health>().Maxhealth;

        if ((TurnBaseSystem.instance.OnBattlePhase == false || (TurnBaseSystem.instance.currentTurn == Turn.Player)) && TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter != Character)
        {
            Button.enabled = true;
        }
        else
        {
            Button.enabled = false;
        }
    }

    public void OnClickignButton()
    {
        Character.GetComponent<EntityTurnBehaviour>().onTurn();
    }
}
