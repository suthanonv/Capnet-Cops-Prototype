using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
        Invoke("Delay", 0.1f);
    }


   void Delay()
    {
        TurnBaseSystem.instance.PlayerMovingScript.SelectCharacter(character);
    }
    

    public override void DoingAction(int TypeOfAction)
    {
    }
}
