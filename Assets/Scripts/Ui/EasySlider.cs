using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EasySlider : MonoBehaviour
{
    public Slider MP;
    public Slider AP;

    private void Update()
    {
        if(TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter != null && TurnBaseSystem.instance.OnBattlePhase)
        {
            AP.maxValue = TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter.gameObject.GetComponent<EntityTurnBehaviour>().Status.ActionPoint;
            MP.maxValue = TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter.gameObject.GetComponent<EntityTurnBehaviour>().Status.moveData.MaxMove;
            AP.value = TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter.gameObject.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint;
            MP.value = TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter.gameObject.GetComponent<EntityTurnBehaviour>().Status.AvalibleMoveStep;
        }
    }
}
