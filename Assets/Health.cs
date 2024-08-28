using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int Maxhealth;


    public void TakeDamage(int Damage)
    {
        Maxhealth -= Damage;
        if(Maxhealth <= 0)
        {
            Died();
        }
    }


    void Died()
    {
        TurnBaseSystem.instance.turnSystems.Remove(this.GetComponent<EnemyTurnBehaviour>());
        Destroy(this.gameObject);

    }
}
