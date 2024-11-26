using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfile : MonoBehaviour
{
    [SerializeField] GameObject Character;
    [SerializeField] Slider Slide;
    [SerializeField] Button Button;
    [SerializeField] Color color;
    [SerializeField] Image characterProfile;
    private void Start()
    {
        characterProfile = this.GetComponent<Image>();
        Slide.maxValue = Character.GetComponent<Health>().Maxhealth;
        Slide.onValueChanged.AddListener(ValueChangeCheck);
    }

    void Update()
    {
        if (Character != null)
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
        else
        {
            Slide.value = 0;

            this.GetComponent<Image>().color = color;
            Button.enabled = false;
        }
    }

    public void ValueChangeCheck(float Kuy)
    {
        if (Slide.value == Slide.maxValue) return;
        characterProfile.color = Color.red;
        StartCoroutine(changeColorBack());
    }

    public void OnClickignButton()
    {
        Character.GetComponent<EntityTurnBehaviour>().onTurn();
    }

    IEnumerator changeColorBack()
    {
        yield return new WaitForSeconds(0.2f);
        characterProfile.color = Color.white;
    }
}
